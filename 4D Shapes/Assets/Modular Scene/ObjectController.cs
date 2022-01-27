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

    private static Renderer mainRenderer;
    private static Renderer miniRenderer;
    private static Renderer matchRenderer;

    // Object Controls
    public static float w = 0;
    public Slider wSlider;

    // Rotors for the main object and randomly posed object
    public static Rotor4 mainRot  = new Rotor4(1, 0, 0, 0, 0, 0, 0, 0);
    public static Rotor4 matchRot = new Rotor4(1, 0, 0, 0, 0, 0, 0, 0);
    public static Rotor4 miniRot  = new Rotor4(1, 0, 0, 0, 0, 0, 0, 0);
    public static Rotor4 initialMatch;
    public static Rotor4 initialMain = new Rotor4(1, 0, 0, 0, 0, 0, 0, 0);

    // Listners
    public static int wCount = 0;
    public static int swipeCount = 0;

    // Start is called before the first frame update
    void Awake()
    {
        mainRenderer = main.GetComponent<Renderer>();
        miniRenderer = mini.GetComponent<Renderer>();
        matchRenderer = match.GetComponent<Renderer>();

        SetRandMatchObjectRotation();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(wCount + " " + swipeCount);

        // Set object W position
        mainRenderer.material.SetFloat("_W", wSlider.value + w);

        if (UIController.Rotation_Match)
            TimedRotor(StateController.rotations);
        if ((!UIController.Shape_Match) && Input.GetKeyDown("space"))
            SoftReset();
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

    public static void Reset()
    {
        w = 0;
        wCount = 0;
        swipeCount = 0;

        mainRot  = new Rotor4(1, 0, 0, 0, 0, 0, 0, 0);
        matchRot = new Rotor4(1, 0, 0, 0, 0, 0, 0, 0);
        miniRot  = new Rotor4(1, 0, 0, 0, 0, 0, 0, 0);

        SetMainObjectRotation();
        SetMatchObjectRotation();
        SetMiniObjectRotation();
    }

    public static void SoftReset()
    {
        w = 0;

        mainRot = new Rotor4(1, 0, 0, 0, 0, 0, 0, 0);
        miniRot = new Rotor4(1, 0, 0, 0, 0, 0, 0, 0);
        matchRot = initialMatch;

        SetMainObjectRotation();
        SetMatchObjectRotation();
        SetMiniObjectRotation();
    }

    public void ResetCounters()
    {
        wCount = 0;
        swipeCount = 0;
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
        float accuracy = Rotor4.Difference(mainRot, matchRot);
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

        // 3D Rotation
        float a1 = Random.Range(0, a);
        float a2 = Random.Range(0, a);
        float a3 = Random.Range(0, a);

        e1 = new Vector4(0, 1, 0, 0);
        e2 = new Vector4(0, 0, 1, 0);
        bv = Bivector4.Wedge(e1, e2);
        r = new Rotor4(bv, a1);

        e1 = new Vector4(1, 0, 0, 0);
        e2 = new Vector4(0, 0, 1, 0);
        bv = Bivector4.Wedge(e1, e2);
        r *= new Rotor4(bv, Random.Range(0, a2));

        e1 = new Vector4(1, 0, 0, 0);
        e2 = new Vector4(0, 1, 0, 0);
        bv = Bivector4.Wedge(e1, e2);
        r *= new Rotor4(bv, Random.Range(0, a3));

        // 4D Rotation
        // If the shape looks like a sphere but is not a sphere, consider not rotating it
        if (StateController.shape == 2 || StateController.shape == 6)
            if (Random.value >= 0.3) a = 0;

        a1 = Random.Range(0, a);
        a2 = Random.Range(0, a);
        a3 = Random.Range(0, a);

        e1 = new Vector4(1, 0, 0, 0);
        e2 = new Vector4(0, 0, 0, 1);
        bv = Bivector4.Wedge(e1, e2);
        r *= new Rotor4(bv, Random.Range(0, a1));

        e1 = new Vector4(0, 1, 0, 0);
        e2 = new Vector4(0, 0, 0, 1);
        bv = Bivector4.Wedge(e1, e2);
        r *= new Rotor4(bv, Random.Range(0, a2));

        e1 = new Vector4(0, 0, 1, 0);
        e2 = new Vector4(0, 0, 0, 1);
        bv = Bivector4.Wedge(e1, e2);
        r *= new Rotor4(bv, Random.Range(0, a3));

        mainRot *= r;
        mainRot.Normalise();
        SetMainObjectRotation();
        initialMain = mainRot;

        // 4D-3D
        e1 = new Vector4(0, 1, 0, 0);
        e2 = new Vector4(0, 0, 1, 0);
        bv = Bivector4.Wedge(e1, e2);
        r = new Rotor4(bv, Random.Range(0, a1));

        e1 = new Vector4(1, 0, 0, 0);
        e2 = new Vector4(0, 0, 1, 0);
        bv = Bivector4.Wedge(e1, e2);
        r *= new Rotor4(bv, Random.Range(0, a2));

        e1 = new Vector4(1, 0, 0, 0);
        e2 = new Vector4(0, 1, 0, 0);
        bv = Bivector4.Wedge(e1, e2);
        r *= new Rotor4(bv, Random.Range(0, a3));

        miniRot *= r;
        miniRot.Normalise();
        SetMiniObjectRotation();
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
        initialMatch = matchRot;
    }

    // Set continuous rotation of the main object
    private void TimedRotor(bool[] rotations)
    {
        Vector4 e1;
        Vector4 e2;
        Bivector4 bv;
        float a = 0;
        if (Timer.running)
            a = Time.deltaTime;


        // 3D
        if (rotations[0])
        {
            e1 = new Vector4(0, 1, 0, 0);
            e2 = new Vector4(0, 0, 1, 0);
            bv = Bivector4.Wedge(e1, e2);
            mainRot *= new Rotor4(bv, a);
        }
        if (rotations[1])
        {
            e1 = new Vector4(1, 0, 0, 0);
            e2 = new Vector4(0, 0, 1, 0);
            bv = Bivector4.Wedge(e1, e2);
            mainRot *= new Rotor4(bv, a);
        }
        if (rotations[2])
        {
            e1 = new Vector4(1, 0, 0, 0);
            e2 = new Vector4(0, 1, 0, 0);
            bv = Bivector4.Wedge(e1, e2);
            mainRot *= new Rotor4(bv, a);
        }

        // 4D
        if (rotations[3])
        {
            e1 = new Vector4(1, 0, 0, 0);
            e2 = new Vector4(0, 0, 0, 1);
            bv = Bivector4.Wedge(e1, e2);
            mainRot *= new Rotor4(bv, a);

            if (UIController.Four_to_Three)
            {
                e1 = new Vector4(0, 1, 0, 0);
                e2 = new Vector4(0, 0, 1, 0);
                bv = Bivector4.Wedge(e1, e2);

                miniRot *= new Rotor4(bv, a);
            }
        }
        if (rotations[4])
        {
            e1 = new Vector4(0, 1, 0, 0);
            e2 = new Vector4(0, 0, 0, 1);
            bv = Bivector4.Wedge(e1, e2);
            mainRot *= new Rotor4(bv, a);

            if (UIController.Four_to_Three)
            {
                e1 = new Vector4(1, 0, 0, 0);
                e2 = new Vector4(0, 0, 1, 0);
                bv = Bivector4.Wedge(e1, e2);

                miniRot *= new Rotor4(bv, a);
            }
        }
        if (rotations[5])
        {
            e1 = new Vector4(0, 0, 1, 0);
            e2 = new Vector4(0, 0, 0, 1);
            bv = Bivector4.Wedge(e1, e2);
            mainRot *= new Rotor4(bv, a);

            if (UIController.Four_to_Three)
            {
                e1 = new Vector4(1, 0, 0, 0);
                e2 = new Vector4(0, 1, 0, 0);
                bv = Bivector4.Wedge(e1, e2);

                miniRot *= new Rotor4(bv, a);
            }
        }

        mainRot.Normalise();
        SetMainObjectRotation();

        if (UIController.Four_to_Three)
        {
            miniRot.Normalise();
            SetMiniObjectRotation();
        }
    }

    // Increment w
    public void IncWCount()
    {
        wCount++;
    }
}