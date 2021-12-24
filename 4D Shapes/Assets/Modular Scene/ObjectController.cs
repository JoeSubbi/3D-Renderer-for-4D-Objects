﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ObjectController : MonoBehaviour
{
    // Object Windows
    public GameObject main;
    public GameObject mini;
    public GameObject match;

    public static Renderer mainRenderer;
    public static Renderer miniRenderer;
    public static Renderer matchRenderer;

    // Object Controls
    public int shape;
    public int effect;
    public static float w = 0;
    public static bool continuousRotation = false;
    // Boolean Array for rotation in YZ, XZ, XY, XW, YW, ZW
    public static bool[] rotations = new bool[] { false, false, false, false, false, false };
    public Slider wSlider;

    // Rotors for the main object and randomly posed object
    public static Rotor4 mainRot  = new Rotor4(1, 0, 0, 0, 0, 0, 0, 0);
    public static Rotor4 matchRot = new Rotor4(1, 0, 0, 0, 0, 0, 0, 0);
    public static Rotor4 miniRot  = new Rotor4(1, 0, 0, 0, 0, 0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        mainRenderer = main.GetComponent<Renderer>();
        miniRenderer = mini.GetComponent<Renderer>();
        matchRenderer = match.GetComponent<Renderer>();

        SetRandMatchObjectRotation();
    }

    // Update is called once per frame
    void Update()
    {
        // Set object W position
        mainRenderer.material.SetFloat("_W", wSlider.value + w);

        SetObjectShape(shape);
        SetObjectTexture(effect);

        if (continuousRotation)
            TimedRotor(rotations);
    }

    // Set shape
    public static void SetObjectShape(int s)
    {
        mainRenderer.material.SetInt("_Shape", s);
        matchRenderer.material.SetInt("_Shape", s);
        // Set 3D representation shape
        // To cube? - Do not want it interfereing with experiments?
        // To main object? - nullifies Shape_Match test
        miniRenderer.material.SetInt("_Shape", 1);
    }

    // Set effect
    public static void SetObjectTexture(int t)
    {
        mainRenderer.material.SetInt("_Effect", t);
        matchRenderer.material.SetInt("_Effect", t);
    }

    // Set rotation for each  object
    public static void SetMainObjectRotation()
    {
        SetObjectRotation(mainRenderer, mainRot);
    }

    public static void SetMiniObjectRotation()
    {
        SetObjectRotation(miniRenderer, miniRot);
    }

    public static void SetMatchObjectRotation()
    {
        SetObjectRotation(matchRenderer, matchRot);
    }

    private static void SetObjectRotation(Renderer rend, Rotor4 rot)
    {
        rend.material.SetFloat("_A", rot.a);
        rend.material.SetFloat("_YZ", rot.byz);
        rend.material.SetFloat("_XZ", rot.bxz);
        rend.material.SetFloat("_XY", rot.bxy);
        rend.material.SetFloat("_XW", rot.bxw);
        rend.material.SetFloat("_YW", rot.byw);
        rend.material.SetFloat("_ZW", rot.bzw);
        rend.material.SetFloat("_XYZW", rot.bxyzw);
    }

    // Set random rotation for the main object
    public static void SetRandMainObjectRotation()
    {
        Vector4 e1;
        Vector4 e2;
        Bivector4 bv;
        Rotor4 r;
        float a = 3.14159f; // Rotor has double coverage - a full rotation is 4pi
                            // If I specify rotation of pi it is doubled to 2pi

        e1 = new Vector4(0, 1, 0, 0);
        e2 = new Vector4(0, 0, 1, 0);
        bv = Bivector4.Wedge(e1, e2);
        r = new Rotor4(bv, Random.Range(0, a));

        e1 = new Vector4(1, 0, 0, 0);
        e2 = new Vector4(0, 0, 1, 0);
        bv = Bivector4.Wedge(e1, e2);
        r *= new Rotor4(bv, Random.Range(0, a));

        e1 = new Vector4(1, 0, 0, 0);
        e2 = new Vector4(0, 1, 0, 0);
        bv = Bivector4.Wedge(e1, e2);
        r *= new Rotor4(bv, Random.Range(0, a));

        e1 = new Vector4(1, 0, 0, 0);
        e2 = new Vector4(0, 0, 0, 1);
        bv = Bivector4.Wedge(e1, e2);
        r = new Rotor4(bv, Random.Range(0, a));

        e1 = new Vector4(0, 1, 0, 0);
        e2 = new Vector4(0, 0, 0, 1);
        bv = Bivector4.Wedge(e1, e2);
        r *= new Rotor4(bv, Random.Range(0, a));

        e1 = new Vector4(0, 0, 1, 0);
        e2 = new Vector4(0, 0, 0, 1);
        bv = Bivector4.Wedge(e1, e2);
        r *= new Rotor4(bv, Random.Range(0, a));

        mainRot *= r;
        mainRot.Normalise();
        SetMatchObjectRotation();
    }

    // Set random rotation for the match pose object
    public static void SetRandMatchObjectRotation()
    {
        Vector4 e1;
        Vector4 e2;
        Bivector4 bv;
        Rotor4 r;
        float a = 3.14159f; // Rotor has double coverage - a full rotation is 4pi
                            // If I specify rotation of pi it is doubled to 2pi

        e1 = new Vector4(1, 0, 0, 0);
        e2 = new Vector4(0, 0, 0, 1);
        bv = Bivector4.Wedge(e1, e2);
        r  = new Rotor4(bv, Random.Range(0, a));
        
        e1 = new Vector4(0, 1, 0, 0);
        e2 = new Vector4(0, 0, 0, 1);
        bv = Bivector4.Wedge(e1, e2);
        r *= new Rotor4(bv, Random.Range(0, a));

        e1 = new Vector4(0, 0, 1, 0);
        e2 = new Vector4(0, 0, 0, 1);
        bv = Bivector4.Wedge(e1, e2);
        r *= new Rotor4(bv, Random.Range(0, a));

        matchRot *= r;
        matchRot.Normalise();
        SetMatchObjectRotation();
    }

    // Set continuous rotation of the main object
    private void TimedRotor(bool[] rotations)
    {
        Vector4 e1;
        Vector4 e2;
        Bivector4 bv;
        Rotor4 r = new Rotor4(1, 0, 0, 0, 0, 0, 0, 0);
        float a = Time.deltaTime;

        if (rotations[0])
        {
            e1 = new Vector4(0, 1, 0, 0);
            e2 = new Vector4(0, 0, 0, 1);
            bv = Bivector4.Wedge(e1, e2);
            r *= new Rotor4(bv, a);
        }
        if (rotations[1])
        {
            e1 = new Vector4(1, 0, 0, 0);
            e2 = new Vector4(0, 0, 0, 1);
            bv = Bivector4.Wedge(e1, e2);
            r *= new Rotor4(bv, a);
        }
        if (rotations[2])
        {
            e1 = new Vector4(0, 0, 1, 0);
            e2 = new Vector4(0, 0, 0, 1);
            bv = Bivector4.Wedge(e1, e2);
            r *= new Rotor4(bv, a);
        }
        if (rotations[3])
        {
            e1 = new Vector4(0, 1, 0, 0);
            e2 = new Vector4(0, 0, 1, 0);
            bv = Bivector4.Wedge(e1, e2);
            r *= new Rotor4(bv, a);
        }
        if (rotations[4])
        {
            e1 = new Vector4(1, 0, 0, 0);
            e2 = new Vector4(0, 1, 0, 0);
            bv = Bivector4.Wedge(e1, e2);
            r *= new Rotor4(bv, a);
        }
        if (rotations[5])
        {
            e1 = new Vector4(1, 0, 0, 0);
            e2 = new Vector4(0, 0, 1, 0);
            bv = Bivector4.Wedge(e1, e2);
            r *= new Rotor4(bv, a);
        }

        mainRot *= r;
        mainRot.Normalise();
        SetMainObjectRotation();
    }

}