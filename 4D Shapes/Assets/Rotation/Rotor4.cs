using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Based on a c++ template by Marc ten Bosch
 * https://marctenbosch.com/news/2011/05/4d-rotations-and-the-4d-equivalent-of-quaternions/
 * 
 * Rotor4 was founded on Marc ten Bosch's Rotor3
 * https://marctenbosch.com/quaternions/code.htm
 */

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

    // pseudo-vector part
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

        float a = p.a;
        float axy = p.bxy;
        float axz = p.bxz;
        float axw = p.bxw;
        float ayz = p.byz;
        float ayw = p.byw;
        float azw = p.bzw;
        float axyzw = p.bxyzw;

        float b = q.a;
        float bxy = q.bxy;
        float bxz = q.bxz;
        float bxw = q.bxw;
        float byz = q.byz;
        float byw = q.byw;
        float bzw = q.bzw;
        float bxyzw = q.bxyzw;

        float e     = -axw * bxw   - axy * bxy   - axz * bxz   - ayw * byw   - ayz * byz   - azw * bzw   + axyzw * bxyzw + a * b;
        float exy   = -axw * byw   + axy * b     - axz * byz   + ayw * bxw   + ayz * bxz   - azw * bxyzw - bxyzw * bzw   + a * bxy;
        float exz   = -axw * bzw   + axy * byz   + axz * b     + ayw * bxyzw - ayz * bxy   + azw * bxw   + axyzw * byw   + a * bxz;
        float exw   =  axw * b     + axy * byw   + axz * bzw   - ayw * bxy   - ayz * bxyzw - azw * bxz   - axyzw * byz   + a * bxw;
        float eyz   = -axw * bxyzw - axy * bxz   + axz * bxy   - ayw * bzw   + ayz * b     + azw * byw   - axyzw * bxw   + a * byz;
        float eyw   =  axw * bxy   - axy * bxw   + axz * bxyzw + ayw * b     + ayz * bzw   - azw * byz   + axyzw * bxz   + a * byw;
        float ezw   =  axw * bxz   - axy * bxyzw - axz * bxw   + ayw * byz   - ayz * byw   + azw * b     - axyzw * bxy   + a * bzw;
        float exyzw =  axw * byz   + axy * bzw   - axz * byw   - ayw * bxz   + ayz * bxw   + azw * bxy   + axyzw * b     + a * bxyzw;

        return new Rotor4(e, exy, exz, eyz, exw, eyw, ezw, exyzw);
    }

    // Rotor4-Vector4 Product (Rotation)
    public static Vector4 operator *(Rotor4 p, Vector4 a)
    {
        return p.Rotate(a);
    }

    public static Vector4 operator *(Vector4 a, Rotor4 p)
    {
        return p.Rotate(a);
    }

    // Rotate a Vector with a Rotor
    public Vector4 Rotate(Vector4 a)
    {
        float s = this.a;
        Vector4 r;

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

    // Rotate one Rotor to another
    public Rotor4 Rotate(Rotor4 r)
    {
        return this * r * this.Reverse();
    }

    // Conjugate
    public Rotor4 Reverse()
    {
        return new Rotor4(a, -bxy, -bxz, -byz, -bxw, -byw, -bzw, bxyzw);
    }

    private float sq(float x)
    {
        return x * x;
    }

    // Length Squared
    private float LengthSquared()
    {
        return sq(a) + sq(bxy) + sq(bxz) + sq(byz) +
                       sq(bxw) + sq(byw) + sq(bzw) + sq(bxyzw);
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
        bxyzw /= l;
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

    public override string ToString()
    {
        return a + " + " + bxy + " + " + bxz + " + " + byz + " + " + bxw + " + " + byw + " + " + bzw + " + " + bxyzw;
    }

    public float[] ToArray()
    {
        return new float[] { a, bxy, bxz, byz, bxw, byw, bzw, bxyzw };
    }

    // Equal
    public static bool equal(Rotor4 p, Rotor4 q)
    {
        if (p.a   == q.a   &&
            p.byz == q.byz &&
            p.bxz == q.bxz &&
            p.bxy == q.bxy &&
            p.bxw == q.bxw &&
            p.byw == q.byw &&
            p.bzw == q.bzw)
            return true;
        return false;
    }

    // Approximatly
    public static bool approx(Rotor4 p, Rotor4 q, double e)
    {
        if (range(p.a,   q.a,   e) &&
            range(p.byz, q.byz, e) &&
            range(p.bxz, q.bxz, e) &&
            range(p.bxy, q.bxy, e) &&
            range(p.bxw, q.bxw, e) &&
            range(p.byw, q.byw, e) &&
            range(p.bzw, q.bzw, e) )
            return true;
        return false;
    }
    
    // Equal within a range
    private static bool range(float a, float b, double epsilon)
    {
        if (Mathf.Abs(a) - Mathf.Abs(b) <= epsilon) return true;
        return false;
    }

    // Difference between 2 rotors
    public static float Difference(Rotor4 p, Rotor4 q)
    {
        Rotor4 dif = p * q.Reverse();
        return Mathf.Acos(dif.a);
    }
}
