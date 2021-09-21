Shader "Unlit/Raymarch"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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

            float DefineShape(){
                
            }

            //Get Distance
            /**
             * param p  distance from point
             *
             * return distance to object
             */
            float GetDist( float3 p ){
                //Distance between origin and surface is distance between
                //origin and center of object minux distance to the surface
                // e.g 0.5 for a sphere of radius 0.5
                float SCALE = 0.4;
                //float d = length(p) - SCALE; //sphere
                float ringRadius = SCALE/3;
                //float d = length( float2(length(p.xy) - SCALE, p.z)) - ringRadius;
                float d = length( float2(length(p.zx) - SCALE, p.y)) - ringRadius;


                return d;
            }

            //Raymarcher
            /** 
             * param ro  RayOrigin
             * param rd  RayDistance
             * 
             * return distance from Origin
             */
            float Raymarch( float3 ro, float3 rd ){
                float dO = 0; //Distance from origin
                float dS;     //Distance from surface
                for( int i=0; i < MAX_STEPS; i++){
                    float3 p = ro + dO * rd;
                    dS = GetDist(p);
                    dO += dS;
                    if (dS < SURF_DIST || dO > MAX_DIST) break;
                }
                return dO;
            }

            //Get Normal
            /**
             * param p  point of surface hit
             *
             */
            float3 GetNormal(float3 p){
                float2 e = float2(0.01, 0);
                //normal
                float3 n = GetDist(p) - float3( 
                        GetDist(p-e.xyy),
                        GetDist(p-e.yxy),
                        GetDist(p-e.yyx)
                    );
                return normalize(n);
            }

            fixed4 frag (v2f i) : SV_Target
            {   
                float2 uv = i.uv - 0.5;

                //Get UV coordinates and transform them to the center of the object
                //(Camera)
                //float3 ro = float3(0, 0, -3); //Ray Origin
                //float3 rd = normalize(float3(uv.x, uv.y, 1)); //Ray Direction

                float3 ro = i.ro;
                float3 rd = normalize(i.hitPos - ro);

                float d = Raymarch(ro, rd);

                // sample the texture
                fixed4 col = 0;
                fixed4 tex = tex2D(_MainTex, i.uv);
                float m = dot(uv, uv);

                if (d < MAX_DIST){
                    float3 p = ro + rd * d;
                    float3 n = GetNormal(p);
                    col.rgb = n;
                } 
                else discard;

                //col = lerp(col, tex, smoothstep(0.23, 0.23, m));

                return col;
            }
            ENDCG
        }
    }
}
