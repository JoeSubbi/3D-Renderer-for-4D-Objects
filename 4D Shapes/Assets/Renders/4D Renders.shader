﻿Shader "Unlit/4D"
{
    Properties
    {
        _W ("W Axis Cross Section", Range(-3,3)) = 0
        _X ("X Canvas Position", Float) = 1
        _Y ("Y Canvas Position", Float) = 0
        _Z ("Z Canvas Position", Float) = 3

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
                o.ro = mul(unity_WorldToObject, float4(_WorldSpaceCameraPos, 1) - float4(_X,0,0,0));
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
                    return sdTorus(p, 0.8, 0.4, 0.1);
                }
                if (_Shape == 5){
                    p = Rotate(p);
                    return sdCapsuleX(p, 2.5, 0.6);
                }
                if (_Shape == 6){
                    p = Rotate(p);
                    return sdPentachoron(p, 0.7);
                }
                p = Rotate(p);
                return sdSphere(p, 1);
            }

            float GetDist(float4 p){
                p = p-float4(_X,_Y,_Z,_W);
                
                float shape = Shape(p);
                float floor = p.y+1;
                
                // BUILD SCENE
                float d = shape;
                      d = min(d, floor);
                
                return d;
            }

            int GetMat(float4 p){
                p = p-float4(_X,_Y,_Z,_W);
                
                float shape = Shape(p);
                float floor = p.y+1;
                
                // BUILD SCENE
                float d = shape;
                      d = min(d, floor);
                
                // ASSIGN MATERIAL
                int mat = 0;
                if (d == shape)    mat = 1;
                if (d == floor)    mat = 2;
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
                float4 lightPos = float4(-2,2,3,0);

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
                    col.rgb = 0.8;
                else {
                    float4 p = ro + rd * d;

                    int mat = GetMat(p);
                    if (mat == 1) col.rgb = (GetLight(p)/4)+0.6*float3(0.8,0.1,0.1);
                    if (mat == 2) col.rgb = ((GetLight(p)/2)+0.5);
                }
                
                return col;
            }
            ENDCG
        }
    }
}
