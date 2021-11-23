Shader "Unlit/Scene"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _R ("Reflectivity", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            #define MAX_STEPS 100
            #define MAX_DIST  100
            #define SURF_DIST 0.001

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;

                float3 ro : TEXCOORD1;
                float3 hitPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            // To make the Unity shader SRP Batcher compatible, declare all
            // properties related to a Material in a a single CBUFFER block with 
            // the name UnityPerMaterial.
            CBUFFER_START(UnityPerMaterial)
                // The following line declares the _BaseColor variable, so that you
                // can use it in the fragment shader.
                fixed4 _Color;
                float _R;
            CBUFFER_END

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                // object space
                //o.ro = mul(unity_WorldToObject, float4(_WorldSpaceCameraPos, 1));
                //o.hitPos = v.vertex; 
                
                //world space
                o.ro = _WorldSpaceCameraPos;
                o.hitPos = mul(unity_ObjectToWorld, v.vertex);

                return o;
            }

            /**
             * \brief   Shape distance function for a sphere
             *
             * \param   p   center point of object
             * \param   r   radius of sphere
             */
            float sdSphere(float3 p, float r){
                float d = length(p) - r;
                return d;
            }

            /**
             * \brief   Shape distance function for a torus
             *
             * \param   p   center point of object
             * \param   r1  major radius - torus ring
             * \param   r2  minor radius - torus thickness
             */
            float sdTorus(float3 p, float r1, float r2){
                float d = length( float2(length(p.zx) - r1, p.y)) - r2;
                return d;
            }

            /**
             * \brief   Shape distance function for a box
             *
             * \param   p   center point of object
             * \param   s   float 3 of box shape
             *              x, y, z -> width, height, depth
             */
            float sdBox(float3 p, float3 s){
                float d = length(max(abs(p)-s, 0));
                return d;
            }

            /**
             * \brief   Shape distance function for a capsule
             *
             * \param   p   center point of object
             * \param   a   origin of first sphere cap
             * \param   b   origin of second sphere cap
             * \param   r   radius of capsule
             */
            float sdCapsule(float3 p, float3 a, float3 b, float r){
                float3 ab = b-a;
                float3 ap = p-a;

                float t = dot(ab, ap) / dot(ab, ab);
                t = clamp(t, 0., 1.);

                float3 c = a + t*ab;
                float d = length(p-c) - r;

                return d;
            }

            /**
             * \brief   Shape distance function for a cylinder
             *
             * \param   p   center point of object
             * \param   a   origin of first circle cap
             * \param   b   origin of second circle cap
             * \param   r   radius of cylinder
             */
            float sdCylinder(float3 p, float3 a, float3 b, float r){
                float3 ab = b-a;
                float3 ap = p-a;

                float t = dot(ab, ap) / dot(ab, ab);

                float3 c = a + t*ab;

                float x = length(p-c) - r;
                float y = (abs(t-0.5)-0.5) * length(ab);
                float e = length(max(float2(x, y), 0));
                float i = min(max(x,y), 0);

                return e+i;
            }

            /**
             * \brief   Shape distance function for a cone
             *
             * \param   r   radius at base of cone
             * \param   h   height of cone
             */
            float sdCone(float3 p, float radius, float height) {
                float2 q = float2(length(p.xz), p.y);
                float2 tip = q - float2(0, height);
                float2 mantleDir = normalize(float2(height, radius));
                float mantle = dot(tip, mantleDir);
                float d = max(mantle, -q.y);
                float projected = dot(tip, float2(mantleDir.y, -mantleDir.x));
                
                // distance to tip
                if ((q.y > height) && (projected < 0)) {
                    d = max(d, length(tip));
                }
                
                // distance to base ring
                if ((q.x > radius) && (projected > length(float2(height, radius)))) {
                    d = max(d, length(q - float2(radius, 0)));
                }
                return d;
            }

            float2x2 RotationMatrix(float a) {
                float s = sin(a);
                float c = cos(a);
                return float2x2(c, s, -s ,c);
            }

            /**
             * \brief   Rotation Matrix Multiplication for 3D rotation
             *
             * \param   mat rotation matrix
             * \param   a   axis of rotation
             * \param   b   axis of rotation
             */
            float2 RotMatMul(float2x2 mat, float2 p){
                return mul(mat, p);
            }

            float GetDist(float3 p){
                
                // Floor
                float floor = p.y+0.5;

                // Front Wheel
                float frontWheel;
                float ball = sdSphere(p-float3(1,0,0), 0.5);
                    //Subtraction
                    float frontSubL = sdSphere(p-float3(1,0, 0.45), 0.35);
                    float frontSubR = sdSphere(p-float3(1,0,-0.45), 0.35);
                    float frontSubBox = sdBox(p-float3(1,0,0), float3(0.5,0.5,0.25)) -0.01;
                    float frontSub = max( -frontSubBox, min(frontSubL, frontSubR));
                    frontWheel = max(-frontSub, ball);
                    //Bolts
                    float FWboltF = sdSphere(p-float3(1,0, 0.25), 0.1);
                    float FWboltB = sdSphere(p-float3(1,0,-0.25), 0.1);
                    frontWheel = min(frontWheel, min(FWboltF,FWboltB));

                // Back Wheel
                float backWheel;
                float pos = -1.2;
                float backWheelLeft  = sdSphere(p-float3(pos,0,-0.9), 1);
                float backWheelRight = sdSphere(p-float3(pos,0,0.9), 1);
                backWheel = max(backWheelLeft, backWheelRight)-0.03;
                    //Subtraction
                    float backSubL = sdSphere(p-float3(pos,0, 0.2), 0.35);
                    float backSubR = sdSphere(p-float3(pos,0,-0.2), 0.35);
                    float backSubBox = sdBox(p-float3(pos,0,0), float3(0.5,0.5,0.03)) -0.01;
                    float backSub = max( -backSubBox, min(backSubL, backSubR));
                    backWheel = max(-backSub, backWheel);
                    //Bolts
                    float BWboltF = sdSphere(p-float3(pos,0, 0.03), 0.1);
                    float BWboltB = sdSphere(p-float3(pos,0,-0.03), 0.1);
                    backWheel = min(backWheel, min(BWboltF,BWboltB));

                // Engine
                float engine;
                float3 ep = p-float3(0.66,0,0);
                float2x2 mat = RotationMatrix(3.14159/2);
                ep.xy = RotMatMul(mat, ep.xy);
                float steer = sdCone(ep, 0.36, 0.9);
                engine = steer;
                // Engine Block
                float3 bp = p-float3(-0.1,-1.4,0);
                float pad = 0.15+0.01;
                float bodyCurve = max( max( (length(bp.xy) - 2.21), -bp.z-pad), bp.z-pad);
                float body = sdBox(p-float3(-0.25,0.22,0), float3(0.95,0.44,pad-0.01))-0.01;
                body = max(bodyCurve, body);
                engine = min(engine, body);
                    //Subtract
                    float bolt = length((p-float3(pos, 0, 0.3)).xy) - 0.125;
                    engine = max(-bolt, engine);
                // Cones Mid
                ep = p-float3(-0.3,0,0);
                mat = RotationMatrix(-3.14159/2);
                ep.xy = RotMatMul(mat, ep.xy);
                float LowConeL = sdCone(ep-float3(0,0, 0.1), 0.1, 0.8);
                float LowConeR = sdCone(ep-float3(0,0,-0.1), 0.1, 0.8);
                engine = min(engine, min(LowConeL, LowConeR));
                // Cones Back
                ep = p-float3(pos-0.03,0.35,0);
                mat = RotationMatrix(-3.14159/2 -0.15);
                ep.xy = RotMatMul(mat, ep.xy);
                float BackConeL = sdCone(ep-float3(0,0, 0.1), 0.1, 0.8);
                float BackConeR = sdCone(ep-float3(0,0,-0.1), 0.1, 0.8);
                ep = p-float3(pos-0.03,0.28,0);
                mat = RotationMatrix(-3.14159/2 -0.15);
                ep.xy = RotMatMul(mat, ep.xy);
                float TireCone  = sdCone(ep-float3(-0.25,-0.18,0), 0.18, 0.8);
                float backCones = max(-p.x+pos-0.01, min(min(BackConeL, BackConeR), TireCone));
                engine = min(engine, backCones);
                    //Subtract
                    float wheelGap = sdCylinder(p-float3(pos, 0, 0),
                                                float3(0,0,0.01),
                                                float3(0,0,-0.01), 0.53)-0.09;
                    engine = max(engine, -wheelGap); 
                // Bar
                float3 cyl = p-float3(-0.54, -0.15, 0);
                float cylinder = max( max( (length(cyl.xy) - 0.03), -cyl.z-0.2), cyl.z-0.2);
                engine = min(engine, cylinder);

                // Window
                float window;
                float3 wp = p-float3(-0.05,-1.4,0);
                pad = 0.11;
                float canopy = max( max( (length(wp.xy) - 2.29), -wp.z-pad), wp.z-pad);
                    // Subtract
                    canopy = max(canopy, -p.y+0.35);
                    canopy = max(canopy, -p.x-1.2);
                    window = max(canopy, -wheelGap); 
                    
                float windScreen = sdCylinder(p-float3(0.7,1.2,0),
                                              float3(0,0,0.2),
                                              float3(0,0,-0.2),
                                              0.8);
                windScreen = max(windScreen, canopy);
                //Side Windows
                float3 winp = p-float3(0.3, 0.35, 0.17);
                winp *= float3(0.45, 1, 1);
                float side1 = sdSphere(winp, 0.33);
                winp = p-float3(0.3, 0.35, -0.17);
                winp *= float3(0.45, 1, 1);
                float side2 = sdSphere(winp, 0.33);
                float sides = min(side1, side2);
                    // Subtract
                    float sub = sdBox(p, float3(2, 0.35, 0.5))-0.01;
                    sides = max(sides, -sub);
                    winp = p-float3(-0.25,0.4,0);
                    mat = RotationMatrix(-3.14159/3);
                    winp.xy = RotMatMul(mat, winp.xy);
                    sub = sdBox(winp, float3(0.5,0.4,0.5))-0.01;
                    sides = max(sides, -sub);
                window = min(window, sides);

                // Walls
                float walls;
                pad = 0.17;
                float wallBlock = sdCylinder(wp, 
                                            float3(0,0, pad),
                                            float3(0,0,-pad), 2.2);
                    // Mask
                    // Triangle
                    float mask = sdBox(p-float3(-0.22,0.26,0), float3(0.3,0.3,pad))-0.01;
                    walls = mask;
                    float3 mp = p-float3(-0.47,0.5,0);
                    mat = RotationMatrix(3.14159/3);
                    mp.xy = RotMatMul(mat, mp.xy);
                    mask = sdBox(mp, float3(0.5,0.2,pad))-0.01;
                    walls = max(walls, mask);
                    // Back
                    mp = p-float3(-0.8,1.2,0);
                    mat = RotationMatrix(3.14159/2.6);
                    mp.xy = RotMatMul(mat, mp.xy);
                    mask = sdBox(mp, float3(0.6,0.55,pad))-0.01;
                    walls = min(walls, mask);
                    // Base
                    mask = sdBox(p-float3(-0.1,0.85,0), float3(0.35,0.7,pad))-0.01;
                    walls = min(walls, mask);
                    // Front
                    mp = p-float3(0.46,0.94,0);
                    mat = RotationMatrix(-3.14159/2.9);
                    mp.xy = RotMatMul(mat, mp.xy);
                    mask = sdBox(mp, float3(0.6,0.55,pad))-0.01;
                    walls = min(walls, mask);
                    // Tip
                    mask = sdBox(p-float3(1,0.6,0), float3(0.5, 0.2, pad))-0.01;
                    walls = min(walls, mask);
                walls = max(walls, wallBlock) -0.05;

                float d = floor;
                      d = min(d, frontWheel);
                      d = min(d, backWheel);
                      d = min(d, engine);
                      d = min(d, window);
                      d = min(d, walls);
                
                return d;
            }

            int GetMat(float3 p){
                // Floor
                float floor = p.y+0.5;

                // Front Wheel
                float frontWheel;
                float ball = sdSphere(p-float3(1,0,0), 0.5);
                    //Subtraction
                    float frontSubL = sdSphere(p-float3(1,0, 0.45), 0.35);
                    float frontSubR = sdSphere(p-float3(1,0,-0.45), 0.35);
                    float frontSubBox = sdBox(p-float3(1,0,0), float3(0.5,0.5,0.25)) -0.01;
                    float frontSub = max( -frontSubBox, min(frontSubL, frontSubR));
                    frontWheel = max(-frontSub, ball);
                    //Bolts
                    float FWboltF = sdSphere(p-float3(1,0, 0.25), 0.1);
                    float FWboltB = sdSphere(p-float3(1,0,-0.25), 0.1);
                    frontWheel = min(frontWheel, min(FWboltF,FWboltB));

                // Back Wheel
                float backWheel;
                float pos = -1.2;
                float backWheelLeft  = sdSphere(p-float3(pos,0,-0.9), 1);
                float backWheelRight = sdSphere(p-float3(pos,0,0.9), 1);
                backWheel = max(backWheelLeft, backWheelRight)-0.03;
                    //Subtraction
                    float backSubL = sdSphere(p-float3(pos,0, 0.2), 0.35);
                    float backSubR = sdSphere(p-float3(pos,0,-0.2), 0.35);
                    float backSubBox = sdBox(p-float3(pos,0,0), float3(0.5,0.5,0.03)) -0.01;
                    float backSub = max( -backSubBox, min(backSubL, backSubR));
                    backWheel = max(-backSub, backWheel);
                    //Bolts
                    float BWboltF = sdSphere(p-float3(pos,0, 0.03), 0.1);
                    float BWboltB = sdSphere(p-float3(pos,0,-0.03), 0.1);
                    backWheel = min(backWheel, min(BWboltF,BWboltB));

                // Engine
                float engine;
                float3 ep = p-float3(0.66,0,0);
                float2x2 mat = RotationMatrix(3.14159/2);
                ep.xy = RotMatMul(mat, ep.xy);
                float steer = sdCone(ep, 0.36, 0.9);
                engine = steer;
                // Engine Block
                float3 bp = p-float3(-0.1,-1.4,0);
                float pad = 0.15+0.01;
                float bodyCurve = max( max( (length(bp.xy) - 2.21), -bp.z-pad), bp.z-pad);
                float body = sdBox(p-float3(-0.25,0.22,0), float3(0.95,0.44,pad-0.01))-0.01;
                body = max(bodyCurve, body);
                engine = min(engine, body);
                    //Subtract
                    float bolt = length((p-float3(pos, 0, 0.3)).xy) - 0.125;
                    engine = max(-bolt, engine);
                // Cones Mid
                ep = p-float3(-0.3,0,0);
                mat = RotationMatrix(-3.14159/2);
                ep.xy = RotMatMul(mat, ep.xy);
                float LowConeL = sdCone(ep-float3(0,0, 0.1), 0.1, 0.8);
                float LowConeR = sdCone(ep-float3(0,0,-0.1), 0.1, 0.8);
                engine = min(engine, min(LowConeL, LowConeR));
                // Cones Back
                ep = p-float3(pos-0.03,0.35,0);
                mat = RotationMatrix(-3.14159/2 -0.15);
                ep.xy = RotMatMul(mat, ep.xy);
                float BackConeL = sdCone(ep-float3(0,0, 0.1), 0.1, 0.8);
                float BackConeR = sdCone(ep-float3(0,0,-0.1), 0.1, 0.8);
                ep = p-float3(pos-0.03,0.28,0);
                mat = RotationMatrix(-3.14159/2 -0.15);
                ep.xy = RotMatMul(mat, ep.xy);
                float TireCone  = sdCone(ep-float3(-0.25,-0.18,0), 0.18, 0.8);
                float backCones = max(-p.x+pos-0.01, min(min(BackConeL, BackConeR), TireCone));
                engine = min(engine, backCones);
                    //Subtract
                    float wheelGap = sdCylinder(p-float3(pos, 0, 0),
                                                float3(0,0,0.01),
                                                float3(0,0,-0.01), 0.53)-0.09;
                    engine = max(engine, -wheelGap); 
                // Bar
                float3 cyl = p-float3(-0.54, -0.15, 0);
                float cylinder = max( max( (length(cyl.xy) - 0.03), -cyl.z-0.2), cyl.z-0.2);
                engine = min(engine, cylinder);

                // Window
                float window;
                float3 wp = p-float3(-0.05,-1.4,0);
                pad = 0.11;
                float canopy = max( max( (length(wp.xy) - 2.29), -wp.z-pad), wp.z-pad);
                    // Subtract
                    canopy = max(canopy, -p.y+0.35);
                    canopy = max(canopy, -p.x-1.2);
                    window = max(canopy, -wheelGap);

                float windScreen = sdCylinder(p-float3(0.7,1.2,0),
                                              float3(0,0,0.2),
                                              float3(0,0,-0.2),
                                              0.8);
                windScreen = max(windScreen, canopy);
                //Side Windows
                float3 winp = p-float3(0.3, 0.35, 0.17);
                winp *= float3(0.45, 1, 1);
                float side1 = sdSphere(winp, 0.33);
                winp = p-float3(0.3, 0.35, -0.17);
                winp *= float3(0.45, 1, 1);
                float side2 = sdSphere(winp, 0.33);
                float sides = min(side1, side2);
                    // Subtract
                    float sub = sdBox(p, float3(2, 0.35, 0.5))-0.01;
                    sides = max(sides, -sub);
                    winp = p-float3(-0.25,0.4,0);
                    mat = RotationMatrix(-3.14159/3);
                    winp.xy = RotMatMul(mat, winp.xy);
                    sub = sdBox(winp, float3(0.5,0.4,0.5))-0.01;
                    sides = max(sides, -sub);
                window = min(window, sides);

                // Walls
                float walls;
                pad = 0.17;
                float wallBlock = sdCylinder(wp, 
                                            float3(0,0, pad),
                                            float3(0,0,-pad), 2.2);
                    // Mask
                    // Triangle
                    float mask = sdBox(p-float3(-0.22,0.26,0), float3(0.3,0.3,pad))-0.01;
                    walls = mask;
                    float3 mp = p-float3(-0.47,0.5,0);
                    mat = RotationMatrix(3.14159/3);
                    mp.xy = RotMatMul(mat, mp.xy);
                    mask = sdBox(mp, float3(0.5,0.2,pad))-0.01;
                    walls = max(walls, mask);
                    // Back
                    mp = p-float3(-0.8,1.2,0);
                    mat = RotationMatrix(3.14159/2.6);
                    mp.xy = RotMatMul(mat, mp.xy);
                    mask = sdBox(mp, float3(0.6,0.55,pad))-0.01;
                    walls = min(walls, mask);
                    // Base
                    mask = sdBox(p-float3(-0.1,0.85,0), float3(0.35,0.7,pad))-0.01;
                    walls = min(walls, mask);
                    // Front
                    mp = p-float3(0.46,0.94,0);
                    mat = RotationMatrix(-3.14159/2.9);
                    mp.xy = RotMatMul(mat, mp.xy);
                    mask = sdBox(mp, float3(0.6,0.55,pad))-0.01;
                    walls = min(walls, mask);
                    // Tip
                    mask = sdBox(p-float3(1,0.6,0), float3(0.5, 0.2, pad))-0.01;
                    walls = min(walls, mask);
                walls = max(walls, wallBlock) -0.05;

                float d = floor;
                      d = min(d, frontWheel);
                      d = min(d, backWheel);
                      d = min(d, engine);
                      d = min(d, window);
                      d = min(d, walls);

                int material = 0; 
                if (d == frontWheel ||
                    d == cylinder  ||
                    d == backWheel ||
                    d == canopy ||
                    d == walls)
                    material = 1;
                
                if (d == backSubBox ||
                    d == frontSubBox)
                    material = 2;

                if (d == sides ||
                    d == windScreen)
                    material = 3;

                if (d == BWboltB || d == BWboltF ||
                    d == FWboltB || d == FWboltF)
                    material = 0;

                return material;
            }

            float Raymarch(float3 ro, float3 rd){
                float dO = 0; // Distance from Origin
                float dS;     // Distance from Surface

                for( int i=0; i<MAX_STEPS; i++ ){
                    float3 p = ro + dO * rd;
                    dS = GetDist(p);
                    dO += dS;
                    if (dS < SURF_DIST || dO > MAX_DIST) break;
                }
                return dO;
            }

            float3 GetNormal(float3 p){
                float2 e = float2(0.01, 0);

                float3 n = GetDist(p) - float3( 
                    GetDist(p-e.xyy),
                    GetDist(p-e.yxy),
                    GetDist(p-e.yyx)
                );
                return normalize(n);
            }

            float3 RotatedNormals(float3 p){
                float3 n = GetNormal(p);
                return n;
            }

            float GetLight(float3 p){
                float3 lightPos = float3(2,3,4);

                //angle dependant fall off
                float3 lv = normalize(lightPos-p);
                float3 n  = GetNormal(p);
                float  light  = clamp(dot(n,lv), 0., 1.);

                //shadow
                float3 so = p + n * SURF_DIST * 2.; //shadow origin
                float3 sd = normalize(lightPos-so); //light direction
                float d = Raymarch(so, sd);
                if( d<length(lightPos-p) ) light *= 0.1;

                return light;
            }

            // Fragment Shader
            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv; // UV coordinates - centered on object
                float3 ro = i.ro;                     // Ray Origin - Camera
                float3 rd = normalize(i.hitPos - ro); // Ray Direction

                float d = Raymarch(ro, rd); // Distance

                // Shading
                fixed4 col = 0;

                // Colour in the cube based on ray march
                if (d > MAX_DIST)
                    discard;
                else {
                    float3 p = ro + rd * d;
                    float3 n = GetNormal(p);

                    //Reflective surface
                    float3 r = reflect(rd, n);
                    float3 ref = tex2D(_MainTex, r).rgb;

                    //Normal Lighting
                    float dif = dot(n, normalize(float3(1,2,3))) * .5 +.5;
                    col.rgb = (dif + GetLight(p))/2;
                    
                    int mat = GetMat(p);
                    if (mat==1) col.rgb *= _Color*(((ref*_R)/3)+0.6);
                    else if (mat==2) col.rgb *= ((ref*_R)/6)+(0.1);
                    else if (mat==3) col.rgb = GetLight(p)/10;

                }

                return col;
            }
            ENDCG
        }
    }
}
