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

/**
 * \brief   Shape distance function for a cone with
 *          the tip extending into the y axis
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

/**
 * \brief   Shape distance function for a cone with 
 *          the tip extending into the w axis
 *
 * \param   r   radius at base of cone
 * \param   h   height of cone
 */
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