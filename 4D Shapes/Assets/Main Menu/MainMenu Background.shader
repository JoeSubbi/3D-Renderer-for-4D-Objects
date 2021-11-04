Shader "Unlit/Background"
{
    Properties
    {
        _W ("W Axis Cross Section", Range(-3,3)) = 0
        _X ("X Canvas Position", Float) = 0
        _Y ("Y Canvas Position", Float) = 0
        _Z ("Z Canvas Position", Float) = 0
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
                float _W;     
                float _Z;
                float _X;
                float _Y; 
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
             * \brief   Shape distance function for a sphere
             *
             * \param   p   center point of object
             * \param   r   radius of the sphere
             */
            float sdSphere(float4 p, float r){
                float d = length(p) - r;
                return d;
            }

            /**
             * \brief   Shape distance function for a box
             *
             * \param   p   center point of object
             * \param   s   float 3 of box shape
             *              x, y, z -> width, height, depth
             */
            float sdBox(float4 p, float4 s){
                float d = length(max(abs(p) - s, 0));
                return d;
            }

            /**
             * \brief   Shape distance function for a torus
             *
             * \param   p   center point of object
             * \param   r1  major radius - torus ring
             * \param   r2  minor radius - torus thickness
             * \param   r3  minor radius in the 4th dimension
             */
            float sdTorus(float4 p, float r1, float r2, float r3){
                float d = length(
                          float2(
                                length( 
                                float2(length(p.zx) - r1, p.y)
                                        ) - r2, p.w
                                )) - r3;
                return d;
            }

            /**
             * \brief   Shape distance function for a cone
             *
             * \param   r   radius at base of cone
             * \param   h   height of cone
             */
            float sdCone(float4 p, float r, float h) {
                float2 q = float2(length(p.xyz), p.w);
                float2 tip = q - float2(0, h);
                float2 mantleDir = normalize(float2(h, r));
                float mantle = dot(tip, mantleDir);
                float d = max(mantle, -q.y);
                float projected = dot(tip, float2(mantleDir.y, -mantleDir.x));
                
                // distance to tip
                if ((q.y > h) && (projected < 0)) {
                    d = max(d, length(tip));
                }
                
                // distance to base ring
                if ((q.x > r) && (projected > length(float2(h, r)))) {
                    d = max(d, length(q - float2(r, 0)));
                }
                return d;
            }

            /**
             * \breif   Rotate a a plane by a degrees
             * 
             * \param   p   plane to be rotated
             * \param   a   angle to rotate by
             */
            float2 Rotate(float2 p, float a){
                return mul(p, float2x2(cos(a), sin(a), -sin(a), cos(a)));
            }

            float GetDist(float4 p){
                p -= float4(_X,_Y,_Z,_W);
                p.yz = Rotate(p.yz, -3.14159/5);
                float4 rp = p;
                
                float plane   = p.y + 0.5;
                //W=0 OBJECTS
                float sphere1 = sdSphere(p-float4(-5,0,-4,0), 0.5);
                float torus1  = sdTorus(p-float4(6,-0.1,-3,0), 0.5, 0.1, 0.3);
                rp = p-float4(4,-0.5,-5,0);
                rp.yw = Rotate(rp.yw, 3.14159/2);
                float cone1   = sdCone(rp, 1, 2);
                rp = p-float4(9,0,-7,-0.5);
                rp.xw = Rotate(rp.xw,-3.14159/3);
                float cube1   = sdBox(rp, float4(0.5,0.5,0.5,0.5));

                //W=1 OBJECTS
                float sphere2 = sdSphere(p-float4(3,0.4,2,0.7), 0.8);
                float cube2   = sdBox(p-float4(-6,0,-3,0.8), float4(0.2,2,0.5,2));
                rp = p-float4(0,-0.1,0,1);
                rp.xy = Rotate(rp.xy, 3.14159/2);
                rp.yz = Rotate(rp.yz,-3.14159/4);
                rp.xw = Rotate(rp.xw, 3.14159/2);
                float torus2  = sdTorus(rp, 1, 0.4, 0.2);
                rp = p-float4(-3,0.1,2,-0.8);
                rp.xw = Rotate(rp.xw, 3.14159/3);
                rp.zw = Rotate(rp.zw, 3.14159/9);
                float cube22  = sdBox(rp, float4(0.6,0.6,0.6,0.6));
                rp = p-float4(-2,0.5,-4,2);
                rp.yw = Rotate(rp.yw, 3.14159);
                float cone2   = sdCone(rp, 1, 2);

                // W=-1 Objects
                float sphere3 = sdSphere(p-float4(-1,0.4,-1,-1.2), 0.8);
                rp = p-float4(-3,0,1,1.7);
                rp.xz = Rotate(rp.xz, 3.14159/0.5);
                rp.xw = Rotate(rp.xw, 3.14159/4);
                rp.zw = Rotate(rp.zw, 3.14159/3);
                float cube3   = sdBox(rp, float4(0.6,0.5,0.6,0.5));
                rp = p-float4(4,-0.2,1,-0.6);
                rp.yw = Rotate(rp.yw, 3.14159/4);
                float torus3 = sdTorus(rp, 0.6, 0.2, 0.1);

                float d = plane;
                      d = min(d, sphere1);
                      d = min(d, cube1);
                      d = min(d, torus1);
                      d = min(d, cone1);

                      d = min(d, sphere2);
                      d = min(d, cube2);
                      d = min(d, torus2);
                      d = min(d, cube22);
                      d = min(d, cone2);

                      d = min(d, sphere3);
                      d = min(d, cube3);
                      d = min(d, torus3);
                
                return d;
            }

            int GetMat(float4 p){
                p -= float4(_X,_Y,_Z,_W);
                p.yz = Rotate(p.yz, -3.14159/5);
                float4 rp = p;
                
                float plane   = p.y + 0.5;
                //W=0 OBJECTS
                float sphere1 = sdSphere(p-float4(-5,0,-4,0), 0.5);
                float torus1  = sdTorus(p-float4(6,-0.1,-3,0), 0.5, 0.1, 0.3);
                rp = p-float4(4,-0.5,-5,0);
                rp.yw = Rotate(rp.yw, 3.14159/2);
                float cone1   = sdCone(rp, 1, 2);
                rp = p-float4(9,0,-7,-0.5);
                rp.xw = Rotate(rp.xw,-3.14159/3);
                float cube1   = sdBox(rp, float4(0.5,0.5,0.5,0.5));

                //W=1 OBJECTS
                float sphere2 = sdSphere(p-float4(3,0.4,2,0.7), 0.8);
                float cube2   = sdBox(p-float4(-6,0,-3,0.8), float4(0.2,2,0.5,2));
                rp = p-float4(0,-0.1,0,1);
                rp.xy = Rotate(rp.xy, 3.14159/2);
                rp.yz = Rotate(rp.yz,-3.14159/4);
                rp.xw = Rotate(rp.xw, 3.14159/2);
                float torus2  = sdTorus(rp, 1, 0.4, 0.2);
                rp = p-float4(-3,0.1,2,-0.8);
                rp.xw = Rotate(rp.xw, 3.14159/3);
                rp.zw = Rotate(rp.zw, 3.14159/9);
                float cube22  = sdBox(rp, float4(0.6,0.6,0.6,0.6));
                rp = p-float4(-2,0.5,-4,2);
                rp.yw = Rotate(rp.yw, 3.14159);
                float cone2   = sdCone(rp, 1, 2);

                // W=-1 Objects
                float sphere3 = sdSphere(p-float4(-1,0.4,-1,-1.2), 0.8);
                rp = p-float4(-3,0,1,1.7);
                rp.xz = Rotate(rp.xz, 3.14159/0.5);
                rp.xw = Rotate(rp.xw, 3.14159/4);
                rp.zw = Rotate(rp.zw, 3.14159/3);
                float cube3   = sdBox(rp, float4(0.6,0.5,0.6,0.5));
                rp = p-float4(4,-0.2,1,-0.6);
                rp.yw = Rotate(rp.yw, 3.14159/4);
                float torus3 = sdTorus(rp, 0.6, 0.2, 0.1);

                float d = plane;
                      d = min(d, sphere1);
                      d = min(d, cube1);
                      d = min(d, torus1);
                      d = min(d, cone1);

                      d = min(d, sphere2);
                      d = min(d, cube2);
                      d = min(d, torus2);
                      d = min(d, cube22);
                      d = min(d, cone2);

                      d = min(d, sphere3);
                      d = min(d, cube3);
                      d = min(d, torus3);

                int mat=0;
                return mat;
            }

            float Raymarch(float4 ro, float4 rd){
                float dO = 0; // Distance from Origin
                float dS;     // Distance from Surface

                for( int i=0; i<MAX_STEPS; i++ ){
                    float4 p = ro + dO * rd;
                    dS = GetDist(p);
                    dO += dS;
                    if (dS < SURF_DIST || dO > MAX_DIST) break;
                }
                return dO;
            }

            float4 GetNormal(float4 p){
                float2 e = float2(0.01, 0);

                float4 n = GetDist(p) - float4( 
                    GetDist(p-e.xyyy),
                    GetDist(p-e.yxyy),
                    GetDist(p-e.yyxy),
                    GetDist(p-e.yyyx)
                );
                return normalize(n);
            }

            // light and shadow of objects
            float GetLight(float4 p){
                float4 lightPos = float4(0,8,4,0);

                //angle dependant fall off
                float4 lv = normalize(lightPos-p);
                float4 n  = GetNormal(p);
                float  light  = clamp(dot(n,lv), 0., 1.);

                //shadow
                float4 so = p + n * SURF_DIST * 2.; //shadow origin
                float4 sd = normalize(lightPos-so); //light direction
                float d = Raymarch(so, sd);
                if( d<length(lightPos-p) ) light *= 0.1;

                return light;
            }            

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;              // UV coordinates - centered on object
                float4 ro = float4(i.ro, 0);   // Ray Origin - Camera
                float4 rd = normalize(
                    float4(i.hitPos, 0) - ro); // Ray Direction

                float d = Raymarch(ro, rd);    // Distance

                // sample the texture
                fixed4 col = 1;

                // Colour in the cube based on ray march
                if (d > MAX_DIST)
                    col.rgb = 0.28;
                else {
                    float4 p = ro + rd * d;
                    /*
                    float4 n = GetNormal(p);
                    float dif = dot(n, normalize(float3(1,2,3))) * .5 +.5;
                    col.rgb = dif;
                    */

                    col.rgb = GetLight(p)/1.2 +0.2;

                    int mat = GetMat(p);
                }

                return col;
            }
            ENDCG
        }
    }
}
