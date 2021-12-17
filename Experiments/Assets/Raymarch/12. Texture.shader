﻿Shader "Unlit/Scene"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TexYZ ("YZ Texture", 2D) = "red" {}
        _TexXZ ("XZ Texture", 2D) = "green" {}
        _TexXY ("XY Texture", 2D) = "blue" {}
        _TexWX ("WX Texture", 2D) = "white" {}
        _TexWY ("WY Texture", 2D) = "white" {}
        _TexWZ ("WZ Texture", 2D) = "white" {}

        _Effect ("ShadingType", Int) = 2
        _W ("W Axis Cross Section", Range(-3,3)) = 0
        
        _A ("A Scalar Rotation", Float) = 1
        _XZ ("X to Z Rotation", Float) = 0
        _XY ("X to Y Rotation", Float) = 0
        _YZ ("Y to Z Rotation", Float) = 0
        _XW ("W to X Rotation", Float) = 0
        _YW ("W to Y Rotation", Float) = 0
        _ZW ("W to Z Rotation", Float) = 0
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

            sampler2D _TexYZ;
            sampler2D _TexXZ;
            sampler2D _TexXY;
            sampler2D _TexWX;
            sampler2D _TexWY;
            sampler2D _TexWZ;
            float4 _TexYZ_ST;
            float4 _TexXZ_ST;
            float4 _TexXY_ST;
            float4 _TexWX_ST;
            float4 _TexWY_ST;
            float4 _TexWZ_ST;

            // To make the Unity shader SRP Batcher compatible, declare all
            // properties related to a Material in a a single CBUFFER block with 
            // the name UnityPerMaterial.
            CBUFFER_START(UnityPerMaterial)
                // The following line declares the _BaseColor variable, so that you
                // can use it in the fragment shader.
                int _Effect;  
                float _W;    
                float _A; 
                float _XZ;
                float _XY;
                float _YZ;
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
            
            float sdTetrahedron2(float4 p, float s){
                float a = +p.x +p.y -p.z -p.w;
                float b = -p.x -p.y -p.z -p.w;
                float c = +p.x -p.y +p.z -p.w;
                float d = -p.x +p.y +p.z -p.w;
                float e = p.w;
                return (max(max(max(a,b),max(c,d)), e)-s)/sqrt(5.);
            }

            /**
             * \brief   Shape distance function for a capsule
             *
             * \param   p   center point of object
             * \param   a   origin of first sphere cap
             * \param   b   origin of second sphere cap
             * \param   r   radius of capsule
             */
            float sdCapsule( float4 p, float4 a, float4 b, float r )
            {
                float4 pa = p - a, ba = b - a;
                float h = clamp( dot(pa,ba)/dot(ba,ba), 0.0, 1.0 );
                return length( pa - ba*h ) - r;
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

            /*
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
            }*/
            float4 Rotate(float4 a){ 

                float s = _A;
                float bxy = _XY;
                float bxz = _XZ;
                float bxw = _XW;
                float byz = _YZ;
                float byw = _YW;
                float bzw = _ZW;
                float bxyzw = _XYZW;
                
                float4 r;
                
                r.x = ( 
                      2 * a.w * bxw * s
                    + 2 * a.w * bxy * byw
                    + 2 * a.w * bxz * bzw
                    + 2 * a.w * byz * bxyzw
                    - a.x * bxw * bxw
                    - a.x * bxy * bxy
                    - a.x * bxz * bxz
                    + a.x * byw * byw
                    + a.x * byz * byz
                    + a.x * bzw * bzw
                    - a.x * bxyzw * bxyzw
                    + a.x * s * s
                    - 2 * a.y * bxw * byw
                    + 2 * a.y * bxy * s
                    - 2 * a.y * bxz * byz
                    + 2 * a.y * bzw * bxyzw
                    - 2 * a.z * bxw * bzw
                    + 2 * a.z * bxy * byz
                    + 2 * a.z * bxz * s
                    - 2 * a.z * byw * bxyzw
                );
                r.y = (
                    - 2 * a.w * bxw * bxy
                    - 2 * a.w * bxz * bxyzw
                    + 2 * a.w * byw * s
                    + 2 * a.w * byz * bzw
                    - 2 * a.x * bxw * byw
                    - 2 * a.x * bxy * s
                    - 2 * a.x * bxz * byz
                    - 2 * a.x * bzw * bxyzw
                    + a.y * bxw * bxw
                    - a.y * bxy * bxy
                    + a.y * bxz * bxz
                    - a.y * byw * byw
                    - a.y * byz * byz
                    + a.y * bzw * bzw
                    - a.y * bxyzw * bxyzw
                    + a.y * s * s
                    + 2 * a.z * bxw * bxyzw
                    - 2 * a.z * bxy * bxz
                    - 2 * a.z * byw * bzw
                    + 2 * a.z * byz * s
                );
                r.z = (
                    - 2 * a.w * bxw * bxz
                    + 2 * a.w * bxy * bxyzw
                    - 2 * a.w * byw * byz
                    + 2 * a.w * bzw * s
                    - 2 * a.x * bxw * bzw
                    + 2 * a.x * bxy * byz
                    - 2 * a.x * bxz * s
                    + 2 * a.x * byw * bxyzw
                    - 2 * a.y * bxw * bxyzw
                    - 2 * a.y * bxy * bxz
                    - 2 * a.y * byw * bzw
                    - 2 * a.y * byz * s
                    + a.z * bxw * bxw
                    + a.z * bxy * bxy
                    - a.z * bxz * bxz
                    + a.z * byw * byw
                    - a.z * byz * byz
                    - a.z * bzw * bzw
                    - a.z * bxyzw * bxyzw
                    + a.z * s * s
                    
                );
                r.w = (
                    - a.w * bxw * bxw
                    + a.w * bxy * bxy
                    + a.w * bxz * bxz
                    - a.w * byw * byw
                    + a.w * byz * byz
                    - a.w * bzw * bzw
                    - a.w * bxyzw * bxyzw
                    + a.w * s * s
                    - 2 * a.x * bxw * s
                    + 2 * a.x * bxy * byw
                    + 2 * a.x * bxz * bzw
                    - 2 * a.x * byz * bxyzw
                    - 2 * a.y * bxw * bxy
                    + 2 * a.y * bxz * bxyzw
                    - 2 * a.y * byw * s
                    + 2 * a.y * byz * bzw
                    - 2 * a.z * bxw * bxz
                    - 2 * a.z * bxy * bxyzw
                    - 2 * a.z * byw * byz
                    - 2 * a.z * bzw * s
                );

                return r;
            }

            float GetDist(float4 p){

                //Translate box
                float4 bp = p-float4(0,0,0,_W);
                //Rotate box according to shader parameters
                bp = Rotate(bp);
                
                //float d = sdSphere( p-float4(0,0,0,_W), 1);
                //float d = sdBox( bp, float4(1,1,1,1)) - 0.01;
                //float d = sdOctahedron(bp, 1) - 0.001;
                //float d = sdTetrahedron(bp, 1) - 0.05;
                //float d = sdTorus(bp, 1, 0.4, 0.1);
                
                //float d =  sdTetrahedron2(bp, 0.5); //pentachoron(bp, 0.5);  //sdTetrahedron2(bp, 1);
                float d = sdCone(bp, 0.5, 1);
                
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
                    float dif = dot(GetNormal(p), normalize(float3(1,2,3))) * .5 +.5;
                    
                    //Light
                    if (_Effect == 1) 
                        col.rgb = GetLight(p);
                    if (_Effect == 2)
                        col.rgb = dif;
                    //3D Normals - classic Rx Gy Bz
                    if (_Effect == 3) 
                        col.rgb = RotatedNormals(p);
                    //3D Normals with lighting
                    if (_Effect == 4) 
                        col.rgb = dif/2 + 2*clamp(RotatedNormals(p)/3, 0, 1);
                    
                    //RGBW - W is light by white
                    if (_Effect == 5)
                        col.rgb = (RotatedNormals(p).xyz / 2) + RotatedNormals(p).w;
        
                    //RGBW with lighting
                    if (_Effect == 6)
                        col.rgb = dif/2 + 2*clamp(((RotatedNormals(p).xyz) + RotatedNormals(p).w)/2, 0, 1);

                    //Only W is lit - red one direction, blue the other
                    if (_Effect == 7) {
                        float t = 0.0001;
                        if (RotatedNormals(p).w > t) col.rgb = float3(1,0,0);
                        else if (RotatedNormals(p).w < -t) col.rgb = float3(0,0,1);
                        else col.rgb = dif;
                    }
                    //Combos - Only show when w is mixed with another direction.
                    //Not good
                    if (_Effect == 8) {
                        col.rgb = dif;
                        float4 n = RotatedNormals(p);
                        float xw = n.x * n.w;
                        float yw = n.y * n.w;
                        float zw = n.z * n.w;
                        col.rgb *= float3(xw, yw, zw);
                    }
                    //Forward and Backward
                    if (_Effect == 9) {
                        if ( dot(float4(1,1,1,1), RotatedNormals(p)) > 0.5)
                            col.rgb = dif*float3(1,0.5,0.5);
                        else if (dot(float4(1,1,1,1), RotatedNormals(p)) < -0.5)
                            col.rgb = dif*float3(0.5,0.5,1);
                        else
                            col.rgb = dif;
                    }
                    
                    float3 colyzw = tex2D(_TexYZ, Rotate(p).yzw).rgb; // X
                    float3 colzxw = tex2D(_TexXZ, Rotate(p).zxw).rgb; // Y
                    float3 colxyw = tex2D(_TexXY, Rotate(p).xyw).rgb; // Z
                    float3 colxyz = tex2D(_TexWX, Rotate(p).xyz).rgb; // W

                    //Textures
                    if (_Effect == 10) {
                        float4 n = RotatedNormals(p);
                        
                        n = pow(n,3);
                        col.rgb = clamp(
                                  colyzw * n.x + colzxw * n.y +
                                  colxyw * n.z + colxyz * n.w,
                                  0, 1);
                    }

                    // Textures Lit
                    // Not much better than RGBW, but does show scaling
                    if (_Effect == 11) {
                        float4 n = RotatedNormals(p);
                        
                        n = pow(n,3);
                        col.rgb = clamp(
                                  colyzw * n.x + colzxw * n.y +
                                  colxyw * n.z + colxyz * n.w,
                                  0.1, 1);
                        
                        col.rgb += dif/3;
                    }

                    //Textures - directional
                    if (_Effect == 12) {
                        float4 n = RotatedNormals(p);

                        float4 np = pow(n,4);
                        col.rgb = clamp(
                                  colyzw * np.x + colzxw * np.y +
                                  colxyw * np.z + colxyz * np.w,
                                  0, 1);

                        if ( dot(float4(1,1,1,1), n) > 0.5)
                            col.rgb *= float3(1,0.5,0.5);
                        else if (dot(float4(1,1,1,1), n) < -0.5)
                            col.rgb *= float3(0.5,0.5,1);
                    }

                    //Textures - directional - lit
                    if (_Effect == 13) {
                        float4 n = RotatedNormals(p);

                        float4 np = pow(n,4);
                        col.rgb = clamp(
                                  colyzw * np.x + colzxw * np.y +
                                  colxyw * np.z + colxyz * np.w,
                                  0, 1);

                        col.rgb += clamp(dif/3, 0, 1);

                        //Colour based on direction
                        if ( dot(float4(1,1,1,1), n) > 0)
                            col.rgb *= float3(1,0.5,0.5);
                        else if (dot(float4(1,1,1,1), n) < 0)
                            col.rgb *= float3(0.5,0.5,1);
                    }

                    //Textures - Coloured RGBW
                    if (_Effect == 14) {
                        float4 n = RotatedNormals(p);

                        float4 np = abs(pow(n,3));
                        col.rgb = clamp(
                                  colyzw * np.x + colzxw * np.y +
                                  colxyw * np.z + colxyz * np.w,
                                  0, 1);

                        col.rgb *= clamp((n.xyz / 2) + n.w, 0, 0.8)+0.2;
                    }
                    /*
                    float3 colyz = tex2D(_TexYZ, Rotate(p).yz).rgb;
                    float3 colzx = tex2D(_TexXZ, Rotate(p).zx).rgb;
                    float3 colxy = tex2D(_TexXY, Rotate(p).xy).rgb;
                    float3 colwx = tex2D(_TexWX, Rotate(p).wx).rgb;
                    float3 colwy = tex2D(_TexWY, Rotate(p).wy).rgb;
                    float3 colwz = tex2D(_TexWZ, Rotate(p).wz).rgb;
                    */
                }

                return col;
            }
            ENDCG
        }
    }
}
