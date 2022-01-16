Shader "Unlit/Timeline2"
{
    Properties
    {
        _W ("W Axis Cross Section", Range(-3,3)) = 0
        _X ("X Canvas Position", Float) = 0
        _Y ("Y Canvas Position", Float) = 0
        _Z ("Z Canvas Position", Float) = 0

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

                // object space
                o.ro = mul(unity_WorldToObject, float4(_WorldSpaceCameraPos, 1));
                o.hitPos = v.vertex; 
                return o;
            }

            float4 Rotate(float4 a){
                return RotorRotate(a, _A, _XY, _XZ, _XW, _YZ, _YW, _ZW, _XYZW);
            }

            // Pick a shape based on the integer property
            float Shape(float4 p){
                //p.w *= -1;
                p = Rotate(p);
                if (_Shape == 1){
                    return sdBox(p, float4(0.9,0.9,0.9,0.9))-0.01;
                }
                if (_Shape == 2){
                    p.w += 1;
                    return sdConeW(p, 1, 2)-0.01;
                }
                if (_Shape == 3){
                    p.y += 1;
                    return sdConeY(p, 1, 2)-0.01;
                }
                if (_Shape == 4){
                    return sdTorus(p, 1, 0.5, 0.2);
                }
                if (_Shape == 5){
                    return sdTorus(p, 0.8, 0, 0.4);
                }
                if (_Shape == 6){
                    return sdCapsuleW(p, 2.5, 0.6);
                }
                if (_Shape == 7){
                    return sdCapsuleX(p, 2.5, 0.6);
                }
                if (_Shape == 8){
                    return sdPentachoron(p, 0.5);
                }
                return sdSphere(p, 1);
            }

            float GetDist(float4 p){
                p -= float4(_X,_Y,_Z,_W);
                
                // 3D COMPONENT
                float shape = Shape(p);

                // Foward
                float shape_f1 = Shape(p-float4(-3,0,0,-0.4));
                float shape_f2 = Shape(p-float4(-6,0,0,-0.8));

                // Backward
                float shape_b1 = Shape(p-float4( 3,0,0, 0.4));
                float shape_b2 = Shape(p-float4( 6,0,0, 0.8));
                
                // BUILD SCENE
                float d = shape;
                      d = min(d, shape_f1);
                      d = min(d, shape_f2);
                      d = min(d, shape_b1);
                      d = min(d, shape_b2);
                
                return d;
            }

            int GetMat(float4 p){
                p -= float4(_X,_Y,_Z,_W);
                
                // 3D COMPONENT
                float shape = Shape(p);

                // Foward
                float shape_f1 = Shape(p-float4(-3,0,0,-0.4));
                float shape_f2 = Shape(p-float4(-6,0,0,-0.8));

                // Backward
                float shape_b1 = Shape(p-float4( 3,0,0, 0.4));
                float shape_b2 = Shape(p-float4( 6,0,0, 0.8));
                
                // BUILD SCENE
                float d = shape;
                      d = min(d, shape_f1);
                      d = min(d, shape_f2);
                      d = min(d, shape_b1);
                      d = min(d, shape_b2);

                int mat=0;
                if (d == shape) mat=0;
                if (d == shape_f1 || d == shape_f2) mat = 1;
                if (d == shape_b1 || d == shape_b2) mat = 2;
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
                    discard; //col.rgb = 0.28;
                else {
                    float4 p = ro + rd * d;
                    
                    float4 offset = p-float4(_X, _Y, _Z, 0);
                    float3 colyzw = tex2D(_TexX, Rotate(offset).yzw).rgb; // X
                    float3 colzxw = tex2D(_TexY, Rotate(offset).zxw).rgb; // Y
                    float3 colxyw = tex2D(_TexZ, Rotate(offset).xyw).rgb; // Z
                    float3 colxyz = tex2D(_TexW, Rotate(offset).xyz).rgb; // W

                    float4 n = Rotate(GetNormal(p));
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
                    if (mat == 1) col.rgb = dif * float3(0,0,1);
                    if (mat == 2) col.rgb = dif * float3(1,0,0);
                }

                return col;
            }
            ENDCG
        }
    }
}
