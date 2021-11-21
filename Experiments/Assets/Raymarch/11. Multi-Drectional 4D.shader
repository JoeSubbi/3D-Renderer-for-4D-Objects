Shader "Unlit/Scene"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Effect ("ShadingType", Int) = 2
        _W ("W Axis Cross Section", Range(-3,3)) = 0
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
                int _Shape;     
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
                                )) - r3;
                return d;
            }

            float opExtrussion( float4 p, float sdf, float h )
            {
                float2 w = float2( sdf, abs(p.w) - h );
                return min(max(w.x,w.y),0.0) + length(max(w,0.0));
            }

            float sdETorus(float4 p, float r1, float r2, float e){
                float d = length( 
                                float2(length(p.zx) - r1, p.y)
                                        ) - r2;

                d = opExtrussion(p, d, e);
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
            float sdConeW(float4 p, float r, float h) {
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

            float sdConeY(float4 p, float r, float h) {
                float2 q = float2(length(p.xzw), p.y);
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

            float dot2( float4 v ) { return dot(v,v); }
            float dot2( float3 v ) { return dot(v,v); }

            /*
            float4 crossProd(float4 u, float4 v){
                float a = acos(dot(u,v) - length(u) * length(v));
                float c = length(u) * length(v) * sin(a);
                return float4();
            }*/

            float udTriangle(float4 p, float4 a, float4 b, float4 c){
                float4 ba = b - a; float4 pa = p - a;
                float4 cb = c - b; float4 pb = p - b;
                float4 ac = a - c; float4 pc = p - c;
                
                float4 nor = float4(cross( ba, ac ), 0);

                return sqrt(
                    (       (dot(cross(ba,nor),pa)) +
                    sign(dot(cross(cb,nor),pb)) +
                    sign(dot(cross(ac,nor),pc))<2.0)
                    ?
                    min( min(
                    dot2(ba*clamp(dot(ba,pa)/dot2(ba),0.0,1.0)-pa),
                    dot2(cb*clamp(dot(cb,pb)/dot2(cb),0.0,1.0)-pb) ),
                    dot2(ac*clamp(dot(ac,pc)/dot2(ac),0.0,1.0)-pc) )
                    :
                    dot(nor,pa)*dot(nor,pa)/dot2(nor) );
            }

            float udTriangle3(float3 p, float3 a, float3 b, float3 c){
                float3 ba = b - a; float3 pa = p - a;
                float3 cb = c - b; float3 pb = p - b;
                float3 ac = a - c; float3 pc = p - c;
                
                float3 nor = cross( ba, ac );

                return sqrt(
                    (       (dot(cross(ba,nor),pa)) +
                    sign(dot(cross(cb,nor),pb)) +
                    sign(dot(cross(ac,nor),pc))<2.0)
                    ?
                    min( min(
                    dot2(ba*clamp(dot(ba,pa)/dot2(ba),0.0,1.0)-pa),
                    dot2(cb*clamp(dot(cb,pb)/dot2(cb),0.0,1.0)-pb) ),
                    dot2(ac*clamp(dot(ac,pc)/dot2(ac),0.0,1.0)-pc) )
                    :
                    dot(nor,pa)*dot(nor,pa)/dot2(nor) );
            }

            float sdTetrahedron2(float4 triangles){
                return min(triangles[0], min(triangles[1], min(triangles[2], triangles[3])));
            }
            
            float Pentachoron(float4 p){
                
                float scale = 0.5;
                float4 verts[5] = {
                                            float4( scale, scale, scale, (-scale/sqrt(5)) ),
                                            float4( scale,-scale,-scale, (-scale/sqrt(5)) ),
                                            float4(-scale, scale,-scale, (-scale/sqrt(5)) ),
                                            float4(-scale,-scale, scale, (-scale/sqrt(5)) ),
                                            float4( 0, 0, 0, ( (4*scale)/sqrt(5)) )
                };
                float triangles[10] = {
                                            udTriangle(p, verts[0], verts[1], verts[4]),
                                            udTriangle(p, verts[1], verts[2], verts[4]),
                                            udTriangle(p, verts[1], verts[3], verts[4]),
                                        
                                            udTriangle(p, verts[0], verts[2], verts[4]),
                                            udTriangle(p, verts[0], verts[3], verts[4]),
                                            udTriangle(p, verts[2], verts[3], verts[4]),
                                            
                                            udTriangle(p, verts[0], verts[1], verts[2]),
                                            udTriangle(p, verts[0], verts[1], verts[3]),
                                            udTriangle(p, verts[0], verts[2], verts[3]),
                                            udTriangle(p, verts[1], verts[2], verts[3])
                };

                float tets[5] = {
                                            sdTetrahedron2( (triangles[3], triangles[4], triangles[5], triangles[8]) ),
                                            sdTetrahedron2( (triangles[0], triangles[2], triangles[4], triangles[7]) ),
                                            sdTetrahedron2( (triangles[0], triangles[1], triangles[3], triangles[6]) ),
                                            sdTetrahedron2( (triangles[1], triangles[2], triangles[5], triangles[9]) ),
                                            sdTetrahedron2( (triangles[6], triangles[7], triangles[8], triangles[9]) )
                };
                
                /*
                float d = min(triangles[0], triangles[1]);
                      d = min(d, triangles[2]);
                      d = min(d, triangles[3]);
                      d = min(d, triangles[4]);
                      d = min(d, triangles[5]);
                      d = min(d, triangles[6]);
                      d = min(d, triangles[7]);
                      d = min(d, triangles[8]);
                      d = min(d, triangles[9]);

                      d = abs(d)-0.01;
                      */
                float d = min(tets[0], tets[1]);
                      d = min(d, tets[2]);
                      d = min(d, tets[3]);
                      d = min(d, tets[4]);
                      d = abs(d)-0.01;

                return d;
            }
            

            float4 Rotate(float a) {
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

            float4 GlobalRotation(float4 p){
                //rotation matrix for continuous rotation
                float4 mat = Rotate(_Time*10);
                
                //3D Rotation
                //p.xy = RotMatMul(mat, p.x, p.y);
                //p.xz = RotMatMul(mat, p.x, p.z);
                //p.yz = RotMatMul(mat, p.y, p.z);
                
                //4D Rotation
                //p.wx = RotMatMul(mat, p.w, p.x);
                //p.wy = RotMatMul(mat, p.w, p.y);
                //p.wz = RotMatMul(mat, p.w, p.z);

                return p;
            }

            float torus(float4 p){
                float4 p1 = p-float4( 1, 1, 0, _W);
                float4 p2 = p-float4(-1, 1, 0, _W);
                float4 p3 = p-float4(-1,-1, 0, _W);
                float4 p4 = p-float4( 1,-1, 0, _W);
                
                float4 mat = Rotate(3.14159/2);
                p2.xw = RotMatMul(mat, p2.x, p2.w);
                p3.yw = RotMatMul(mat, p3.y, p3.w);
                p4.zw = RotMatMul(mat, p4.z, p4.w);

                p1 = GlobalRotation(p1);
                p2 = GlobalRotation(p2);
                p3 = GlobalRotation(p3);
                p4 = GlobalRotation(p4);

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

            float coneW(float4 p){
                float4 p1 = p-float4( 1, 1, 0, _W);
                float4 p2 = p-float4(-1, 1, 0, _W);
                float4 p3 = p-float4(-1,-1, 0, _W);
                float4 p4 = p-float4( 1,-1, 0, _W);
                
                float4 mat = Rotate(3.14159/2);
                p2.xw = RotMatMul(mat, p2.x, p2.w);
                p3.yw = RotMatMul(mat, p3.y, p3.w);
                p4.zw = RotMatMul(mat, p4.z, p4.w);

                p1 = GlobalRotation(p1);
                p2 = GlobalRotation(p2);
                p3 = GlobalRotation(p3);
                p4 = GlobalRotation(p4);

                float r = 0.5;
                float h = 1;
                
                p1 = p1-float4( 0, 0, 0,-0.5);
                p2 = p2-float4( 0, 0, 0,-0.5);
                p3 = p3-float4( 0, 0, 0,-0.5);
                p4 = p4-float4( 0, 0, 0,-0.5);
                
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

            float coneY(float4 p){

                float4 p1 = p-float4( 1, 1, 0, _W);
                float4 p2 = p-float4(-1, 1, 0, _W);
                float4 p3 = p-float4(-1,-1, 0, _W);
                float4 p4 = p-float4( 1,-1, 0, _W);
                
                float4 mat = Rotate(3.14159/2);
                p2.xw = RotMatMul(mat, p2.x, p2.w);
                p3.yw = RotMatMul(mat, p3.y, p3.w);
                p4.zw = RotMatMul(mat, p4.z, p4.w);

                p1 = GlobalRotation(p1);
                p2 = GlobalRotation(p2);
                p3 = GlobalRotation(p3);
                p4 = GlobalRotation(p4);

                float r = 0.5;
                float h = 1;
                
                p1 = p1-float4( 0,-0.5, 0, 0);
                p2 = p2-float4( 0,-0.5, 0, 0);
                p3 = p3-float4( 0,-0.5, 0, 0);
                p4 = p4-float4( 0,-0.5, 0, 0);

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

            float GetDist(float4 p){
                /*
                int s = _Shape % 3.;
                if (s == 0) return torus(p);
                else if (s == 1) return coneW(p);
                else if (s == 2) return coneY(p);
                
                return 0;*/
                return Pentachoron(p-float4(0,0,0,_W));
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
                float4 ro = float4(i.ro.x, i.ro.y, i.ro.z, 0);           // Ray Origin - Camera
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
                    
                    float4 n = GlobalRotation(GetNormal(p));             // Normal
                    float4 l = GetLight(p);              // Light
                    float4 c = GlobalRotation(GetNormal(p))*GetLight(p); //Lit Normal

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
