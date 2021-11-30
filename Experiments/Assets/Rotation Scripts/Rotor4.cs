﻿using System.Collections;
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

    // Rotor4-Rotor4 Product - Global Axis
    public static Rotor4 operator /(Rotor4 p, Rotor4 q)
    {
        float ae   = p.a;
        float ae12 = p.bxy;
        float ae31 = p.bxz;
        float ae23 = p.byz;
        float ae41 = p.bxw;
        float ae42 = p.byw;
        float ae43 = p.bzw;
        float ae1234 = p.bxyzw;

        float be   = q.a;
        float be12 = q.bxy;
        float be31 = q.bxz;
        float be23 = q.byz;
        float be41 = q.bxw;
        float be42 = q.byw;
        float be43 = q.bzw;
        float be1234 = q.bxyzw;

        /*/ Unsigned
        float e     =   ae * be     - ae12 * be12   - ae31 * be31   - ae23 * be23   - ae41 * be41   - ae42 * be42   - ae43 * be43   + ae1234 * be1234;
		float e12   =   ae * be12   + ae12 * be     - ae31 * be23   + ae23 * be31   + ae41 * be42   - ae42 * be41   + ae43 * be1234 + ae1234 * be43;
		float e31   = - ae * be31   - ae12 * be23   + ae31 * be     + ae23 * be12   + ae41 * be43   - ae42 * be1234 - ae43 * be41   - ae1234 * be42;
		float e23   =   ae * be23   - ae12 * be31   + ae31 * be12   + ae23 * be     - ae41 * be1234 + ae42 * be43   - ae43 * be42   + ae1234 * be41;
		float e41   = - ae * be41   - ae12 * be41   + ae31 * be43   + ae23 * be1234 + ae41 * be     + ae42 * be12   - ae43 * be31   + ae1234 * be23;
		float e42   = - ae * be42   - ae12 * be42   - ae31 * be1234 + ae23 * be43   + ae41 * be12   + ae42 * be     - ae43 * be23   - ae1234 * be31;
		float e43   = - ae * be43   - ae12 * be43   - ae31 * be41   + ae23 * be42   + ae41 * be31   - ae42 * be23   + ae43 * be     + ae1234 * be12;
		float e1234 =   ae * be1234 + ae12 * be1234 - ae31 * be42   - ae23 * be41   - ae41 * be23   - ae42 * be31   - ae43 * be12   + ae1234 * be;
        */

        // Signed
        float e     = ae * be     - ae12 * be12   - ae31 * be31   - ae23 * be23   - ae41 * be41   - ae42 * be42   - ae43 * be43   + ae1234 * be1234;
		float e12   = ae * be12   + ae12 * be     + ae31 * be23   - ae23 * be31   + ae41 * be42   - ae42 * be41   - ae43 * be1234 - ae1234 * be43;
		float e31   = ae * be31   - ae12 * be23   + ae31 * be     + ae23 * be12   + ae41 * be43   + ae42 * be1234 - ae43 * be41   + ae1234 * be42;
		float e23   = ae * be23   + ae12 * be31   - ae31 * be12   + ae23 * be     - ae41 * be1234 + ae42 * be43   - ae43 * be42   - ae1234 * be41;
		float e41   = ae * be41   + ae12 * be42   + ae31 * be43   + ae23 * be1234 + ae41 * be     - ae42 * be12   - ae43 * be31   + ae1234 * be23;
		float e42   = ae * be42   + ae12 * be41   + ae31 * be1234 - ae23 * be43   - ae41 * be12   + ae42 * be     + ae43 * be23   + ae1234 * be31;
		float e43   = ae * be43   + ae12 * be1234 - ae31 * be41   - ae23 * be42   + ae41 * be31   + ae42 * be23   + ae43 * be     + ae1234 * be12;
		float e1234 = ae * be1234 + ae12 * be43   - ae31 * be42   + ae23 * be41   + ae41 * be23   - ae42 * be31   + ae43 * be12   + ae1234 * be;

        return new Rotor4(e, e12, e31, e23, e41, e42, e43, e1234);
    }

    // Rotor4-Rotor4 Product - Local Axis
    public static Rotor4 operator *(Rotor4 p, Rotor4 q)
    {
        float ae = p.a;
        float ae12 = p.bxy;
        float ae31 = -p.bxz;
        float ae23 = p.byz;
        float ae41 = -p.bxw;
        float ae42 = -p.byw;
        float ae43 = -p.bzw;
        float ae1234 = p.bxyzw;

        float be = q.a;
        float be12 = q.bxy;
        float be31 = -q.bxz;
        float be23 = q.byz;
        float be41 = -q.bxw;
        float be42 = -q.byw;
        float be43 = -q.bzw;
        float be1234 = q.bxyzw;

        /*/ Unsigned
        float e     =   ae * be     - ae12 * be12   - ae31 * be31   - ae23 * be23   - ae41 * be41   - ae42 * be42   - ae43 * be43   + ae1234 * be1234;
		float e12   =   ae * be12   + ae12 * be     - ae31 * be23   + ae23 * be31   + ae41 * be42   - ae42 * be41   + ae43 * be1234 + ae1234 * be43;
		float e31   = - ae * be31   - ae12 * be23   + ae31 * be     + ae23 * be12   + ae41 * be43   - ae42 * be1234 - ae43 * be41   - ae1234 * be42;
		float e23   =   ae * be23   - ae12 * be31   + ae31 * be12   + ae23 * be     - ae41 * be1234 + ae42 * be43   - ae43 * be42   + ae1234 * be41;
		float e41   = - ae * be41   - ae12 * be41   + ae31 * be43   + ae23 * be1234 + ae41 * be     + ae42 * be12   - ae43 * be31   + ae1234 * be23;
		float e42   = - ae * be42   - ae12 * be42   - ae31 * be1234 + ae23 * be43   + ae41 * be12   + ae42 * be     - ae43 * be23   - ae1234 * be31;
		float e43   = - ae * be43   - ae12 * be43   - ae31 * be41   + ae23 * be42   + ae41 * be31   - ae42 * be23   + ae43 * be     + ae1234 * be12;
		float e1234 =   ae * be1234 + ae12 * be1234 - ae31 * be42   - ae23 * be41   - ae41 * be23   - ae42 * be31   - ae43 * be12   + ae1234 * be;
        */

        // Signed
        float e = ae * be - ae12 * be12 - ae31 * be31 - ae23 * be23 - ae41 * be41 - ae42 * be42 - ae43 * be43 + ae1234 * be1234;
        float e12 = ae * be12 + ae12 * be + ae31 * be23 - ae23 * be31 + ae41 * be42 - ae42 * be41 - ae43 * be1234 - ae1234 * be43;
        float e31 = ae * be31 - ae12 * be23 + ae31 * be + ae23 * be12 + ae41 * be43 + ae42 * be1234 - ae43 * be41 + ae1234 * be42;
        float e23 = ae * be23 + ae12 * be31 - ae31 * be12 + ae23 * be - ae41 * be1234 + ae42 * be43 - ae43 * be42 - ae1234 * be41;
        float e41 = ae * be41 + ae12 * be42 + ae31 * be43 + ae23 * be1234 + ae41 * be - ae42 * be12 - ae43 * be31 + ae1234 * be23;
        float e42 = ae * be42 + ae12 * be41 + ae31 * be1234 - ae23 * be43 - ae41 * be12 + ae42 * be + ae43 * be23 + ae1234 * be31;
        float e43 = ae * be43 + ae12 * be1234 - ae31 * be41 - ae23 * be42 + ae41 * be31 + ae42 * be23 + ae43 * be + ae1234 * be12;
        float e1234 = ae * be1234 + ae12 * be43 - ae31 * be42 + ae23 * be41 + ae41 * be23 - ae42 * be31 + ae43 * be12 + ae1234 * be;

        return new Rotor4(e, e12, -e31, e23, -e41, -e42, -e43, e1234);
    }

    // Rotate a Vector with a Rotor
    public Vector4 Rotate(Vector4 u)
    {
        Rotor4 p = this;

        float be1 = u.x;
        float be2 = u.y;
        float be3 = u.z;
        float be4 = u.w;

        float ae = p.a;
        float ae12 = p.bxy;
        float ae31 = p.bxz;
        float ae23 = p.byz;
        float ae41 = p.bxw;
        float ae42 = p.byw;
        float ae43 = p.bzw;
        float ae1234 = p.bxyzw;

        // q = Px
        Vector4 q;
        // xP
        /*
        q.x = ae1 * be - ae2 * be12 + ae3 * be31;
        q.y = ae2 * be + ae1 * be12 - ae3 * be23 + ae4 * be42;
        q.z = ae3 * be - ae1 * be31 + ae2 * be23 - ae4 * be43;
        q.w = ae4 * be + ae1 * be41 - ae2 * be42 + ae3 * be43 - ae4 * be41;

        float q123 =  ae1 * be23   + ae2 * be31   + ae3 * be12   - ae4 * be1234;
        float q134 = -ae1 * be43   + ae2 * be1234 + ae3 * be41   + ae4 * be31;
        float q142 =  ae1 * be42   + ae2 * be41   - ae3 * be1234 - ae4 * be12;
        float q324 =  ae1 * be1234 + ae2 * be43   + ae3 * be42   + ae4 * be23;
        
        /*/ //?
        q.x =  ae   * be1 + ae12 * be2 - ae31 * be3 + ae41 * be4;
	    q.y = -ae12 * be1 + ae   * be2 + ae23 * be3 - ae42 * be4;
	    q.z =  ae31 * be1 - ae   * be3 + ae23 * be2 + ae43 * be4;
	    q.w = -ae41 * be1 + ae   * be4 + ae42 * be2 - ae43 * be3;

	    float q123 =  ae23   * be1 + ae31   * be2 + ae12   * be3 + ae1234 * be4;
	    float q134 = -ae43   * be1 - ae1234 * be2 + ae41   * be3 + ae31   * be4;
	    float q142 =  ae42   * be1 + ae41   * be2 + ae1234 * be3 - ae12   * be4;
	    float q324 = -ae1234 * be1 + ae43   * be2 + ae42   * be3 + ae23   * be4;
        
        /*/ Unsigned
        q.x = ae * be1 + ae12 * be2 - ae31 * be3 + ae41 * be4;
        q.y = ae * be2 - ae12 * be1 + ae23 * be3 - ae42 * be4;
        q.z = ae * be3 + ae31 * be1 - ae23 * be2 + ae43 * be4;
        q.w = ae * be4 - ae41 * be1 + ae42 * be2 - ae43 * be3;

        float q123 =  ae12 * be3 + ae31 * be2 + ae23 * be1 + ae1234 * be4;
        float q134 =  ae31 * be4 + ae41 * be3 - ae43 * be1 - ae1234 * be2;
        float q142 = -ae12 * be4 + ae41 * be2 + ae42 * be1 + ae1234 * be3;
        float q324 =  ae23 * be4 + ae42 * be3 + ae43 * be2 - ae1234 * be1;
        */
        /*/ Signed
        q.x = ae * be1 + ae12 * be2 + ae31 * be3 - ae41 * be4;
        q.y = ae * be2 - ae12 * be1 + ae23 * be3 + ae42 * be4;
        q.z = ae * be3 - ae31 * be1 - ae23 * be2 - ae43 * be4;
        q.w = ae * be4 + ae41 * be1 - ae42 * be2 + ae43 * be3;

        float q123 =   ae12 * be3 - ae31 * be2 + ae23 * be1 + ae1234 * be4;
        float q142 = - ae12 * be4 - ae41 * be2 - ae42 * be1 + ae1234 * be3;
        float q134 = - ae31 * be4 - ae41 * be3 + ae43 * be1 - ae1234 * be2;
        float q324 =   ae23 * be4 - ae42 * be3 - ae43 * be2 - ae1234 * be1;
        */
        
        //p = p.Reverse();
        float be = p.a;
        float be12 = p.bxy;
        float be31 = p.bxz;
        float be23 = p.byz;
        float be41 = p.bxw;
        float be42 = p.byw;
        float be43 = p.bzw;
        float be1234 = p.bxyzw;

        float ae1 = q.x;
        float ae2 = q.y;
        float ae3 = q.z;
        float ae4 = q.w;
        float ae123 = -q123;
        float ae134 = -q134;
        float ae142 = -q142;
        float ae324 = -q324;

        // r = qP*
        Vector4 r;
        // P*q
        /*
        r.x = ae * be1 + ae12 * be2   - ae31 * be3   + ae23 * be123 + ae41 * be4   + ae42 * be134 + ae43 * be142 - ae1234 * be324;
        r.y = ae * be2 - ae12 * be1   + ae31 * be123 + ae23 * be3   + ae41 * be134 - ae42 * be4   - ae43 * be324 - ae1234 * be142;
        r.z = ae * be3 + ae12 * be123 + ae31 * be1   - ae23 * be2   - ae41 * be142 - ae42 * be324 + ae43 * be4   - ae1234 * be134;
        r.w = ae * be4 - ae12 * be142 - ae31 * be134 - ae23 * be324 - ae41 * be1   + ae42 * be2   - ae43 * be3   - ae1234 * be123;
        */
        /* Unsigned
        r.x =  ae1 * be   - ae2 * be12 + ae3 * be31                           + ae123 * be23   + ae134 * be43   + ae142 * be42   + ae324 * be1234;
	    r.y =  ae1 * be12 + ae2 * be   - ae3 * be23 + ae4 * be42              + ae123 * be31   + ae134 * be1234 + ae142 * be41   - ae324 * be43;
	    r.z = -ae1 * be31 + ae2 * be23 + ae3 * be   - ae4 * be43              + ae123 * be12   - ae134 * be41   + ae142 * be1234 - ae324 * be42;
	    r.w =  ae1 * be41 - ae2 * be42 + ae3 * be43 + ae4 * be   - ae4 * be41 + ae123 * be1234 - ae134 * be31   - ae142 * be12   - ae324 * be23;
        */
        
        // Signed including negative for be1234
        r.x =   ae1 * be   + ae2 * be12 + ae3 * be31                           - ae123 * be23   + ae142 * be42   + ae134 * be43   - ae324 * be1234;
        r.y = - ae1 * be12 + ae2 * be   + ae3 * be23 + ae4 * be42              + ae123 * be31   + ae142 * be41   - ae134 * be1234 - ae324 * be43;
        r.z = - ae1 * be31 - ae2 * be23 + ae3 * be   - ae4 * be43              - ae123 * be12   - ae142 * be1234 - ae134 * be41   - ae324 * be42;
        r.w =   ae1 * be41 - ae2 * be42 + ae3 * be43 + ae4 * be   - ae4 * be41 - ae123 * be1234 + ae142 * be12   - ae134 * be31   + ae324 * be23;
        
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
