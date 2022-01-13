Shader "Unlit/4D"
{
    Properties
    {
        _W ("W Axis Cross Section", Range(-3,3)) = 0
        _X ("X Canvas Position", Float) = 0
        _Y ("Y Canvas Position", Float) = 0
        _Z ("Z Canvas Position", Float) = 0

        _ZY ("X Rotation", Float) = 0
        _XZ ("Y Rotation", Float) = 0
        _XY ("Z Rotation", Float) = 0
        _XW ("XW Rotation", Float) = 0
        _YW ("YW Rotation", Float) = 0
        _ZW ("ZW Rotation", Float) = 0

        _Shape ("Shape", Int) = 0
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
                float _Z;
                float _X;
                float _Y;

                int _Shape;

                float _XZ;
                float _ZY;
                float _XY;  
                float _XW;
                float _YW;
                float _ZW;   
            CBUFFER_END

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);

                // object space
                o.ro = mul(unity_WorldToObject, float4(_WorldSpaceCameraPos, 1));
                o.hitPos = v.vertex; 
                return o;
            }

            float4 Rotate(float4 p){
                float a = _XZ;
                float b = _ZY;
                float c = _XY;
                p.xz = mul(p.xz, float2x2(cos(a), sin(a), -sin(a), cos(a)));
                p.zy = mul(p.zy, float2x2(cos(b), sin(b), -sin(b), cos(b)));
                p.xy = mul(p.xy, float2x2(cos(c), sin(c), -sin(c), cos(c)));
                a = _XW;
                b = _YW;
                c = _ZW;
                p.xw = mul(p.xw, float2x2(cos(a), sin(a), -sin(a), cos(a)));
                p.yw = mul(p.yw, float2x2(cos(b), sin(b), -sin(b), cos(b)));
                p.zw = mul(p.zw, float2x2(cos(c), sin(c), -sin(c), cos(c)));
                return p;
            }

            // Pick a shape based on the integer property
            float Shape(float4 p){
                if (_Shape == 1){
                    p = Rotate(p);
                    return sdBox(p, float4(0.9,0.9,0.9,0.9))-0.01;
                }
                if (_Shape == 2){
                    p.w *= -1;
                    p = Rotate(p);
                    p.w += 1;
                    return sdConeW(p, 1, 2)-0.01;
                }
                if (_Shape == 3){
                    p = Rotate(p);
                    p.y += 1;
                    return sdConeY(p, 1, 2)-0.01;
                }
                if (_Shape == 4){
                    p = Rotate(p);
                    return sdTorus(p, 1, 0.5, 0.2);
                }
                if (_Shape == 5){
                    p = Rotate(p);
                    return sdCapsuleW(p, 2.5, 0.6);
                }
                if (_Shape == 6){
                    p = Rotate(p);
                    return sdCapsuleX(p, 2.5, 0.6);
                }
                if (_Shape == 7){
                    p = Rotate(p);
                    return sdPentachoron(p, 0.5);
                }
                p = Rotate(p);
                return sdSphere(p, 1);
            }

            float GetDist(float4 p){
                p -= float4(_X,_Y,_Z,_W);
                
                //float W_offset = 0.4;
                float W_offset = abs(_W % 0.4);

                // 3D COMPONENT
                float c = Shape(p-float4(0,0,0,0));
                float f = Shape(p-float4(0,0,0, W_offset));
                float b = Shape(p-float4(0,0,0,-W_offset));
                
                // BUILD SCENE
                float d = min(c, min(f, b));
                
                return d;
            }

            float GetDist2(float4 p){
                p -= float4(_X,_Y,_Z,_W);
                
                // 3D COMPONENT
                float c = Shape(p);
                
                // BUILD SCENE
                float d = c;
                
                return d;
            }

            int GetMat(float4 p){
                p -= float4(_X,_Y,_Z,_W);
                
                //float W_offset = 0.4;
                float W_offset = abs(_W % 0.4);

                // 3D COMPONENT
                float c = Shape(p-float4(0,0,0,0));
                float f = Shape(p-float4(0,0,0, W_offset));
                float b = Shape(p-float4(0,0,0,-W_offset));
                
                // BUILD SCENE
                float d = min(c, min(f, b));

                int mat=0;
                if (d == c) mat=0;
                else if (d == f) mat=1;
                else if (d == b) mat=2;
                
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
            float Raymarch2(float4 ro, float4 rd){
                float dO = 0; // Distance from Origin
                float dS;     // Distance from Surface

                for( int i=0; i<MAX_STEPS; i++ ){
                    float4 p = ro + dO * rd;
                    dS = GetDist2(p);
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

            float4 GetNormal2(float4 p){
                float2 e = float2(0.01, 0);

                float4 n = GetDist2(p) - float4( 
                    GetDist2(p-e.xyyy),
                    GetDist2(p-e.yxyy),
                    GetDist2(p-e.yyxy),
                    GetDist2(p-e.yyyx)
                );
                return normalize(n);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv; // UV coordinates - centered on object
                float4 ro = float4(i.ro.x, i.ro.y, i.ro.z, 0);           // Ray Origin - Camera
                float4 rd = normalize(
                    float4(i.hitPos.x, i.hitPos.y, i.hitPos.z, 0) - ro); // Ray Direction

                float d1 = Raymarch(ro, rd); // Distance
                float d2 = Raymarch2(ro, rd); // Distance

                // Shading
                fixed4 col = 0;

                // Colour in the cube based on ray march
                if (d1 > MAX_DIST && d2 > MAX_DIST)
                    discard;//col.rgb = 0.28;
                else {
                    float4 p1 = ro + rd * d1;
                    float4 p2 = ro + rd * d2;

                    float4 n = GetNormal(p1);
                    float dif1 = dot(n, normalize(float3(1,2,3))) * .5 +.5;
                    n = GetNormal2(p2);
                    float dif2 = dot(n, normalize(float3(1,2,3))) * .5 +.5;

                    col.rgb = dif2/2;

                    float glass = (dif1*2)-1;
                    
                    int mat1 = GetMat(p1);
                    if (mat1 == 0) col.rgb += dif1/2;  
                    if (mat1 == 1) col.rgb += glass * float3(1,0.1,0.1);
                    if (mat1 == 2) col.rgb += glass * float3(0.1,0.1,1);
                }
                
                return col;
            }
            ENDCG
        }
    }
}
