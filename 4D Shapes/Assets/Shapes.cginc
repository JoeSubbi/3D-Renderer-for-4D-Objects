/*
 * 4D SHAPES
 */

/**
 * \brief   Signed Distance Function for a sphere
 *
 * \param   p   center point of object
 * \param   r   radius of the sphere
 */
float sdSphere(float4 p, float r)
{
    float d = length(p) - r;
    return d;
}

/**
 * \brief   Signed Distance Function for a box
 *
 * \param   p   center point of object
 * \param   s   float 3 of box shape
 *              x, y, z -> width, height, depth
 */
float sdBox(float4 p, float4 s)
{
    float d = length(max(abs(p) - s, 0));
    return d;
}

/**
 * \brief   Signed Distance Function for a torus
 *
 * \param   p   center point of object
 * \param   r1  major radius - torus ring
 * \param   r2  minor radius - torus thickness
 * \param   r3  minor radius in the 4th dimension
 */
float sdTorus(float4 p, float r1, float r2, float r3)
{
    float d = length(
                float2(
                    length( 
                    float2(length(p.zx) - r1, p.y)
                            ) - r2, p.w
                    )) - r3;
    return d;
}

/**
 * \brief   Signed Distance Function for a cone with
 *          the tip extending into the y axis
 *
 * \param   r   radius at base of cone
 * \param   h   height of cone
 */
float sdConeW(float4 p, float r, float h)
{
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

/**
 * \brief   Signed Distance Function for a cone with 
 *          the tip extending into the w axis
 *
 * \param   r   radius at base of cone
 * \param   h   height of cone
 */
float sdConeY(float4 p, float r, float h)
{
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

/**
 * \brief   Signed Distance Function for a capsule
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
 * \brief   Signed Distance Function for a capsule
 *          elongated along the x axis
 *
 * \param   p       center point of object
 * \param   length  length of the capsule
 * \param   r       radius of capsule
 */
float sdCapsuleX(float4 p, float length, float r)
{
    float l = length/2;

    return sdCapsule(p, 
                     float4(-l, 0, 0, 0),
                     float4( l, 0, 0, 0),
                     r
                    );
}

/**
 * \brief   Signed Distance Function for a capsule
 *          elongated along the w axis
 *
 * \param   p       center point of object
 * \param   length  length of the capsule
 * \param   r       radius of capsule
 */
float sdCapsuleW(float4 p, float length, float r)
{
    float l = length/2;

    return sdCapsule(p, 
                     float4(0, 0, 0,-l),
                     float4(0, 0, 0, l),
                     r
                    );
}

/**
 * \brief   Signed Distance Function for a pentachoron
 *          Also known as a 5-cell or hyper tetrahedron
 *
 * \param   p   center point of object
 * \param   s   scale of object (radius)
 */
float sdPentachoron(float4 p, float s){
    float a = +p.x +p.y -p.z -p.w;
    float b = -p.x -p.y -p.z -p.w;
    float c = +p.x -p.y +p.z -p.w;
    float d = -p.x +p.y +p.z -p.w;
    float e = p.w;
    return (max(max(max(a,b),max(c,d)), e)-s)/sqrt(5.);
}

/*
 * 3D SHAPES
 */

 /**
* \brief   Signed Distance Function for a sphere
*
* \param   p   center point of object
* \param   r   radius of sphere
*/
float sdSphere(float3 p, float r){
    float d = length(p) - r;
    return d;
}

/**
* \brief   Signed Distance Function for a box
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
* \brief   Signed Distance Function for a torus
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
 * \brief Signed Distance Function for a cone extending along Z
 * 
 * \param   p        center point of object
 * \param   radius   radius of the base of the cone
 * \param   height   height of the cone
 */
float sdConeZ(float3 p, float radius, float height) {
    float2 q = float2(length(p.xy), p.z);
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

/**
 * \brief Signed Distance Function for a cone extending along Y
 * 
 * \param   p        center point of object
 * \param   radius   radius of the base of the cone
 * \param   height   height of the cone
 */
float sdConeY(float3 p, float radius, float height) {
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

/**
 * \brief   Signed Distance Function for a capsule
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
 * \brief   Signed Distance Function for a capsule
 *          elongated along the x axis
 * 
 * \param   p       center point of object
 * \param   length  length of the capsule
 * \param   r       radius of capsule
 */
float sdCapsuleX(float3 p, float length, float r)
{
    float l = length/2;

    return sdCapsule(p, 
                    float3(-l, 0, 0),
                    float3( l, 0, 0),
                    r
                    );
}

/**
 * \brief   Signed Distance Function for a capsule
 *          elongated along the y axis
 *
 * \param   p       center point of object
 * \param   length  length of the capsule
 * \param   r       radius of capsule
 */
float sdCapsuleZ(float3 p, float length, float r)
{
    float l = length/2;

    return sdCapsule(p, 
                    float3(0, 0,-l),
                    float3(0, 0, l),
                    r
                    );
}

/**
 * \brief   Signed distance function for an octohedron
 *
 * \param   p   center point of object
 * \param   s   size of the object
*/
float sdOctahedron( float3 p, float s){
    float c = sqrt(3);
    return ((dot(abs(p), float3(1,1,1)) - s) / c);
}

/**
 * \brief   Signed Distance Function for a Tetrahedron
 *
 * \param   p   center point of object
 * \param   s   scale of object (radius)
 */
float sdTetrahedron(float3 p, float s){
    float a = +p.x +p.y -p.z;
    float b = -p.x -p.y -p.z;
    float c = +p.x -p.y +p.z;
    float d = -p.x +p.y +p.z;
    return (max(max(a,b),max(c,d))-s)/sqrt(5.);
}