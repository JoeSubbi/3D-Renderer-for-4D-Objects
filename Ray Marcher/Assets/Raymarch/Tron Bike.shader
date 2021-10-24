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
            #define SURF_DIST 0.0001

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
                o.ro = mul(unity_WorldToObject, float4(_WorldSpaceCameraPos, 1));
                o.hitPos = v.vertex; 
                
                //world space
                //o.ro = _WorldSpaceCameraPos;
                //o.hitPos = mul(unity_ObjectToWorld, v.vertex);

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
             * \brief   Shape distance function for a box
             *
             * \param   p   center point of object
             * \param   s   float 3 of box shape
             *              x, y, z -> width, height, depth
             * \param   r   radius of the bevel
             */
            float sdRoundedBox(float3 p, float3 s, float r){
                float d = length(max(abs(p)-s, 0)) - r;
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

            float4 RotationMatrix(float a) {
                float s = sin(a);
                float c = cos(a);
                return float4(c, s, -s ,c);
            }

            /**
             * \brief   Rotation Matrix Multiplication for 3D rotation
             *
             * \param   mat rotation matrix
             * \param   a   axis of rotation
             * \param   b   axis of rotation
             */
            float2 RotMatMul(float4 mat, float a, float b){
                float2 rot = float2( a * mat[0] + b * mat[1],
                                     a * mat[2] + b * mat[3] );
                return rot;
            }

            float GetDist(float3 p){
                
                // Front Wheel
                float3 fwp = p-float3(0.25,0,0);
                float fwheel = sdSphere(fwp, 0.1);
                fwheel = max(fwheel, -sdSphere(fwp-float3(0,0,0.1), 0.07));
                fwheel = max(fwheel, -sdSphere(fwp-float3(0,0,-0.1), 0.07));

                fwheel = min(fwheel, sdCylinder(p,
                                                float3(0.25,0,-0.055),
                                                float3(0.25,0,0.055), 0.06));
                float cap = min(
                    sdSphere(fwp-float3(0,0,0.05), 0.02),
                    sdSphere(fwp-float3(0,0,-0.05), 0.02)
                );
                fwheel = min(fwheel, cap);
                
                // Back Wheel
                float3 bwp = p-float3(-0.25,0,0);
                float bwheel = sdCylinder(p, 
                                          float3(-0.25,0,-0.005), 
                                          float3(-0.25,0,0.005), 0.09)-0.01;
                bwheel = max(bwheel, -sdCylinder(p,
                                                float3(-0.25,0,-0.03),
                                                float3(-0.25,0,0.03), 0.06));

                bwheel = min(bwheel, sdCylinder(p,
                                                float3(-0.25,0,-0.009),
                                                float3(-0.25,0,0.009), 0.06));
                cap = min(
                    sdSphere(bwp-float3(0,0,0.01), 0.02),
                    sdSphere(bwp-float3(0,0,-0.01), 0.02)
                );
                bwheel = min(bwheel, cap);

                // Engine

                float d = min(fwheel, bwheel);
                //d = min(d, stear);
                
                return d;
            }

            int GetMat(float3 p){
                int mat = 1;

                // Front Wheel
                float3 fwp = p-float3(0.25,0,0);
                float fwheel = sdSphere(fwp, 0.1);
                fwheel = max(fwheel, -sdSphere(fwp-float3(0,0,0.1), 0.07));
                fwheel = max(fwheel, -sdSphere(fwp-float3(0,0,-0.1), 0.07));

                float ball = fwheel;

                float hub = sdCylinder(p,
                                        float3(0.25,0,-0.058),
                                        float3(0.25,0,0.058), 0.06);
                fwheel = min(fwheel, hub);

                float cap = min(
                    sdSphere(p-float3(0.25,0,0.05), 0.02),
                    sdSphere(p-float3(0.25,0,-0.05), 0.02)
                );
                fwheel = min(fwheel, cap);

                // Back Wheel
                float bwheel = sdCylinder(p, 
                                          float3(-0.25,0,-0.005), 
                                          float3(-0.25,0,0.005), 0.09)-0.01;
                bwheel = max(bwheel, -sdCylinder(p,
                                                float3(-0.25,0,-0.03),
                                                float3(-0.25,0,0.03), 0.06));
                float bball = bwheel;

                float bhub = sdCylinder(p,
                                float3(-0.25,0,-0.009),
                                float3(-0.25,0,0.009), 0.06);
                bwheel = min(bwheel, bhub);

                float bcap = min(
                    sdSphere(p-float3(-0.25,0,0.01), 0.02),
                    sdSphere(p-float3(-0.25,0,-0.01), 0.02)
                );
                bwheel = min(bwheel, bcap);

                float d = min(fwheel, bwheel);

                if(d == ball || d == bball) mat = 1;
                else if(d == hub || d == bhub) mat = 2;
                else if(d == cap || d == bcap) mat = 3;

                return mat;
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
                float3 lightPos = float3(3,2,2);

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

                    //Global illumination - nicer than 1 diffuse light
                    float dif = dot(n, normalize(float3(1,2,3))) * .5 +.5;
                    col.rgb = float3(dif,dif,dif);
                    
                    int mat = GetMat(p);
                    if (mat==1) col.rgb *= _Color*(((ref*_R)/3)+0.6);
                    else if (mat==2) col.rgb *= ((ref*_R)/6)+(0.1);
                    else if (mat==3) col.rgb *= ((ref*_R)/1)+(0.4);;
                }

                return col;
            }
            ENDCG
        }
    }
}
