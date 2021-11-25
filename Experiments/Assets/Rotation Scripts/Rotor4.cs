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
                           float bxw, float byw, float bzw, float bxyzw)
    {
        this.a = a;
        this.bxy = bxy;
        this.bxz = bxz;
        this.byz = byz;
        this.bxw = bxw;
        this.byw = byw;
        this.bzw = bzw;
        this.bxyzw = bxyzw;
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
        this.bxyzw = 0;
    }

    // Rotor4-Rotor4 Product
    public static Rotor4 operator *(Rotor4 p, Rotor4 q)
    {
        float a0    = p.a;
        float a1e01 = p.bxy;
        float a2e20 = p.bxz;
        float a4e12 = p.byz;
        float a3e30 = p.bxw;
        float a5e31 = p.byw;
        float a6e32 = p.bzw;
        float a7e0123 = p.bxyzw;

        float b0    = q.a;
        float b1e01 = q.bxy;
        float b2e20 = q.bxz;
        float b4e12 = q.byz;
        float b3e30 = q.bxw;
        float b5e31 = q.byw;
        float b6e32 = q.bzw;
        float b7e0123 = q.bxyzw;
 
        float e     = a0 * b0      - a1e01 * b1e01    - a2e20 * b2e20   - a4e12 * b4e12   - a3e30 * b3e30   - a5e31 * b5e31   - a6e32 * b6e32	  + a7e0123 * b7e0123;
		float e01   = a0 * b1e01   + a1e01 * b0       + a2e20 * b4e12   - a4e12 * b2e20   + a3e30 * b5e31   - a5e31 * b3e30   - a6e32 * b7e0123   - a7e0123 * b6e32;
		float e20   = a0 * b2e20   - a1e01 * b4e12    + a2e20 * b0      + a4e12 * b1e01   + a3e30 * b6e32   + a5e31 * b7e0123 - a6e32 * b3e30	  + a7e0123 * b5e31;
		float e12   = a0 * b4e12   + a1e01 * b2e20    - a2e20 * b1e01   + a4e12 * b0      - a3e30 * b7e0123 + a5e31 * b6e32   - a6e32 * b5e31	  - a7e0123 * b3e30;
		float e30   = a0 * b3e30   + a1e01 * b5e31    + a2e20 * b6e32   + a4e12 * b7e0123 + a3e30 * b0      - a5e31 * b1e01   - a6e32 * b2e20	  + a7e0123 * b4e12;
		float e31   = a0 * b5e31   + a1e01 * b3e30    + a2e20 * b7e0123 - a4e12 * b6e32   - a3e30 * b1e01   + a5e31 * b0      + a6e32 * b4e12	  + a7e0123 * b2e20;
		float e32   = a0 * b6e32   + a1e01 * b7e0123  - a2e20 * b3e30   - a4e12 * b5e31   + a3e30 * b2e20   + a5e31 * b4e12   + a6e32 * b0        + a7e0123 * b1e01;
		float e0123 = a0 * b7e0123 + a1e01 * b6e32    - a2e20 * b5e31   + a4e12 * b3e30   + a3e30 * b4e12   - a5e31 * b2e20   + a6e32 * b1e01     + a7e0123 * b0;

        return new Rotor4(e, e01, e20, e12, e30, e31, e32, e0123);
    }

    // Rotate a Vector with a Rotor
    public Vector4 Rotate(Vector4 u)
    {
        Rotor4 p = this;

        float ae1 = u.x;
        float ae2 = u.y;
        float ae3 = u.z;
        float ae4 = u.w;

        float be = p.a;
        float be12 = p.bxy;
        float be31 = p.bxz;
        float be23 = p.byz;
        float be41 = -p.bxw;
        float be42 = -p.byw;
        float be43 = -p.bzw;
        float be1234 = p.bxyzw;

        // q = Px
        Vector4 q;
        q.x = ae1 * be - ae2 * be12 + ae3 * be31;
        q.y = ae2 * be + ae1 * be12 - ae3 * be23 + ae4 * be42;
        q.z = ae3 * be - ae1 * be31 + ae2 * be23 - ae4 * be43;
        q.w = ae4 * be + ae1 * be41 - ae4 * be41 + ae3 * be43;

        float q123 =  ae1 * be23   + ae2 * be31   + ae3 * be12   - ae4 * be1234;
        float q134 = -ae1 * be43   + ae2 * be1234 + ae3 * be41   + ae4 * be31;
        float q142 =  ae1 * be42   + ae2 * be41   - ae3 * be1234 - ae4 * be12;
        float q324 =  ae1 * be1234 + ae2 * be43   + ae3 * be42   + ae4 * be23;

        p = p.Reverse();
        float ae = p.a;
        float ae12 = p.bxy;
        float ae31 = p.bxz;
        float ae23 = p.byz;
        float ae41 = -p.bxw;
        float ae42 = -p.byw;
        float ae43 = -p.bzw;
        float ae1234 = p.bxyzw;

        float be1 = q.x;
        float be2 = q.y;
        float be3 = q.z;
        float be4 = q.w;
        float be123 = q123;
        float be134 = q134;
        float be142 = q142;
        float be324 = q324;

        // r = qP*
        Vector4 r;
        r.x = ae * be1 + ae12 * be2   - ae31 * be3   + ae23 * be123 + ae41 * be4   + ae42 * be134 + ae43 * be142 - ae1234 * be324;
        r.y = ae * be2 - ae12 * be1   + ae31 * be123 + ae23 * be3   + ae41 * be134 - ae42 * be4   - ae43 * be324 - ae1234 * be142;
        r.z = ae * be3 + ae12 * be123 + ae31 * be1   - ae23 * be2   - ae41 * be142 - ae42 * be324 + ae43 * be4   - ae1234 * be134;
        r.w = ae * be4 - ae12 * be142 - ae31 * be134 - ae23 * be324 - ae41 * be1   + ae42 * be2   - ae43 * be3   - ae1234 * be123;

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
        return new Rotor4(a, -bxy, -bxz, -byz, -bxw, -byw, -bzw, -bxyzw);
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
                                             bv.bxw, bv.byw, bv.bzw, 0);
    }
}
