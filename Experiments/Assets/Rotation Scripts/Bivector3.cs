using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bivector3
{
    public float b01 = 0;
    public float b02 = 0;
    public float b12 = 0;

    public Bivector3(float b01, float b02, float b12)
    {
        this.b01 = b01;
        this.b02 = b02;
        this.b12 = b12;
    }

    public static Bivector3 Wedge(Vector3 u, Vector3 v)
    {
        Bivector3 b = new Bivector3(
                u.x * v.y - u.y * v.x, //XY
                u.x * v.z - u.z * v.x, //XZ
                u.y * v.z - u.z * v.y  //YZ
            );
        return b;
    }
}
