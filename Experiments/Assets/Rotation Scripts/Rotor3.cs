using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Implementation of a Rotor3 class based on Marc ten Bosch's c++ implementation
 * https://marctenbosch.com/quaternions/code.htm
 */

public class Rotor3
{
    // scalar part
    public float a = 1;

    // bivector part
    public float b01 = 0;
    public float b02 = 0;
    public float b12 = 0;

    // CONSTRUCTORS
    public Rotor3(float a, float b01, float b02, float b12)
    {
        this.a = a;
        this.b01 = b01;
        this.b02 = b02;
        this.b12 = b12;
    }

    public Rotor3(Bivector3 bv, float angle)
    {
        float sina = Mathf.Sin(angle / 2);
        a = Mathf.Cos(angle / 2);
        this.b01 = -sina * bv.b01;
        this.b02 = -sina * bv.b02;
        this.b12 = -sina * bv.b12;
    }

    // Rotor3-Rotor3 Product
    public static Rotor3 operator *(Rotor3 p, Rotor3 q)
    {
        float a   = p.a   * q.a - p.b01 * q.b01 - p.b02 * q.b02 - p.b12 * q.b12;
        float b01 = p.b01 * q.a + p.a   * q.b01 + p.b12 * q.b02 - p.b02 * q.b12;
        float b02 = p.b02 * q.a + p.a   * q.b02 - p.b12 * q.b01 + p.b01 * q.b12;
        float b12 = p.b12 * q.a + p.a   * q.b12 + p.b02 * q.b01 - p.b01 * q.b02;

        p = new Rotor3(a, b01, b02, b12);
        return p;
    }

    // Rotate a Vector with a Rotor
    public Vector3 Rotate(Vector3 u)
    {
        Rotor3 p = this;

        // q = Px
        Vector3 q;
        q.x = p.a * u.x + u.y * p.b01 + u.z * p.b02;
        q.y = p.a * u.y - u.x * p.b01 + u.z * p.b12;
        q.z = p.a * u.z - u.x * p.b02 - u.y * p.b12;

        float q012 = u.x * p.b12 - u.y * p.b02 + u.z * p.b01;

        // r = qP*
        Vector3 r;
        r.x = p.a * q.x + q.y  * p.b01 + q.z  * p.b02 + q012 * p.b12;
        r.y = p.a * q.y - q.x  * p.b01 - q012 * p.b02 + q.z  * p.b12;
        r.z = p.a * q.z + q012 * p.b01 - q.x  * p.b02 - q.y  * p.b12;

        return r;
    }

    // Rotate one Rotor to another
    public Rotor3 Rotate(Rotor3 r)
    {
        return this * r * this.Reverse();
    }

    // Conjugate
    public Rotor3 Reverse()
    {
        return new Rotor3(a, -b01, -b02, -b12);
    }

    // Length Squared
    private float LengthSquared()
    {
        return a * a + b01 * b01 + b02 * b02 + b12 * b12;
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
        b01 /= l;
        b02 /= l;
        b12 /= l;
    }

    // Normalised copy of a rotor
    public Rotor3 Normal()
    {
        Rotor3 r = this;
        r.Normalise();
        return r;
    }

    // Geometric Product
    public static Rotor3 GeoProd( Vector3 a, Vector3 b)
    {
        Bivector3 bv = Bivector3.Wedge(a, b);
        return new Rotor3(Vector3.Dot(a, b), bv.b01, bv.b02, bv.b12);
    }
}
