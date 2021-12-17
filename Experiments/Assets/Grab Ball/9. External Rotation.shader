Shader "Unlit/Scene"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Effect ("ShadingType", Int) = 2
        _W ("W Axis Cross Section", Range(-3,3)) = 0
        
        _A ("A Scalar", Float) = 1
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

            float4 Rotate(float4 a){ 

                float s = _A;
                float bxy = _XY;
                float bxz = _XZ;
                float bxw = _XW;
                float byz = _YZ;
                float byw = _YW;
                float bzw = _ZW;
                float bxyzw = _XYZW;

                /*
                float4 r;

                r.x = (
                    + 2 * a.w * bxw * s
                    + 2 * a.w * bxy * byw
                    + 2 * a.w * bxz * bzw
                    - a.x * bxw * bxw
                    - a.x * bxy * bxy
                    - a.x * bxz * bxz
                    + a.x * byw * byw
                    + a.x * byz * byz
                    + a.x * bzw * bzw
                    + a.x * s * s
                    - 2 * a.y * bxw * byw
                    + 2 * a.y * bxy * s
                    - 2 * a.y * bxz * byz
                    - 2 * a.z * bxz * bzw
                    + 2 * a.z * bxy * byz
                    + 2 * a.z * bxz * s 
                );
                r.y = (
                    - 2 * a.w * bxw * bxy
                    + 2 * a.w * byw * s
                    + 2 * a.w * byz * bzw
                    - 2 * a.x * bxw * byw
                    - 2 * a.x * bxy * s
                    - 2 * a.x * bxz * byz
                    + a.y * bxw * bxw
                    - a.y * bxy * bxy
                    + a.y * bxz * bxz
                    - a.y * byw * byw
                    - a.y * byz * byz
                    + a.y * bzw * bzw
                    + a.y * s * s
                    - 2 * a.z * bxy * bxz
                    - 2 * a.z * byw * bzw
                    + 2 * a.z * byz * s
                );
                r.z = (
                    - 2 * a.w * bxw * bxz
                    - 2 * a.w * byw * byz
                    + 2 * a.w * bzw * s
                    - 2 * a.x * bxw * bzw
                    + 2 * a.x * bxy * byz
                    - 2 * a.x * bxz * s
                    - 2 * a.y * bxy * bxz
                    - 2 * a.y * byw * bzw
                    - 2 * a.y * byz * s
                    + a.z * bxw * bxw
                    + a.z * bxy * bxy
                    - a.z * bxz * bxz
                    + a.z * byw * byw
                    - a.z * byz * byz
                    - a.z * bzw * bzw
                    + a.z * s * s
                );
                r.w = (
                    - a.w * bxw * bxw
                    + a.w * bxy * bxy
                    + a.w * bxz * bxz
                    - a.w * byw * byw
                    + a.w * byz * byz
                    - a.w * bzw * bzw
                    + a.w * s * s
                    - 2 * a.x * bxw * s
                    + 2 * a.x * bxy * byw
                    + 2 * a.x * bxz * bzw
                    - 2 * a.y * bxw * bxy
                    - 2 * a.y * byw * s
                    + 2 * a.y * byz * bzw
                    - 2 * a.z * bxw * bxz
                    - 2 * a.z * byw * byz
                    - 2 * a.z * bzw * s
                );

                return r;
                */
                
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
                float sphere = sdSphere( p-float4(0,0,0,_W), 1);
                
                //Box 3D rotation

                //Translate box
                float4 bp = p-float4(0,0,0,_W);
                //Rotate box according to shader parameters
                bp = Rotate(bp);
                
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
