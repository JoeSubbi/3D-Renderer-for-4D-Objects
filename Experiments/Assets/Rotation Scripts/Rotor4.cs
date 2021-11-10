using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotor4
{
    // scalar part
    public float a = 1;

    // bivector part
    public float bxy = 0;
    public float bxz = 0;
    public float byz = 0;
    public float bxw = 0;
    public float byw = 0;
    public float bzw = 0;

    // quadvector part
    public float bxyzw = 0;

    // CONSTRUCTORS
    public Rotor4(float a, float bxy, float bxz, float byz,
                           float bxw, float byw, float bzw)
    {
        this.a = a;
        this.bxy = bxy;
        this.bxz = bxz;
        this.byz = byz;
        this.bxw = bxw;
        this.byw = byw;
        this.bzw = bzw;
    }

    public Rotor4(Bivector4 bv, float angle)
    {
        float sina = Mathf.Sin(angle / 2);
        a = Mathf.Cos(angle / 2);
        this.bxy = -sina * bv.bxy;
        this.bxz = -sina * bv.bxz;
        this.byz = -sina * bv.byz;
        this.bxw = -sina * bv.bxw;
        this.byw = -sina * bv.byw;
        this.bzw = -sina * bv.bzw;
    }

    // Rotor3-Rotor3 Product
    public static Rotor4 operator *(Rotor4 p, Rotor4 q)
    {
        float a = p.a * q.a - p.bxy * q.bxy - p.bxz * q.bxz - p.byz * q.byz - 
                              p.bxw * p.bxw - p.byw * p.byw - p.bzw * p.bzw;

        float bxy = p.bxy * q.a + p.a * q.bxy +
                    p.byz * q.bxz - p.bxz * q.byz;// - 
                    //p.byw * q.bzw + p.bzw * q.byw;

        float bxz = p.bxz * q.a + p.a * q.bxz -
                    p.byz * q.bxy + p.bxy * q.byz;// -
                    //p.bxw * q.bzw + p.bzw * p.bxw;

        float byz = p.byz * q.a + p.a * q.byz + 
                    p.bxz * q.bxy - p.bxy * q.bxz;// -
                    //p.bxw * q.byw + q.byw * q.bxw;

        float bxw = p.bxw * q.a + p.a * q.bxw -
                    p.bxy * q.byw + p.byw * q.bxy;// -
                    //p.bxz * q.bzw + p.bzw * q.bxz;

        float byw = p.byw * q.a + p.a * q.byw +
                    p.bxy * q.bxw - p.bxw * q.bxy;// -
                    //p.bxz * q.bzw + p.bzw * q.bxz;

        float bzw = p.bzw * q.a + p.a * q.bzw +
                    p.bxz * q.bxw - p.bxw * q.bxz;// +
                    //p.byz * q.byw - p.byw * q.byz;

        p = new Rotor4(a, bxy, bxz, byz, bxw, byw, bzw);
        return p;
    }

    // Rotate a Vector with a Rotor
    public Vector4 Rotate(Vector4 u)
    {
        Rotor4 p = this;

        // q = Px
        Vector4 q;
        q.x = p.a * u.x + u.y * p.bxy + u.z * p.bxz + u.w * p.bxw;
        q.y = p.a * u.y;
                        // - u.x * p.bxy + u.z * p.byz + u.w * p.byw;
        q.z = p.a * u.z;
                        // - u.x * p.bxz - u.y * p.byz + u.w * p.bzw;
        q.w = p.a * u.w;
                        // - u.x * p.bxw - u.y * p.bzw + u.w * p.byw;

        //float qxyz = u.x * p.byz - u.y * p.bxz + u.z * p.bxy;
        //bxyzw = u.x * p.bxw - u.y * p.byw + u.z * p.bzw + u.w * qxyz;

        // r = qP*
        Vector4 r;
        r.x = p.a * u.x;
                        // + q.y  * p.bxy + q.z  * p.bxz + qxyz * p.byz + q.w * p.bxw;
        r.y = p.a * u.y;
                        // - q.x  * p.bxy - qxyz * p.bxz + q.z  * p.byz + q.w * p.byw;
        r.z = p.a * u.z;
                        // + qxyz * p.bxy - q.x  * p.bxz - q.y  * p.byz + q.w * p.bzw;
        r.w = p.a * u.w;
                        // + bxyzw * p.bxw + bxyzw * p.byw + bxyzw * p.bzw + bxyzw * qxyz;

        return r;
    }

    // Rotate one Rotor to another
    public Rotor4 Rotate(Rotor4 r)
    {
        return this * r * this.Reverse();
    }

    // Conjugate
    public Rotor4 Reverse()
    {
        return new Rotor4(a, -bxy, -bxz, -byz, -bxw, -byw, -bzw);
    }

    private float sq(float x)
    {
        return x * x;
    }

    // Length Squared
    private float LengthSquared()
    {
        return sq(a) + sq(bxy) + sq(bxz) + sq(byz) + 
                       sq(bxw) + sq(byw) + sq(bzw);
    }

    // Length
    private float Length()
    {
        return Mathf.Sqrt(LengthSquared());
    }

    // Normalise this rotor
    public void Normalise()
    {
        float l = Length();
        a /= l;
        bxy /= l;
        bxz /= l;
        byz /= l;
        bxw /= l;
        byw /= l;
        bzw /= l;
    }

    // Normalised copy of a rotor
    public Rotor4 Normal()
    {
        Rotor4 r = this;
        r.Normalise();
        return r;
    }

    // Geometric Product
    public Rotor4 GeoProd(Vector4 a, Vector4 b)
    {
        Bivector4 bv = Bivector4.Wedge(a, b);
        return new Rotor4(Vector4.Dot(a, b), bv.bxy, bv.bxz, bv.byz,
                                             bv.bxw, bv.byw, bv.bzw);
    }
}
