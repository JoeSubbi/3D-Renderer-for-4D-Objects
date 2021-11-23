﻿Shader "Unlit/Scene"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Effect ("ShadingType", Int) = 2
        _W ("W Axis Cross Section", Range(-3,3)) = 0
        // Rotation -pi to pi rad == -180 to 180 deg
        _XZ ("X to Z Rotation", Range(-3.14159, 3.14159)) = 0
        _XY ("X to Y Rotation", Range(-3.14159, 3.14159)) = 0
        _YZ ("Y to Z Rotation", Range(-3.14159, 3.14159)) = 0
        _WX ("W to X Rotation", Range(-3.14159, 3.14159)) = 0
        _WY ("W to Y Rotation", Range(-3.14159, 3.14159)) = 0
        _WZ ("W to Z Rotation", Range(-3.14159, 3.14159)) = 0
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
                float _XZ;
                float _XY;
                float _YZ;
                float _WX;
                float _WY;
                float _WZ;  
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
            
            float sdTetrahedron2(float4 p, float s){
                float a = +p.x +p.y -p.z -p.w;
                float b = -p.x -p.y -p.z -p.w;
                float c = +p.x -p.y +p.z -p.w;
                float d = -p.x +p.y +p.z -p.w;
                float e = p.w;
                return (max(max(max(a,b),max(c,d)), e)-s)/sqrt(5.);
            }

            float4 RotationMatrix(float a) {
                float s = sin(a);
                float c = cos(a);
                return float4(c, s, -s ,c);
            }

            /**
             * \brief   Rotation Matrix Multiplication
             *
             * \param   mat rotation matrix
             * \param   a   axis to rotate from
             * \param   b   axis to rotate towards
             */
            float2 RotMatMul(float4 mat, float a, float b){
                float2 rot = float2( a * mat[0] + b * mat[1],
                                     a * mat[2] + b * mat[3] );
                return rot;
            }

            float4 Rotate(float4 p){
                float4 xz = RotationMatrix(_XZ);
                float4 xy = RotationMatrix(_XY);
                float4 yz = RotationMatrix(_YZ);

                p.xz = RotMatMul(xz, p.x, p.z);
                p.yz = RotMatMul(yz, p.y, p.z);
                p.xy = RotMatMul(xy, p.x, p.y);
                
                float4 wx = RotationMatrix(_WX);
                float4 wy = RotationMatrix(_WY);
                float4 wz = RotationMatrix(_WZ);
                
                p.wx = RotMatMul(wx, p.w, p.x);
                p.wy = RotMatMul(wy, p.w, p.y);
                p.wz = RotMatMul(wz, p.w, p.z);

                return p;
            }

            float GetDist(float4 p){

                //Translate box
                float4 bp = p-float4(0,0,0,_W);
                //Rotate box according to shader parameters
                bp = Rotate(bp);
                
                //float d = sdSphere( p-float4(0,0,0,_W), 1);
                float d = sdBox( bp, float4(1,1,1,1)) - 0.01;
                //float d = sdOctahedron(bp, 1) - 0.001;
                //float d = sdTetrahedron(bp, 1) - 0.05;
                //float d = sdTorus(bp, 1, 0.4, 0.1);
                
                //float d =  sdTetrahedron2(bp, 0.5); //pentachoron(bp, 0.5);  //sdTetrahedron2(bp, 1);
                
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
                n = Rotate(n);
                return n;
            }

            float GetLight(float4 p){
                float4 lightPos = float4(2,2,2,0);

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
                    float dif = dot(RotatedNormals(p), normalize(float3(1,2,3))) * .5 +.5;
                    
                    //Light
                    if (_Effect == 1) 
                        col.rgb = GetLight(p);
                    //3D Normals - classic Rx Gy Bz
                    if (_Effect == 2) 
                        col.rgb = RotatedNormals(p);
                    //Only W is lit - red one direction, blue the other
                    if (_Effect == 3) {
                        float t = 0.0001;
                        if (RotatedNormals(p).w > t) col.rgb = float3(1,0,0);
                        else if (RotatedNormals(p).w < -t) col.rgb = float3(0,0,1);
                        else col.rgb = dif;
                    }
                    //RGBW - W is light by white
                    if (_Effect == 4)
                        col.rgb = (RotatedNormals(p).xyz / 2) + RotatedNormals(p).w;
                    //Combos - Not that good
                    if (_Effect == 5) {
                        col.rgb = dif;
                        float4 n = RotatedNormals(p);
                        float xw = n.x * n.w;
                        float yw = n.y * n.w;
                        float zw = n.z * n.w;
                        col.rgb *= float3(xw, yw, zw);
                    }
                    //RGBW with lighting
                    if (_Effect == 4)
                        col.rgb = dif/2 + 2*((RotatedNormals(p).xyz) + RotatedNormals(p).w)/3;
                }

                return col;
            }
            ENDCG
        }
    }
}