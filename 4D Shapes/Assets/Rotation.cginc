/*
 * \brief   Rotate a 4D vector using 
 *          the components of a 4D rotor
 *
 * \param   a       4D vector to rotate
 * \param   s       4D rotor scalar component
 * \param   b..     4D rotor bivector components
 * \param   bxyzw   4D rotor pseudovector component
 */
float4 RotorRotate(float4 a, float s,
                            float bxy,
                            float bxz,
                            float bxw,
                            float byz,
                            float byw,
                            float bzw,
                            float bxyzw){ 

    float4 r;

    r.x = ( 
        + 2 * a.w * bxw * s
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

/*
float4 Rotate(float4 p, XZ, YZ, XY, XW, YW, ZW){
    float a = XZ;
    float b = YZ;
    float c = XY;
    p.xz = mul(p.xz, float2x2(cos(a), sin(a), -sin(a), cos(a)));
    p.zy = mul(p.zy, float2x2(cos(b), sin(b), -sin(b), cos(b)));
    p.xy = mul(p.xy, float2x2(cos(c), sin(c), -sin(c), cos(c)));
    a = XW;
    b = YW;
    c = ZW;
    p.xw = mul(p.xw, float2x2(cos(a), sin(a), -sin(a), cos(a)));
    p.yw = mul(p.yw, float2x2(cos(b), sin(b), -sin(b), cos(b)));
    p.zw = mul(p.zw, float2x2(cos(c), sin(c), -sin(c), cos(c)));
    return p;
}
*/