Shader "Hidden/NewImageEffectShader"
{
    Properties
    {
        _Z ("Z Axis Cross Section", Range(-2,2)) = 0
        _X ("X Canvas Position", Float) = -5.8
        _Y ("Y Canvas Position", Float) = -2.2

        _Shape ("Shape", Int) = 0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            #define MAX_STEPS 1000
            #define MAX_DIST  1000
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

            // To make the Unity shader SRP Batcher compatible, declare all
            // properties related to a Material in a a single CBUFFER block with 
            // the name UnityPerMaterial.
            CBUFFER_START(UnityPerMaterial)
                // The following line declares the _BaseColor variable, so that you
                // can use it in the fragment shader.
                float _Z;
                float _X;
                float _Y;

                int _Shape;
            CBUFFER_END

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                // object space
                o.ro = mul(unity_WorldToObject, float4(_WorldSpaceCameraPos, 1));
                o.hitPos = v.vertex;
                return o;
            }

            sampler2D _MainTex;

            /**
             * \brief   Shape distance function for an 2 sided infinite plane 
             *          to create a narrow cross section
             *
             * \param   p   center point of object
             */
            float crossSection(float3 p){
                float d = max(p.z+0.01, -p.z-0.01);
                return d;
            }

            /**
             * \brief   Shape distance function for a sphere
             *
             * \param   p   center point of object
             * \param   r   radius of sphere
             */
            float sdSphere(float3 p, float r){
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
            float sdBox(float3 p, float3 s){
                float d = length(max(abs(p)-s, 0));
                return d;
            }

            /**
             * \brief   Shape distance function for a torus
             *
             * \param   p   center point of object
             * \param   r1  major radius - torus ring
             * \param   r2  minor radius - torus thickness
             */
            float sdTorus(float3 p, float r1, float r2){
                float d = length( float2(length(p.zx) - r1, p.y)) - r2;
                return d;
            }

            /**
             * \brief Shape distance function for a cone
             * 
             * \param   p        center point of object
             * \param   radius   radius of the base of the cone
             * \param   height   height of the cone
             */
            float sdCone(float3 p, float radius, float height) {
                float2 q = float2(length(p.xz), p.y);
                float2 tip = q - float2(0, height);
                float2 mantleDir = normalize(float2(height, radius));
                float mantle = dot(tip, mantleDir);
                float d = max(mantle, -q.y);
                float projected = dot(tip, float2(mantleDir.y, -mantleDir.x));
                
                // distance to tip
                if ((q.y > height) && (projected < 0)) {
                    d = max(d, length(tip));
                }
                
                // distance to base ring
                if ((q.x > radius) && (projected > length(float2(height, radius)))) {
                    d = max(d, length(q - float2(radius, 0)));
                }
                return d;
            }

            // Pick a shape based on the integer property
            float Shape(float3 p){   
                if (_Shape == 1){
                    return sdBox(p-float3(0,0,_Z), float3(1,1,1));
                }
                if (_Shape == 2){
                    return sdTorus(p-float3(0,-0.5,_Z), 1, 0.5);
                }
                if (_Shape == 3){
                    return sdCone(p-float3(0,-1,_Z), 1, 2);
                }
                return sdSphere(p-float3(0,0,_Z), 1);
            }

            // Distance to point p from the camera
            float GetDist(float3 p){
                p = p-float3(_X,_Y,0);
                
                // 3D COMPONENT
                float polyhedron = Shape(p);
                // Cross Section
                float cs = crossSection(p);
                // 2D Component
                float polygon = max(polyhedron, cs);

                // ENVIRONMENT
                float floor = max(cs, p.y+1);
                
                // BUILD SCENE
                float d = min(floor, polygon);
                
                return d;
            }

            // Assign materials based on the distance
            int GetMat(float3 p){
                p = p-float3(_X,_Y,0);
                
                // 3D COMPONENT
                float polyhedron = Shape(p);
                // Cross Section
                float cs = crossSection(p);
                // 2D Component
                float polygon = max(polyhedron, cs);

                // ENVIRONMENT
                float floor = max(cs, p.y+1);
                
                // BUILD SCENE
                float d = min(floor, polygon);
                
                // ASSIGN MATERIAL
                int mat = 0;
                if (d == polygon)    mat = 1;
                if (d == floor)     mat = 2;
                return mat;
            }

            /**
             * \brief   Raymarch function
             *
             * \param   ro  Ray origin - camera
             * \param   rd  Ray direction
             */
            float Raymarch(float3 ro, float3 rd){
                float dO = 0; // Distance from Origin
                float dS;     // Distance from Surface

                for( int i=0; i<MAX_STEPS; i++ ){
                    float3 p = ro + dO * rd;
                    dS = GetDist(p);
                    dO += dS;
                    if (dS < SURF_DIST || dO > MAX_DIST) break;
                }
                return dO;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;                     // UV coordinates - centered on object
                float3 ro = i.ro;                     // Ray Origin - Camera
                float3 rd = normalize(i.hitPos - ro); // Ray Direction

                float d = Raymarch(ro, rd);           // Distance to a surface

                // Shading
                fixed4 col = 0;
                // Use the distance to colour the shader. 
                // If the ray didn't hit anything discard the pixel.
                if (d > MAX_DIST)
                    col = 0.8;
                else {
                    float3 p = ro + rd * d;
                    int mat = GetMat(p);
                    if (mat == 1) col.rgb = float3(0.8,0.1,0.1);
                    if (mat == 2) col.rgb = float3(0.5,0.5,0.5);
                }
                return col;
            }
            ENDCG
        }
    }
}
