Shader "Unlit/4D Multi-View"
{
    Properties
    {
        _W ("W Axis Cross Section", Range(-3,3)) = 0
        _X ("X Canvas Position", Float) = 1
        _Y ("Y Canvas Position", Float) = 0
        _Z ("Z Canvas Position", Float) = 3

        _A  ("A Rotor Scalar", Float) = 1
        _YZ ("X Rotation", Float)  = 0
        _XZ ("Y Rotation", Float)  = 0
        _XY ("Z Rotation", Float)  = 0
        _XW ("XW Rotation", Float) = 0
        _YW ("YW Rotation", Float) = 0
        _ZW ("ZW Rotation", Float) = 0
        _XYZW ("XYZW Rotation", Float) = 0

        _Shape ("Shape", Int) = 0
        _Effect ("ShadingType", Int) = 0

        _TexX ("X Texture", 2D) = "red" {}
        _TexY ("Y Texture", 2D) = "green" {}
        _TexZ ("Z Texture", 2D) = "blue" {}
        _TexW ("W Texture", 2D) = "white" {}
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
            #include "../Rotation.cginc"

            #define MAX_STEPS 200
            #define MAX_DIST  100
            #define SURF_DIST 0.001
            #define SEP 1.5

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

                int _Shape;

                float _A;
                float _YZ;
                float _XZ;
                float _XY;  
                float _XW;
                float _YW;
                float _ZW;  
                float _XYZW; 

                int _Effect; 
            CBUFFER_END

            sampler2D _TexX;
            sampler2D _TexY;
            sampler2D _TexZ;
            sampler2D _TexW;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);

                // object space
                o.ro = mul(unity_WorldToObject, float4(_WorldSpaceCameraPos, 1) - float4(_X+1.5,-_Y-1.5,4,0));
                o.hitPos = v.vertex; 
                return o;
            }

            float2x2 RotateMat(float a) {
                float s = sin(a);
                float c = cos(a);
                return float2x2(c, s, -s ,c);
            }

            /**
             * \brief   Rotation Matrix Multiplication
             *
             * \param   mat rotation matrix
             * \param   a   axis to rotate from
             * \param   b   axis to rotate towards
             */
            float2 RotMatMul(float2 p, float2x2 mat){
                return mul(p, mat);
            }

            float4 Rotate(float4 p){
                return RotorRotate(p, _A, _XY, _XZ, _XW, _YZ, _YW, _ZW, _XYZW);
            }

            float GetDist(float4 p){
                p = p-float4(_X,_Y,_Z,_W);

                //p.w *= -1;

                //Split into 4 coordinates
                float4 p1 = p-float4( SEP, SEP, 0, 0); // Top Left      Normal
                float4 p2 = p-float4(-SEP, SEP, 0, 0); // Top Right     YW
                float4 p3 = p-float4(-SEP,-SEP, 0, 0); // Bottom Right  ZW
                float4 p4 = p-float4( SEP,-SEP, 0, 0); // Bottom left   XW

                //Rotate each view in each w direction
                float4 mat = RotateMat(3.14159/2);
                p2.yw = RotMatMul(p2.yw, mat);
                p3.zw = RotMatMul(p3.zw, mat);
                p4.xw = RotMatMul(p4.xw, mat);

                //Globally rotate
                p1 = Rotate(p1);
                p2 = Rotate(p2);
                p3 = Rotate(p3);
                p4 = Rotate(p4);

                // Box
                if (_Shape == 1){
                    float size = float4(0.6, 0.6, 0.6, 0.6);

                    float s1 = sdBox(p1, size)-0.01;
                    float s2 = sdBox(p2, size)-0.01;
                    float s3 = sdBox(p3, size)-0.01;
                    float s4 = sdBox(p4, size)-0.01;
                    
                    float d = s1;
                            d = min(d, s2);
                            d = min(d, s3);
                            d = min(d, s4);
                    return d;
                }

                // Cone along W
                if (_Shape == 2){
                    float r = 0.75;
                    float h = 1.5;
                    
                    p1 = p1-float4( 0, 0, 0,-h/2);
                    p2 = p2-float4( 0, 0, 0,-h/2);
                    p3 = p3-float4( 0, 0, 0,-h/2);
                    p4 = p4-float4( 0, 0, 0,-h/2);
                    
                    float s1 = sdConeW(p1, r, h);
                    float s2 = sdConeW(p2, r, h);
                    float s3 = sdConeW(p3, r, h);
                    float s4 = sdConeW(p4, r, h);

                    float d = s1;
                        d = min(d, s2);
                        d = min(d, s3);
                        d = min(d, s4);
                    return d;
                }

                // Cone along Y
                if (_Shape == 3){
                    float r = 0.75;
                    float h = 1.5;
                    
                    p1 = p1-float4( 0,-h/2, 0, 0);
                    p2 = p2-float4( 0,-h/2, 0, 0);
                    p3 = p3-float4( 0,-h/2, 0, 0);
                    p4 = p4-float4( 0,-h/2, 0, 0);

                    float s1 = sdConeY(p1, r, h);
                    float s2 = sdConeY(p2, r, h);
                    float s3 = sdConeY(p3, r, h);
                    float s4 = sdConeY(p4, r, h);

                    float d = s1;
                        d = min(d, s2);
                        d = min(d, s3);
                        d = min(d, s4);

                    return d;
                }

                // Torus
                if (_Shape == 4){
                    float r1 = 0.5;
                    float r2 = 0.2;
                    float r3 = 0.1;

                    float s1 = sdTorus(p1, r1, r2, r3);
                    float s2 = sdTorus(p2, r1, r2, r3);
                    float s3 = sdTorus(p3, r1, r2, r3);
                    float s4 = sdTorus(p4, r1, r2, r3);
                    
                    float d = s1;
                          d = min(d, s2);
                          d = min(d, s3);
                          d = min(d, s4);
                    return d;
                }

                // Capsule along X
                if (_Shape == 5){
                    float l = 1.8;
                    float r = 0.5;

                    float s1 = sdCapsuleW(p1, l, r);
                    float s2 = sdCapsuleW(p2, l, r);
                    float s3 = sdCapsuleW(p3, l, r);
                    float s4 = sdCapsuleW(p4, l, r);
                    
                    float d = s1;
                          d = min(d, s2);
                          d = min(d, s3);
                          d = min(d, s4);
                    return d;
                }

                //Capsule along W
                if (_Shape == 6){
                    float l = 1.8;
                    float r = 0.5;

                    float s1 = sdCapsuleX(p1, l, r);
                    float s2 = sdCapsuleX(p2, l, r);
                    float s3 = sdCapsuleX(p3, l, r);
                    float s4 = sdCapsuleX(p4, l, r);
                    
                    float d = s1;
                          d = min(d, s2);
                          d = min(d, s3);
                          d = min(d, s4);
                    return d;
                }

                //Pentachoron
                if (_Shape == 7){
                    float s = 0.5;
                    
                    float s1 = sdPentachoron(p1, s);
                    float s2 = sdPentachoron(p2, s);
                    float s3 = sdPentachoron(p3, s);
                    float s4 = sdPentachoron(p4, s);
                    
                    float d = s1;
                          d = min(d, s2);
                          d = min(d, s3);
                          d = min(d, s4);
                    return d;
                }

                // Sphere
                float r = 0.75;

                float s1 = sdSphere(p1, r);
                float s2 = sdSphere(p2, r);
                float s3 = sdSphere(p3, r);
                float s4 = sdSphere(p4, r);
                
                float d = s1;
                        d = min(d, s2);
                        d = min(d, s3);
                        d = min(d, s4);
                return d;
            }

            int GetMat(float4 p){
                p = p-float4(_X,_Y,_Z,_W);

                //p.w *= -1;

                //Split into 4 coordinates
                float4 p1 = p-float4( SEP, SEP, 0, 0); // Top Left      Normal
                float4 p2 = p-float4(-SEP, SEP, 0, 0); // Top Right     YW
                float4 p3 = p-float4(-SEP,-SEP, 0, 0); // Bottom Right  ZW
                float4 p4 = p-float4( SEP,-SEP, 0, 0); // Bottom left   XW

                //Rotate each view in each w direction
                float4 mat = RotateMat(3.14159/2);
                p2.yw = RotMatMul(p2.yw, mat);
                p3.zw = RotMatMul(p3.zw, mat);
                p4.xw = RotMatMul(p4.xw, mat);

                //Globally rotate
                p1 = Rotate(p1);
                p2 = Rotate(p2);
                p3 = Rotate(p3);
                p4 = Rotate(p4);

                // Box
                if (_Shape == 1){
                    float size = float4(0.6, 0.6, 0.6, 0.6);

                    float s1 = sdBox(p1, size)-0.01;
                    float s2 = sdBox(p2, size)-0.01;
                    float s3 = sdBox(p3, size)-0.01;
                    float s4 = sdBox(p4, size)-0.01;
                    
                    float d = s1;
                            d = min(d, s2);
                            d = min(d, s3);
                            d = min(d, s4);
                }

                // Cone along W
                if (_Shape == 2){
                    float r = 0.75;
                    float h = 1.5;
                    
                    p1 = p1-float4( 0, 0, 0,-h/2);
                    p2 = p2-float4( 0, 0, 0,-h/2);
                    p3 = p3-float4( 0, 0, 0,-h/2);
                    p4 = p4-float4( 0, 0, 0,-h/2);
                    
                    float s1 = sdConeW(p1, r, h);
                    float s2 = sdConeW(p2, r, h);
                    float s3 = sdConeW(p3, r, h);
                    float s4 = sdConeW(p4, r, h);

                    float d = s1;
                        d = min(d, s2);
                        d = min(d, s3);
                        d = min(d, s4);
                }

                // Cone along Y
                if (_Shape == 3){
                    float r = 0.75;
                    float h = 1.5;
                    
                    p1 = p1-float4( 0,-h/2, 0, 0);
                    p2 = p2-float4( 0,-h/2, 0, 0);
                    p3 = p3-float4( 0,-h/2, 0, 0);
                    p4 = p4-float4( 0,-h/2, 0, 0);

                    float s1 = sdConeY(p1, r, h);
                    float s2 = sdConeY(p2, r, h);
                    float s3 = sdConeY(p3, r, h);
                    float s4 = sdConeY(p4, r, h);

                    float d = s1;
                        d = min(d, s2);
                        d = min(d, s3);
                        d = min(d, s4);
                }

                // Torus
                if (_Shape == 4){
                    float r1 = 0.5;
                    float r2 = 0.2;
                    float r3 = 0.1;

                    float s1 = sdTorus(p1, r1, r2, r3);
                    float s2 = sdTorus(p2, r1, r2, r3);
                    float s3 = sdTorus(p3, r1, r2, r3);
                    float s4 = sdTorus(p4, r1, r2, r3);
                    
                    float d = s1;
                          d = min(d, s2);
                          d = min(d, s3);
                          d = min(d, s4);
                }

                // Capsule along X
                if (_Shape == 5){
                    float l = 1.8;
                    float r = 0.5;

                    float s1 = sdCapsuleW(p1, l, r);
                    float s2 = sdCapsuleW(p2, l, r);
                    float s3 = sdCapsuleW(p3, l, r);
                    float s4 = sdCapsuleW(p4, l, r);
                    
                    float d = s1;
                          d = min(d, s2);
                          d = min(d, s3);
                          d = min(d, s4);
                }

                //Capsule along W
                if (_Shape == 6){
                    float l = 1.8;
                    float r = 0.5;

                    float s1 = sdCapsuleX(p1, l, r);
                    float s2 = sdCapsuleX(p2, l, r);
                    float s3 = sdCapsuleX(p3, l, r);
                    float s4 = sdCapsuleX(p4, l, r);
                    
                    float d = s1;
                          d = min(d, s2);
                          d = min(d, s3);
                          d = min(d, s4);
                }

                // Sphere
                float r = 0.75;

                float s1 = sdSphere(p1, r);
                float s2 = sdSphere(p2, r);
                float s3 = sdSphere(p3, r);
                float s4 = sdSphere(p4, r);
                
                float d = s1;
                        d = min(d, s2);
                        d = min(d, s3);
                        d = min(d, s4);
                
                // ASSIGN MATERIAL
                int m = 0;
                if (d == s1)    m = 1;
                if (d == s2)    m = 2;
                if (d == s3)    m = 3;                
                if (d == s4)    m = 4;
                return m;
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

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;               // UV coordinates - centered on object
                float4 ro = float4(i.ro, 0);    // Ray Origin - Camera
                float4 rd = normalize(float4(i.hitPos, 0) - ro); // Ray Direction

                float d = Raymarch(ro, rd); // Distance

                // Shading
                fixed4 col = 0;

                // Colour in the cube based on ray march
                if (d > MAX_DIST)
                    //col.rgb = float3(0.28,0.28,0.28);
                    discard;
                else {
                    float4 p = ro + rd * d;

                    float4 offset = p-float4(SEP+_X, SEP+_Y, _Z, 0);
                    float3 colyzw = tex2D(_TexX, Rotate(offset).yzw).rgb; // X
                    float3 colzxw = tex2D(_TexY, Rotate(offset).zxw).rgb; // Y
                    float3 colxyw = tex2D(_TexZ, Rotate(offset).xyw).rgb; // Z
                    float3 colxyz = tex2D(_TexW, Rotate(offset).xyz).rgb; // W

                    float4 n = Rotate(-GetNormal(p));
                    if (_Shape == 7) n *= -1;

                    float dif = dot(GetNormal(p), 
                                    normalize(float3(1,2,3))) * .5 +.5;

                    //Colours - RGBW
                    if (_Effect == 1){
                        float3 rgbw = clamp(((n.xyz) + n.w)/2, 0, 1);
                        col.rgb = dif/2 + 2*rgbw;
                    }
                    //Textures - Directional
                    else if (_Effect == 2){
                        float4 np = pow(n,4);
                        col.rgb = clamp(
                                  colyzw * np.x + colzxw * np.y +
                                  colxyw * np.z + colxyz * np.w,
                                  0, 1);

                        col.rgb += clamp(dif/3, 0, 1);

                        //Colour based on direction
                        if ( dot(float4(1,1,1,1), n) > 0.5)
                            col.rgb *= float3(1,0.5,0.5);
                        else if (dot(float4(1,1,1,1), n) < 0.5)
                            col.rgb *= float3(0.5,0.5,1);
                    }
                    //Textures - RGBW
                    else if (_Effect == 3){
                        float4 np = abs(pow(n,3));
                        col.rgb = clamp(
                                  colyzw * np.x + colzxw * np.y +
                                  colxyw * np.z + colxyz * np.w,
                                  0, 1);

                        col.rgb *= clamp(n.xyz + n.w, 0, 0.8)+0.2;
                    }
                    //Normal Diffuse
                    else{
                        col.rgb = dif;
                    }

                    int mat = GetMat(p);
                    if (mat == 2) col.rgb = dif*float3(0,1,0);
                    if (mat == 3) col.rgb = dif*float3(0,0,1);
                    if (mat == 4) col.rgb = dif*float3(1,0,0);
                }

                return col;
            }
            ENDCG
        }
    }
}
