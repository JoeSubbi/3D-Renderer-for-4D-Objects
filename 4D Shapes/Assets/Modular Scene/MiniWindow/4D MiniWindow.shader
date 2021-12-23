Shader "Unlit/4D"
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
            #include "../../Shapes.cginc"
            #include "../../Rotation.cginc"

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
                o.ro = mul(unity_WorldToObject, float4(_WorldSpaceCameraPos, 1) - float4(_X,0,0,0));
                o.hitPos = v.vertex; 
                return o;
            }

            float4 Rotate(float4 p){
                return RotorRotate(p, _A, _XY, _XZ, _XW, _YZ, _YW, _ZW, _XYZW);
            }

            // Pick a shape based on the integer property
            float Shape(float4 p){
                if (_Shape == 1){
                    p -= float4(0,0.4,0,0);
                    p = Rotate(p);
                    return sdBox(p, float3(0.9,0.9,0.9))-0.005;
                }
                if (_Shape == 2){
                    p -= float4(0,0.4,0,0);
                    p = Rotate(p);
                    p -= float4(0,0,-1,0);
                    return sdConeZ(p, 1, 2);
                }
                if (_Shape == 3){
                    p -= float4(0,0.4,0,0);
                    p = Rotate(p);
                    p -= float4(0,-1,0,0);
                    return sdConeY(p, 1, 2);
                }
                if (_Shape == 4){
                    p -= float4(0,0.5,0,0);
                    p = Rotate(p);
                    return sdTorus(p, 1, 0.5);
                }
                if (_Shape == 5){
                    p -= float4(0,0.6,0.4,0);
                    p = Rotate(p);
                    return sdCapsuleZ(p, 1.5, 0.4);
                }
                if (_Shape == 6){
                    p -= float4(0,0.6,0.4,0);
                    p = Rotate(p);
                    return sdCapsuleX(p, 1.5, 0.4);
                }
                if (_Shape == 7){
                    p -= float4(0,0.6,0.4,0);
                    p = Rotate(p);
                    return sdTetrahedron(p, 0.7);
                }
                p = Rotate(p);
                return sdSphere(p, 1);
            }

            // Distance to point p from the camera
            float GetDist(float4 p){
                p = p-float4(_X,_Y,_Z,0);
                //p.z *= -1;
                
                // 3D COMPONENT
                float shape = Shape(p);

                // ENVIRONMENT
                float floor = p.y+1;
                
                // BUILD SCENE
                float d = min(floor, shape);
                
                return d;
            }

            // Assign materials based on the distance
            int GetMat(float4 p){
                p = p-float4(_X,_Y,_Z,0);
                //p.z *= -1;
                
                // 3D COMPONENT
                float shape = Shape(p);

                // ENVIRONMENT
                float floor = p.y+1;
                
                // BUILD SCENE
                float d = min(floor, shape);
                
                // ASSIGN MATERIAL
                int mat = 0;
                if (d == shape)    mat = 1;
                if (d == floor)     mat = 2;
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

            float GetLight(float4 p){
                float4 lightPos = float4(-1,4,2,0);

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
                float2 uv = i.uv; // UV coordinates - centered on object
                float4 ro = float4(i.ro.x, i.ro.y, i.ro.z, 0);           // Ray Origin - Camera
                float4 rd = normalize(
                    float4(i.hitPos.x, i.hitPos.y, i.hitPos.z, 0) - ro); // Ray Direction

                float d = Raymarch(ro, rd); // Distance

                // Shading
                fixed4 col = 0;

                // Colour in the cube based on ray march
                if (d > MAX_DIST)
                    col = 0.7;
                else {
                    float4 p = ro + rd * d;
                    int mat = GetMat(p);
                    if (mat == 1) col.rgb = (GetLight(p)/4)+0.8*float3(0.8,0.1,0.1);
                    if (mat == 2) col.rgb = ((GetLight(p)/2)+0.7)*0.6;
                }
                return col;
            }
            ENDCG
        }
    }
}