using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotor4
{
    // Scalar
    // The rotation induced by the rotor = e**(theta)
    public float s;

    // Bivectors
    // Area of the Rotor bivector, projected onto each euclidian plane
    public float bzy;
    public float bxz;
    public float bxy;
    public float bxw;
    public float byw;
    public float bzw;

    // Quad-vector
    private float bxyzw;

    // Default Constructor
    public Rotor4()
    {
        this.s = 1;

        this.bzy = 0;
        this.bxz = 0;
        this.bxy = 0;
        this.bxw = 0;
        this.byw = 0;
        this.bzw = 0;

        this.bxyzw = 0;
    }

    // The wedge product of vectors a and b
    // returns an array of "areas" of the bivector, projected onto each euclidian plane
    private static float[] WedgeProduct(Vector4 a, Vector4 b)
    {
        //return s + (a.x * b.y - a.y * b.x) * bxy + (a.x * b.z - a.z * b.x) * bxz + (a.y * b.z - a.z * b.y) * bzy +
        //           (a.x * b.w - a.w * b.x) * bxw + (a.y * b.w - a.w * b.y) * byw + (a.z * b.w - a.w * b.z) * bzw;
        return new float[] { (a.x * b.y - a.y * b.x), (a.x * b.z - a.z * b.x), (a.y * b.z - a.z * b.y),
                             (a.x * b.w - a.w * b.x), (a.y * b.w - a.w * b.y), (a.z * b.w - a.w * b.z)};
    }

    // The geometric product of vectors a and b
    // notation: ab = a.b + a^b
    // Returns a rotor from vector a to vector b
    public static Rotor4 GeometricProduct(Vector4 a, Vector4 b)
    {
        float dot = Vector4.Dot(a, b);
        float[] wedge = WedgeProduct(a, b);

        float theta = Mathf.Acos(Vector4.Dot(a, b) / (a.magnitude * b.magnitude));

        Rotor4 r = new Rotor4();
        r.s = theta;
        r.bzy = dot + wedge[0];
        r.bxz = dot + wedge[1];
        r.bxy = dot + wedge[2];
        r.bxw = dot + wedge[3];
        r.byw = dot + wedge[4];
        r.bzw = dot + wedge[5];
        r.bxyzw = 0;

        r.Normalise();
        return r;
    }

    private Vector4 R(float s, float b, Vector4 v)
    {
        return Mathf.Cos(s) * v + Mathf.Sin(s) * b * v;
    }
    private Vector4 R_(float s, float b, Vector4 v)
    {
        return Mathf.Cos(s) * v - Mathf.Sin(s) * b * v;
    }

    // Rotate a 4d vector by the rotor
    // What will be in shader
    /*
    public Vector4 Rotate(Vector4 v) 
    {
        Vector4 u = R(s, bxy, new Vector4(1,1,0,0)) * R(s, bxz, new Vector4(1, 0, 1, 0)) * R(s, bzy, new Vector4(0, 1, 1, 0)) * 
                    v *
                    R_(s, bxy, new Vector4(1, 1, 0, 0)) * R_(s, bxz, new Vector4(1, 0, 1, 0)) * R_(s, bzy, new Vector4(0, 1, 1, 0));
        return u;
    }*/

    // Used to obtain the composite of rotors R1 and R2 (combining the rotations into a single rotation)
    public static Rotor4 operator *(Rotor4 r1, Rotor4 r2)
    {
        Rotor4 r = new Rotor4();
        
        //r.s = r1.s*r2.s;

        r.bzy = Mathf.Log(Mathf.Exp(r1.bzy) * Mathf.Exp(r2.bzy));
        r.bxz = Mathf.Log(Mathf.Exp(r1.bxz) * Mathf.Exp(r2.bxz));
        r.bxy = Mathf.Log(Mathf.Exp(r1.bxy) * Mathf.Exp(r2.bxy));
        r.bxw = Mathf.Log(Mathf.Exp(r1.bxw) * Mathf.Exp(r2.bxw));
        r.byw = Mathf.Log(Mathf.Exp(r1.byw) * Mathf.Exp(r2.byw));
        r.bzw = Mathf.Log(Mathf.Exp(r1.bzw) * Mathf.Exp(r2.bzw));
        
        r.bxyzw = 0;
        r.Normalise();

        return r;
    }
    /*
    public static Vector4 operator *(Vector4 u, Vector4 v)
    {
        return new Vector4(u.x*v.x, u.y * v.y, u.z * v.z, u.w * v.w);
    }*/
    // ======================================
    // Utility
    // ======================================

    // Square of x
    public float square(float x)
    {
        return (float) (x * x);
    }

    //TODO
    public float Length()
    {
        return 0;//a * bzy + b * bxz + c * bxy + d * bxw + e * yw + e * zw;
    }

    //TODO
    public float LengthSquared()
    {
        return 0;// - (square(a), square(b), square(c) ...);
    }
    
    // The magnitude of the rotor
    public float Magnitude()
    {
        return Mathf.Sqrt(square(s) + square(bzy) + square(bxz) + square(bxy) + square(bxw) + square(byw) + square(bzw) + square(bxyzw));
    }

    // Normalise the rotor such that the sum of its components add to 1
    public void Normalise()
    {
        //this.s   /= this.Magnitude();

        this.bzy /= this.Magnitude();
        this.bxz /= this.Magnitude();
        this.bxy /= this.Magnitude();
        this.bxw /= this.Magnitude();
        this.byw /= this.Magnitude();
        this.bzw /= this.Magnitude();

        this.bxyzw /= this.Magnitude();
    }

    public string ToString()
    {
        return s.ToString() +" "+ bzy.ToString() + " " + bxz.ToString() + " " + bxy.ToString() + " " + bxw.ToString() + " " + byw.ToString() + " " + bzw.ToString();
    }
}
