Shader "Unlit/3D"
{
    Properties
    {
        _Z ("Z Axis Cross Section", Range(-2,2)) = 0
        _Shape ("Shape", Int) = 0
        
        _ZY ("X Rotation", Float) = 0
        _XZ ("Y Rotation", Float) = 0
        _XY ("Z Rotation", Float) = 0
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
            #include "../Shapes.cginc"

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
                float _Z;
                int _Shape;
                
                float _XZ;
                float _ZY;
                float _XY;
            CBUFFER_END

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                // object space
                o.ro = mul(unity_WorldToObject, float4(_WorldSpaceCameraPos, 1));
                o.hitPos = v.vertex;
                return o;
            }

            /**
             * \brief   Shape distance function for an 2 sided infinite plane 
             *          to create a narrow cross section
             *
             * \param   p   center point of object
             */
            float crossSection(float3 p){
                float d = max(p.y+0.01, -p.y-0.01);
                return d;
            }

            float3 Rotate(float3 p){
                float a = _XZ;
                float b = _ZY;
                float c = _XY;
                p.xz = mul(p.xz, float2x2(cos(a), sin(a), -sin(a), cos(a)));
                p.zy = mul(p.zy, float2x2(cos(b), sin(b), -sin(b), cos(b)));
                p.xy = mul(p.xy, float2x2(cos(c), sin(c), -sin(c), cos(c)));
                return p;
            }

            // Pick a shape based on the integer property
            float Shape(float3 p){
                p.z-=_Z;
                if (_Shape == 1){
                    p -= float3(0,0.4,0);
                    p = Rotate(p);
                    return sdBox(p, float3(0.9,0.9,0.9))-0.005;
                }
                if (_Shape == 2){
                    p -= float3(0,0.4,0);
                    p = Rotate(p);
                    p -= float3(0,0,-1);
                    return sdConeZ(p, 1, 2);
                }
                if (_Shape == 3){
                    p -= float3(0,0.4,0);
                    p = Rotate(p);
                    p -= float3(0,-1,0);
                    return sdConeY(p, 1, 2);
                }
                if (_Shape == 4){
                    p -= float3(0,0.5,0);
                    p = Rotate(p);
                    return sdTorus(p, 1, 0.5);
                }
                if (_Shape == 5){
                    p -= float3(0,0.6,0.4);
                    p = Rotate(p);
                    return sdCapsuleZ(p, 1.5, 0.4);
                }
                if (_Shape == 6){
                    p -= float3(0,0.6,0.4);
                    p = Rotate(p);
                    return sdCapsuleX(p, 1.5, 0.4);
                }
                if (_Shape == 7){
                    p -= float3(0,0.4,0);
                    p = Rotate(p);
                    return sdTetrahedron(p, 0.8);
                }
                p = Rotate(p);
                return sdSphere(p, 1);
            }

            // Distance to point p from the camera
            float GetDist(float3 p){
                p = p-float3(-1,0,-4.5);
                
                // 3D COMPONENT
                float shape = Shape(p);

                // ENVIRONMENT
                float floor = p.y+1;
                float playArea = sdBox(p-float3(0,-1,-0.3), float3(2,0.01,3.6));
                
                // BUILD SCENE
                float d = min(floor, shape);
                      d = min(d, playArea);

                return d;
            }

            // Distance including the cross section plane - overlay the 2 for a transparent effect
            float GetDist2(float3 p){
                p = p-float3(-1,0,-4.5);
                
                // 3D COMPONENT
                float shape = Shape(p);

                // ENVIRONMENT
                float floor = p.y+1;
                float cs = sdBox(p, float3(2,5,0));
                float playArea = sdBox(p-float3(0,-1,-0.3), float3(2,0.01,3.6));
                
                // BUILD SCENE
                float d = min(floor, shape);
                      d = min(d, playArea);
                      d = min(d, cs);

                return d;
            }

            // Assign materials based on the distance
            int GetMat(float3 p){
                p = p-float3(-1,0,-4.5);
                
                // 3D COMPONENT
                float shape = Shape(p);

                // ENVIRONMENT
                float floor = p.y+1;
                float cs = sdBox(p, float3(2,5,0));
                float playArea = sdBox(p-float3(0,-1,-0.3), float3(2,0.01,3.6));
                
                // BUILD SCENE
                float d = min(floor, shape);
                      d = min(d, playArea);
                      d = min(d, cs);

                // ASSIGN MATERIAL
                int mat = 0;
                if (d == shape)    mat = 1;
                if (d == floor)     mat = 2;
                if (d == cs)       mat = 3;
                if (d == playArea)  mat = 4;
                return mat;
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

            /**
             * \brief   Raymarch function
             *
             * \param   ro  Ray origin - camera
             * \param   rd  Ray direction
             */
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
            // Second raymatch uses GetDist2. Run both ray marches and overlay the 2 results for a transparent effect
            float Raymarch2(float3 ro, float3 rd){
                float dO = 0; // Distance from Origin
                float dS;     // Distance from Surface

                for( int i=0; i<MAX_STEPS; i++ ){
                    float3 p = ro + dO * rd;
                    dS = GetDist2(p);
                    dO += dS;
                    if (dS < SURF_DIST || dO > MAX_DIST) break;
                }
                return dO;
            }

            // light and shadow of objects
            float GetLight(float3 p){
                float3 lightPos = float3(-3,4,0.5);

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

            // fragment Shader
            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;                     // UV coordinates - centered on object
                float3 ro = i.ro;                     // Ray Origin - Camera
                float3 rd = normalize(i.hitPos - ro); // Ray Direction

                float d1 = Raymarch(ro, rd);           // Distance to a surface
                float d2 = Raymarch2(ro, rd);         // Distance to a surface

                // Shading
                fixed4 col = 0;
                // Use the distance to colour the shader. 
                // If the ray didn't hit anything discard the pixel.
                if (d1 > MAX_DIST && d2 > MAX_DIST)
                    discard;
                else {
                    float3 p1 = ro + rd * d1;
                    float3 p2 = ro + rd * d2;
                    float3 n = GetNormal(p1);

                    // LIGHTING
                    float dif = dot(n, normalize(float3(1,2,3))) * .5 +.5;
                    col.rgb = float3(dif,dif,dif);

                    // MATERIALS
                    int mat1 = GetMat(p1);
                    int mat2 = GetMat(p2);
                    if (mat1 == 1) col.rgb *= float3(0.8,0.2,0.2);
                    if (mat1 == 1 && mat2 == 3) col.rgb += 0.2;
                    if (mat1 == 2 && mat2 == 2) col.rgb  = (GetLight(p1) / 5) + 0.5;
                    if (mat1 == 2 && mat2 == 3) col.rgb  = (GetLight(p1) / 5) + 0.7;
                    if (mat1 == 4 && mat2 == 4) col.rgb  = (GetLight(p1) / 5) + 0.6;
                    if (mat1 == 4 && mat2 == 3) col.rgb  = (GetLight(p1) / 5) + 0.8;
                }
                return col;
            }
            ENDCG
        }
    }
}
