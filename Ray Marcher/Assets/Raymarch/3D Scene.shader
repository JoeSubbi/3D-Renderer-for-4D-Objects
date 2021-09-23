Shader "Unlit/Scene"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Effect ("ShadingType", Int) = 2
        _Y ("Y Axis Cross Section", Range(-0.1,2)) = 0
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
                int _Effect;  
                float _Y;          
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
             * \brief   Shape distance function for an infinite plane
             *
             * \param   p   center point of object
             */
            float sdPlaneInf(float3 p){
                float d = p.y;
                return d;
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
            float sdCapsule( float3 p, float3 a, float3 b, float r){
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
            float sdCylinder( float3 p, float3 a, float3 b, float r){
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

            float4 Rotate(float a) {
                float s = sin(a);
                float c = cos(a);
                return float4(c, -s, s ,c);
            }

            float GetDist(float3 p){
                //Define shapes
                float plane = sdBox( p-float3(0, -0.05, 0), float3(1.5,0.1,1.5));

                float sphere = sdSphere(    p-float3(0.9, 0.4, 0.5), 
                                            0.4);
                float torus  = sdTorus(     p-float3(-0.2, 0.15, -0.5), 
                                            0.4, 0.15);
                
                float3 bp = p-float3(1, 0.5, -1);
                //bp.xz *= Rotate(_Time);
                //bp.yz *= Rot(_Time);
                
                float box     = sdBox(      bp,
                                            float3(0.5,0.5,0.5));
                float capsule = sdCapsule(  p, 
                                            float3(-0.6, 0.1, 0.4), 
                                            float3(-1.1, 0.1, -0.1), 
                                            0.1);
                float cylinder = sdCylinder(p, 
                                            float3(0, 0, 0.7), 
                                            float3(0, 0.3, 0.7), 
                                            0.3);
                
                // Union all shapes
                float d = min(sphere, torus);
                d = min(d, box);
                d = min(d, capsule);
                d = min(d, cylinder);
                d = min(d, plane);

                //cross section
                float slice = sdBox( p-float3(0, _Y, 0), float3(1.5,0.001,1.5));
                d = max(slice, d);

                return d;
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

            float GetLight(float3 p){
                float3 lightPos = float3(0,2,0);

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
                    
                    float3 n = GetNormal(p);             // Normal
                    float3 l = GetLight(p);              // Light
                    float3 c = GetNormal(p)*GetLight(p); //Lit Normal

                    int effect = _Effect;
                    if (effect == 2) col.rgb = c;
                    else if (effect > 2) col.rgb = l;
                    else if (effect < 2) col.rgb = n;
                    //col.rgb = l;
                }

                return col;
            }
            ENDCG
        }
    }
}
