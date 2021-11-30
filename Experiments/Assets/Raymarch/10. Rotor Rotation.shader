﻿Shader "Unlit/Scene"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Effect ("ShadingType", Int) = 2
        _W ("W Axis Cross Section", Range(-3,3)) = 0
        
        _A ("A", Float) = 0
        _YZ ("Y to Z Rotation", Float) = 0
        _XZ ("Z to X Rotation", Float) = 0
        _XY ("X to Y Rotation", Float) = 0
        _XW ("X to W Rotation", Float) = 0
        _YW ("Y to W Rotation", Float) = 0
        _ZW ("Z to W Rotation", Float) = 0
        _XYZW ("Quad-Vector", Float) = 0
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

            #define MAX_STEPS 200
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
                float _W;   

                float _A;
                float _YZ;
                float _XZ;
                float _XY;
                float _XW;
                float _YW;
                float _ZW;
                float _XYZW;

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
             * \brief   Shape distance function for an octohedron
             *
             * \param   p   center point of object
             * \param   s   size of the object
             */
            float sdOctahedron( float4 p, float s){
                float c = sqrt(4); //Square root of number of dimensions
                return ((dot(abs(p), float4(1,1,1,1)) - s) / c);
            }

            /**
             * \brief   Shape distance function for a tetrahedron
             *
             * \param   p   center point of object
             * \param   s   scale of the object
             */
            float sdTetrahedron(float4 p, float s){
                return (max(
                            abs(max(
                                abs(p.x+p.y)-p.z,
                                abs(p.x-p.y)+p.z
                                )-p.w),
                            abs(max(
                                abs(p.x+p.y)-p.z,
                                abs(p.x-p.y)+p.z
                                )+p.w)
                            )-1*s
                        )/sqrt(4);
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
                                )) - 0.2;
                return d;
            }

            float4 Rotate(float4 u){
                float be1 = u.x;
                float be2 = u.y;
                float be3 = u.z;
                float be4 = u.w;

                float ae = _A;
                float ae12 = _XY;
                float ae31 = -_XZ;
                float ae23 = _YZ;
                float ae41 = -_XW;
                float ae42 = -_YW;
                float ae43 = -_ZW;
                float ae1234 = _XYZW;
                
                float4 q;
                
                /*/ Unsigned
                q.x = ae * be1 + ae12 * be2 - ae31 * be3 + ae41 * be4;
                q.y = ae * be2 - ae12 * be1 + ae23 * be3 - ae42 * be4;
                q.z = ae * be3 + ae31 * be1 - ae23 * be2 + ae43 * be4;
                q.w = ae * be4 - ae41 * be1 + ae42 * be2 - ae43 * be3;

                float q123 =  ae12 * be3 + ae31 * be2 + ae23 * be1 + ae1234 * be4;
                float q134 =  ae31 * be4 + ae41 * be3 - ae43 * be1 - ae1234 * be2;
                float q142 = -ae12 * be4 + ae41 * be2 + ae42 * be1 + ae1234 * be3;
                float q324 =  ae23 * be4 + ae42 * be3 + ae43 * be2 - ae1234 * be1;
                */
                // Signed
                q.x = ae * be1 + ae12 * be2 + ae31 * be3 - ae41 * be4;
                q.y = ae * be2 - ae12 * be1 + ae23 * be3 + ae42 * be4;
                q.z = ae * be3 - ae31 * be1 - ae23 * be2 - ae43 * be4;
                q.w = ae * be4 + ae41 * be1 - ae42 * be2 + ae43 * be3;

                float q123 =   ae12 * be3 - ae31 * be2 + ae23 * be1 + ae1234 * be4;
                float q142 = - ae12 * be4 - ae41 * be2 - ae42 * be1 + ae1234 * be3;
                float q134 = - ae31 * be4 - ae41 * be3 + ae43 * be1 - ae1234 * be2;
                float q324 =   ae23 * be4 - ae42 * be3 - ae43 * be2 - ae1234 * be1;
                //*/
                
                float be = _A;
                float be12 = _XY;
                float be31 = -_XZ;
                float be23 = _YZ;
                float be41 = -_XW;
                float be42 = -_YW;
                float be43 = -_ZW;
                float be1234 = _XYZW;

                float ae1 = q.x;
                float ae2 = q.y;
                float ae3 = q.z;
                float ae4 = q.w;
                float ae123 = -q123;
                float ae134 = -q134;
                float ae142 = -q142;
                float ae324 = -q324;

                // r = qP*

                float4 r;
                
                /*/ Unsigned 
                r.x =   ae1 * be   - ae2 * be12 + ae3 * be31                           + ae123 * be23   + ae134 * be43   + ae142 * be42   + ae324 * be1234;
                r.y =   ae1 * be12 + ae2 * be   - ae3 * be23 + ae4 * be42              + ae123 * be31   + ae134 * be1234 + ae142 * be41   - ae324 * be43;
                r.z = - ae1 * be31 + ae2 * be23 + ae3 * be   - ae4 * be43              + ae123 * be12   - ae134 * be41   + ae142 * be1234 - ae324 * be42;
                r.w =   ae1 * be41 - ae2 * be42 + ae3 * be43 + ae4 * be   - ae4 * be41 + ae123 * be1234 - ae134 * be31   - ae142 * be12   - ae324 * be23;
                //*/
                // Signed inc e1234
                r.x =   ae1 * be   + ae2 * be12 + ae3 * be31                           - ae123 * be23   + ae142 * be42   + ae134 * be43   - ae324 * be1234;
                r.y = - ae1 * be12 + ae2 * be   + ae3 * be23 + ae4 * be42              + ae123 * be31   + ae142 * be41   - ae134 * be1234 - ae324 * be43;
                r.z = - ae1 * be31 - ae2 * be23 + ae3 * be   - ae4 * be43              - ae123 * be12   - ae142 * be1234 - ae134 * be41   - ae324 * be42;
                r.w =   ae1 * be41 - ae2 * be42 + ae3 * be43 + ae4 * be   - ae4 * be41 - ae123 * be1234 + ae142 * be12   - ae134 * be31   + ae324 * be23;
                //*/
                /*/ Signed ex e1234
                r.x =   ae1 * be   + ae2 * be12 + ae3 * be31                           - ae123 * be23   + ae142 * be42   + ae134 * be43   + ae324 * be1234;
                r.y = - ae1 * be12 + ae2 * be   + ae3 * be23 + ae4 * be42              + ae123 * be31   + ae142 * be41   + ae134 * be1234 - ae324 * be43;
                r.z = - ae1 * be31 - ae2 * be23 + ae3 * be   - ae4 * be43              - ae123 * be12   + ae142 * be1234 - ae134 * be41   - ae324 * be42;
                r.w =   ae1 * be41 - ae2 * be42 + ae3 * be43 + ae4 * be   - ae4 * be41 + ae123 * be1234 + ae142 * be12   - ae134 * be31   + ae324 * be23;
                //*/
                /*/ Signed only e1234
                r.x =   ae1 * be   - ae2 * be12 + ae3 * be31                           + ae123 * be23   + ae134 * be43   + ae142 * be42   - ae324 * be1234;
                r.y =   ae1 * be12 + ae2 * be   - ae3 * be23 + ae4 * be42              + ae123 * be31   - ae134 * be1234 + ae142 * be41   - ae324 * be43;
                r.z = - ae1 * be31 + ae2 * be23 + ae3 * be   - ae4 * be43              + ae123 * be12   - ae134 * be41   - ae142 * be1234 - ae324 * be42;
                r.w =   ae1 * be41 - ae2 * be42 + ae3 * be43 + ae4 * be   - ae4 * be41 - ae123 * be1234 - ae134 * be31   - ae142 * be12   - ae324 * be23;
                */
                return r;
            }
            
            /*
            float4 Rotate(float4 u){
                // q = Px
                float4 q;
                q.x = _A * u.x + u.y * _XY + u.z * _XZ + u.w * _XW;
                q.y = _A * u.y - u.x * _XY + u.z * _YZ + u.w * _YW;
                q.z = _A * u.z - u.x * _XZ - u.y * _YZ + u.w * _ZW;
                q.w = _A * u.w - u.x * _XW - u.y * _ZW + u.w * _YW;

                float qxyz = u.x * _YZ - u.y * _XZ + u.z * _XY;
                float bxyzw = u.x * _XW - u.y * _YW + u.z * _ZW + u.w * qxyz;

                // r = qP*
                float4 r;
                r.x = _A * u.x + q.y  * _XY + q.z  * _XZ + qxyz * _YZ + q.w * _XW;
                r.y = _A * u.y - q.x  * _XY - qxyz * _XZ + q.z  * _YZ + q.w * _YW;
                r.z = _A * u.z + qxyz * _XY - q.x  * _XZ - q.y  * _YZ + q.w * _ZW;
                r.w = _A * u.w + bxyzw * _XW + bxyzw * _YW + bxyzw * _ZW + bxyzw * qxyz;
                return r;
            }*/

            float GetDist(float4 p){
                
                //Box 3D rotation

                //Translate box
                float4 bp = p-float4(0,0,0,_W);
                //Rotate box according to shader parameters
                bp = Rotate(bp);
                
                //float d = sdSphere( p-float4(0,0,0,_W), 1);
                float d = sdBox( bp, float4(1,1,1,1)) - 0.05;
                //float d = sdOctahedron(bp, 1) - 0.001;
                //float d = sdTetrahedron(bp, 1) - 0.05;
                //float d = sdTorus(bp, 1, 0.4, 0.1);
                
                return d;
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

            float4 RotatedNormals(float4 p){
                float4 n = GetNormal(p);
                n = abs(Rotate(n));
                return n;
            }

            float GetLight(float4 p){
                float4 lightPos = float4(1,1,4,0);

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

            // Fragment Shader
            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv; // UV coordinates - centered on object
                float4 ro = float4(i.ro.x, i.ro.y, i.ro.z, 0);                     // Ray Origin - Camera
                float4 rd = normalize(
                    float4(i.hitPos.x, i.hitPos.y, i.hitPos.z, 0) - ro); // Ray Direction

                float d = Raymarch(ro, rd); // Distance

                // Shading
                fixed4 col = 0;

                // Colour in the cube based on ray march
                if (d > MAX_DIST)
                    discard;
                else {
                    float4 p = ro + rd * d;
                    
                    float3 n = RotatedNormals(p);             // Normal
                    float3 l = GetLight(p);              // Light
                    float3 c = RotatedNormals(p)*GetLight(p); //Lit Normal

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
