using System.Collections;
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
    public Slider wSlider;

    // Rotors for the main object and randomly posed object
    public static Rotor4 mainRot  = new Rotor4(1, 0, 0, 0, 0, 0, 0, 0);
    public static Rotor4 matchRot = new Rotor4(1, 0, 0, 0, 0, 0, 0, 0);
    public static Rotor4 miniRot  = new Rotor4(1, 0, 0, 0, 0, 0, 0, 0);

    public bool grabBall;
    public GameObject GrabBall;
    public GameObject SwipeEmpty;

    // Start is called before the first frame update
    void Start()
    {
        mainRenderer = main.GetComponent<Renderer>();
        miniRenderer = mini.GetComponent<Renderer>();
        matchRenderer = match.GetComponent<Renderer>();

        SetRandObjectRotation();

        SwitchRotation(grabBall);
    }

    // Update is called once per frame
    void Update()
    {
        // Set object W position
        mainRenderer.material.SetFloat("_W", wSlider.value);

        // Set object shape
        mainRenderer.material.SetInt("_Shape", shape);
        matchRenderer.material.SetInt("_Shape", shape);
        // Set 3D Representation to cube - Do not want it interfereing with experiments?
        miniRenderer.material.SetInt("_Shape", 1);
        // Set 3D Representation to main object - nullifies Shape_Match test
        miniRenderer.material.SetInt("_Shape", shape);

        // Set object texture
        mainRenderer.material.SetInt("_Effect", effect);
        matchRenderer.material.SetInt("_Effect", effect);
    }

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

    public static void SetRandObjectRotation()
    {
        Vector4   e1 = new Vector4(1, 0, 0, 0);
        Vector4   e2 = new Vector4(0, 0, 0, 1);
        Bivector4 bv = Bivector4.Wedge(e1, e2);
        Rotor4    r = new Rotor4(bv, Random.Range(0, 3.14159f));
        
        e1 = new Vector4(0, 1, 0, 0);
        e2 = new Vector4(0, 0, 0, 1);
        bv = Bivector4.Wedge(e1, e2);
        r *= new Rotor4(bv, Random.Range(0, 3.14159f));

        e1 = new Vector4(0, 0, 1, 0);
        e2 = new Vector4(0, 0, 0, 1);
        bv = Bivector4.Wedge(e1, e2);
        r *= new Rotor4(bv, Random.Range(0, 3.14159f));

        matchRot *= r;
        
        matchRot.Normalise();
        SetMatchObjectRotation();
    }

    public void SwitchRotation(bool grabBall)
    {
        if (grabBall)
        {
            GrabBall.SetActive(true);
            SwipeEmpty.SetActive(false);
        }
        else
        {
            GrabBall.SetActive(false);
            SwipeEmpty.SetActive(true);
        }
    }
}