using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quaternion4
{
    public float s;
    public float x;
    public float y;
    public float z;
    public float w;

    public Quaternion4(float x, float y, float z, float w, float s)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
        this.s = s;
    }

    public float magnitude()
    {
        return Mathf.Sqrt(x * x + y * y + z * z + w * w + s * s);
    }

    public void normalise()
    {
        this.x /= this.magnitude();
        this.y /= this.magnitude();
        this.z /= this.magnitude();
        this.w /= this.magnitude();
        this.s /= this.magnitude();
    }

    public static Quaternion4 operator *(Quaternion4 q1, Quaternion4 q2)
    {
        float s = q1.s * q2.s - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z;
        float x = q1.s * q2.x + q1.x * q2.s + q1.y * q2.z - q1.z * q2.y;
        float y = q1.s * q2.y - q1.x * q2.z + q1.y * q2.s + q1.z * q2.x;
        float z = q1.s * q2.z + q1.x * q2.y - q1.y * q2.x + q1.z * q2.s;
        float w = 0;
        return new Quaternion4(x,y,z,w,s);
    }

    public static Quaternion4 AngleAxis(float angle, Vector3 axis)
    {
        return new Quaternion4(0, 0, 0, 0, 1);
    }
}
