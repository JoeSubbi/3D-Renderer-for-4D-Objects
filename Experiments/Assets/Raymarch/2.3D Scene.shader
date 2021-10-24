Shader "Unlit/Scene"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Effect ("ShadingType", Int) = 2
        _Y ("Y Axis Cross Section", Range(-0.1,2)) = 0
        _CS ("Cross Section", Int) = 1
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

            // To make the Unity shader SRP Batcher compatible, declare all
            // properties related to a Material in a a single CBUFFER block with 
            // the name UnityPerMaterial.
            CBUFFER_START(UnityPerMaterial)
                // The following line declares the _BaseColor variable, so that you
                // can use it in the fragment shader.
                int _Effect;  
                float _Y;   
                int _CS;       
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
             * \brief   Shape distance function for an infinite plane
             *
             * \param   p   center point of object
             */
            float sdPlaneInf(float3 p){
                float d = p.y;
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
             * \brief   Shape distance function for a box
             *
             * \param   p   center point of object
             * \param   s   float 3 of box shape
             *              x, y, z -> width, height, depth
             * \param   r   radius of the bevel
             */
            float sdRoundedBox(float3 p, float3 s, float r){
                float d = length(max(abs(p)-s, 0)) - r;
                return d;
            }

            /**
             * \brief   Shape distance function for a capsule
             *
             * \param   p   center point of object
             * \param   a   origin of first sphere cap
             * \param   b   origin of second sphere cap
             * \param   r   radius of capsule
             */
            float sdCapsule(float3 p, float3 a, float3 b, float r){
                float3 ab = b-a;
                float3 ap = p-a;

                float t = dot(ab, ap) / dot(ab, ab);
                t = clamp(t, 0., 1.);

                float3 c = a + t*ab;
                float d = length(p-c) - r;

                return d;
            }

            /**
             * \brief   Shape distance function for a cylinder
             *
             * \param   p   center point of object
             * \param   a   origin of first circle cap
             * \param   b   origin of second circle cap
             * \param   r   radius of cylinder
             */
            float sdCylinder(float3 p, float3 a, float3 b, float r){
                float3 ab = b-a;
                float3 ap = p-a;

                float t = dot(ab, ap) / dot(ab, ab);

                float3 c = a + t*ab;

                float x = length(p-c) - r;
                float y = (abs(t-0.5)-0.5) * length(ab);
                float e = length(max(float2(x, y), 0));
                float i = min(max(x,y), 0);

                return e+i;
            }

            float sdOctahedron( float3 p, float s){
                float c = sqrt(4); //Square root of number of dimensions
                return ((dot(abs(p), float4(1,1,1,1)) - s) / c);
            }

            float sdTetrahedron(float3 p, float s){
                return (max(
                            abs(p.x+p.y)-p.z,
                            abs(p.x-p.y)+p.z
                            )-1*s
                        )/sqrt(3.) ;
            }

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

            float dot2( float3 v ) { return dot(v,v); }
            float udTriangle(float p, float a)
            {

                //float3 v1 = p-float3((-a/2),(-a/(2*sqrt(3))),(-a/(2*sqrt(6))) );
                //float3 v2 = p-float3(( a/2),(-a/(2*sqrt(3))),(-a/(2*sqrt(6))) );
                //float3 v3 = p-float3(   0  ,(-a/(  sqrt(3))),(-a/(2*sqrt(6))) );

                float3 v1 = p-float3(-0.1, 0  , 0.1);
                float3 v2 = p-float3( 0.1, 0  , 0.1);
                float3 v3 = p-float3( 0  , 0.1, 0.1);

                float3 v21 = v2 - v1; float3 p1 = p - v1;
                float3 v32 = v3 - v2; float3 p2 = p - v2;
                float3 v13 = v1 - v3; float3 p3 = p - v3;
                float3 nor = cross( v21, v13 );

                return sqrt( (sign(dot(cross(v21,nor),p1)) + 
                            sign(dot(cross(v32,nor),p2)) + 
                            sign(dot(cross(v13,nor),p3))<2.0) 
                            ?
                            min( min( 
                            dot2(v21*clamp(dot(v21,p1)/dot2(v21),0.0,1.0)-p1), 
                            dot2(v32*clamp(dot(v32,p2)/dot2(v32),0.0,1.0)-p2) ), 
                            dot2(v13*clamp(dot(v13,p3)/dot2(v13),0.0,1.0)-p3) )
                            :
                            dot(nor,p1)*dot(nor,p1)/dot2(nor) );
            }

            float4 Rotate(float a) {
                float s = sin(a);
                float c = cos(a);
                return float4(c, s, -s ,c);
            }

            /**
             * \brief   Rotation Matrix Multiplication for 3D rotation
             *
             * \param   mat rotation matrix
             * \param   a   axis of rotation
             * \param   b   axis of rotation
             */
            float2 Rot3MatMul(float4 mat, float a, float b){
                float2 rot = float2( a * mat[0] + b * mat[1],
                                     a * mat[2] + b * mat[3] );
                return rot;
            }

            float GetDist(float3 p){
                //Define shapes
                float plane = sdBox( p-float3(0, -0.1, 0), float3(1.5,0.1,1.5));

                float sphere = sdSphere(    p-float3(0.9, 0.4, 0.5), 
                                            0.4);
                float torus  = sdTorus(     p-float3(-0.2, 0.15, -0.5), 
                                            0.4, 0.15);                
                
                //translate box
                float3 bp = p-float3(1, 0.8, -1);
                //Shadow
                //bp *= float3(1,0,1);
                //rotation matrix for continuous rotation
                float4 mat = Rotate(_Time*10);

                bp.xz = Rot3MatMul(mat, bp.x, bp.z);
                bp.yz = Rot3MatMul(mat, bp.y, bp.z);
                bp.xy = Rot3MatMul(mat, bp.x, bp.y);

                
                float box     = sdBox(      bp,
                                            float3(0.5,0.5,0.5))-0.01;
                float capsule = sdCapsule(  p, 
                                            float3(-0.6, 0.1, 0.4), 
                                            float3(-1.1, 0.1, -0.1), 
                                            0.1);
                float cylinder = sdCylinder(p, 
                                            float3(0, 0, 0.7), 
                                            float3(0, 0.3, 0.7), 
                                            0.3);
                float octahedron = sdOctahedron(p-float3(-0.9,0.7,0.9), 
                                            0.5);
                float tetrahedron = sdTetrahedron(p-float3(-0.9,0.7,-0.9),
                                            0.2);
                float cone =        sdCone(p-float3(-0.95,0,-0.95),
                                            0.3, 0.6);
                float tetrahedron2 = udTriangle(p-float3(-0.9, 0.7, 0.9), 1);

                // Union all shapes
                float d = min(sphere, torus);
                d = min(d, box);
                d = min(d, capsule);
                d = min(d, cylinder);
                d = min(d, plane);
                //d = min(d, octahedron);
                //d = min(d, tetrahedron);
                d = min(d, cone);
                //d = min(d, tetrahedron2);

                //cross section
                float slice = sdBox( p-float3(0, _Y, 0), float3(1.5,0.001,1.5));
                if (_CS == 0)
                    d = max(slice, d);
                else
                    d = min(slice, d);

                return d;
            }

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

            float3 GetNormal(float3 p){
                float2 e = float2(0.01, 0);

                float3 n = GetDist(p) - float3( 
                    GetDist(p-e.xyy),
                    GetDist(p-e.yxy),
                    GetDist(p-e.yyx)
                );
                return normalize(n);
            }

            float GetLight(float3 p){
                float3 lightPos = float3(0,2,0);

                //angle dependant fall off
                float3 lv = normalize(lightPos-p);
                float3 n  = GetNormal(p);
                float  light  = clamp(dot(n,lv), 0., 1.);

                //shadow
                float3 so = p + n * SURF_DIST * 2.; //shadow origin
                float3 sd = normalize(lightPos-so); //light direction
                float d = Raymarch(so, sd);
                if( d<length(lightPos-p) ) light *= 0.1;

                return light;
            }

            // Fragment Shader
            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv; // UV coordinates - centered on object
                float3 ro = i.ro;                     // Ray Origin - Camera
                float3 rd = normalize(i.hitPos - ro); // Ray Direction

                float d = Raymarch(ro, rd); // Distance

                // Shading
                fixed4 col = 0;

                // Colour in the cube based on ray march
                if (d > MAX_DIST)
                    discard;
                else {
                    float3 p = ro + rd * d;
                    
                    float3 n = GetNormal(p);             // Normal
                    float3 l = GetLight(p);              // Light
                    float3 c = GetNormal(p)*GetLight(p); //Lit Normal

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
