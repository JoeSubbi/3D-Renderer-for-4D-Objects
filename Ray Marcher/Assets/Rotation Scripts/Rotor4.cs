using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotor4
{
    // Scalar
    // The rotation induced by the rotor = e**(theta)
    public double s;

    // Bivectors
    // Area of the Rotor bivector, projected onto each euclidian plane
    public double bzy;
    public double bxz;
    public double bxy;
    public double bxw;
    public double byw;
    public double bzw;

    // Quad-vector
    private double bxyzw;

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
    private double[] WedgeProduct(Vector4 a, Vector4 b)
    {
        //return s + (a.x * b.y - a.y * b.x) * bxy + (a.x * b.z - a.z * b.x) * bxz + (a.y * b.z - a.z * b.y) * bzy +
        //           (a.x * b.w - a.w * b.x) * bxw + (a.y * b.w - a.w * b.y) * byw + (a.z * b.w - a.w * b.z) * bzw;
        return new double[] { (a.x * b.y - a.y * b.x), (a.x * b.z - a.z * b.x), (a.y * b.z - a.z * b.y),
                              (a.x * b.w - a.w * b.x), (a.y * b.w - a.w * b.y), (a.z * b.w - a.w * b.z)};
    }

    // The geometric product of vectors a and b
    // notation: ab = a.b + a^b
    // Returns a rotor from vector a to vector b
    private Rotor4 GeometricProduct(Vector4 a, Vector4 b)
    {
        double dot = Vector4.Dot(a, b);
        double[] wedge = WedgeProduct(a, b);

        //double theta = Mathf.Acos(Vector4.Dot(a, b) / (a.magnitude * b.magnitude));

        Rotor4 r = new Rotor4();
        r.s = 1;
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

    // Rotate a 4d vector by the rotor
    // TODO: PRIORITY - WHAT WILL BE USED IN SHADER
    public Vector4 Rotate(Vector4 v) 
    {
        // Define a plane/oreintation
        // Multiply vector
        return v;
    }

    // Used to obtain the composite of rotors R1 and R2 (combining the rotations into a single rotation)
    // TODO: NOT NECESSARY YET - 2ND PRIORITY
    public static Rotor4 operator *(Rotor4 r1, Rotor4 r2)
    {
        return r1;
    }

    // ======================================
    // Utility
    // ======================================

    // Square of x
    public float square(double x)
    {
        return (float) (x * x);
    }

    //TODO
    public double Length()
    {
        return 0;//a * bzy + b * bxz + c * bxy + d * bxw + e * yw + e * zw;
    }

    //TODO
    public double LengthSquared()
    {
        return 0;// - (square(a), square(b), square(c) ...);
    }
    
    // The magnitude of the rotor
    public double Magnitude()
    {
        return Mathf.Sqrt(square(s) + square(bzy) + square(bxz) + square(bxy) + square(bxw) + square(byw) + square(bzw) + square(bxyzw));
    }

    // Normalise the rotor such that the sum of its components add to 1
    public void Normalise()
    {
        this.s   /= this.Magnitude();

        this.bzy /= this.Magnitude();
        this.bxz /= this.Magnitude();
        this.bxy /= this.Magnitude();
        this.bxw /= this.Magnitude();
        this.byw /= this.Magnitude();
        this.bzw /= this.Magnitude();

        this.bxyzw /= this.Magnitude();
    }
}
