using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bivector4
{
    public float bxy = 0;
    public float bxz = 0;
    public float byz = 0;
    public float bxw = 0;
    public float byw = 0;
    public float bzw = 0;

    public Bivector4(float bxy, float bxz, float byz,
                  float bxw, float byw, float bzw)
    {
        this.bxy = bxy;
        this.bxz = bxz;
        this.byz = byz;
        this.bxw = bxw;
        this.byw = byw;
        this.bzw = bzw;
    }

    public static Bivector4 Wedge(Vector4 u, Vector4 v)
    {
        Bivector4 b = new Bivector4(
                u.x * v.y - u.y * v.x, //XY
                u.x * v.z - u.z * v.x, //XZ
                u.y * v.z - u.z * v.y, //YZ
                u.x * v.w - u.w * v.x, //XW
                u.y * v.w - u.w * v.y, //YW
                u.z * v.w - u.w * v.z  //ZW
            );
        return b;
    }
}
