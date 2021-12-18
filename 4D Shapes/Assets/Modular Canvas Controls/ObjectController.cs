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

    private Renderer mainRenderer;
    private Renderer miniRenderer;
    private Renderer matchRenderer;

    // Object Controls
    public int shape;
    public int effect;
    public Slider wSlider;

    // Rotors for the main object and randomly posed object
    public Rotor4 mainRot  = new Rotor4(1, 0, 0, 0, 0, 0, 0, 0);
    public Rotor4 matchRot = new Rotor4(1, 0, 0, 0, 0, 0, 0, 0);
    public Rotor4 miniRot  = new Rotor4(1, 0, 0, 0, 0, 0, 0, 0);

    // Rotor construction
    private Vector4 e1 = new Vector4(0, 0, 0, 0);
    private Vector4 e2 = new Vector4(0, 0, 0, 0);
    private Bivector4 bv;
    private Rotor4 r;

    // User Interaction
    private int speed = 50;
    private float x;
    private float y;
    private bool go = false;

    // Start is called before the first frame update
    void Start()
    {
        mainRenderer = main.GetComponent<Renderer>();
        miniRenderer = mini.GetComponent<Renderer>();
        matchRenderer = match.GetComponent<Renderer>();

        SetRandObjectRotation();
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

        // Set object rotation
        // Main Object Rotation

        // 3D Component into matchRenderer

        // 4D Component as 3D Component into miniRenderer
        SetMainObjectRotation();

    }

    public void SetMainObjectRotation()
    {
        mainRenderer.material.SetFloat("_A", mainRot.a);
        mainRenderer.material.SetFloat("_YZ", mainRot.byz);
        mainRenderer.material.SetFloat("_XZ", mainRot.bxz);
        mainRenderer.material.SetFloat("_XY", mainRot.bxy);
        mainRenderer.material.SetFloat("_XW", mainRot.bxw);
        mainRenderer.material.SetFloat("_YW", mainRot.byw);
        mainRenderer.material.SetFloat("_ZW", mainRot.bzw);
        mainRenderer.material.SetFloat("_XYZW", mainRot.bxyzw);
    }

    public void SetMatchObjectRotation()
    {
        matchRenderer.material.SetFloat("_A", matchRot.a);
        matchRenderer.material.SetFloat("_YZ", matchRot.byz);
        matchRenderer.material.SetFloat("_XZ", matchRot.bxz);
        matchRenderer.material.SetFloat("_XY", matchRot.bxy);
        matchRenderer.material.SetFloat("_XW", matchRot.bxw);
        matchRenderer.material.SetFloat("_YW", matchRot.byw);
        matchRenderer.material.SetFloat("_ZW", matchRot.bzw);
        matchRenderer.material.SetFloat("_XYZW", matchRot.bxyzw);
    }

    public void SetRandObjectRotation()
    {
        e1 = new Vector4(1, 0, 0, 0);
        e2 = new Vector4(0, 0, 0, 1);
        bv = Bivector4.Wedge(e1, e2);
        r = new Rotor4(bv, Random.Range(0, 3.14159f));
        
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

    public void SetMiniObjectRotation()
    {
        miniRenderer.material.SetFloat("_A", miniRot.a);
        miniRenderer.material.SetFloat("_YZ", miniRot.byz);
        miniRenderer.material.SetFloat("_XZ", miniRot.bxz);
        miniRenderer.material.SetFloat("_XY", miniRot.bxy);
        miniRenderer.material.SetFloat("_XW", miniRot.bxw);
        miniRenderer.material.SetFloat("_YW", miniRot.byw);
        miniRenderer.material.SetFloat("_ZW", miniRot.bzw);
        miniRenderer.material.SetFloat("_XYZW", miniRot.bxyzw);
    }

    void OnGUI()
    {
        Event e = Event.current;
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            go = EventSystem.current.currentSelectedGameObject.name != wSlider.name;
        }
        else go = true;

        if (e.isMouse && go)
        {
            // Get Mouse Movement
            x = e.delta.x / speed;
            y = e.delta.y / speed;

            // 3D Rotation - Left Click
            if (Input.GetMouseButton(0))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    // Rotate with a circular gesture around global XY plane
                    if (Input.mousePosition.y < Screen.height / 2) x *= -0.5f;
                    if (Input.mousePosition.x < Screen.width / 2) y *= -0.5f;

                    e1 = new Vector4(-1, 0, 0, 0);
                    e2 = new Vector4(0, 1, 0, 0);
                    bv = Bivector4.Wedge(e1, e2);
                    r = new Rotor4(bv, x + y);

                    mainRot /= r;
                    matchRot /= r;
                }
                else
                {
                    // Rotate with vertical gesture around global YZ plane
                    e1 = new Vector4(0, -1, 0, 0);
                    e2 = new Vector4(0, 0, 1, 0);
                    bv = Bivector4.Wedge(e1, e2);
                    r = new Rotor4(bv, y);

                    mainRot /= r;
                    matchRot /= r;

                    // Rotate with horizontal gesture around global XZ plane
                    e1 = new Vector4(-1, 0, 0, 0);
                    e2 = new Vector4(0, 0, 1, 0);
                    bv = Bivector4.Wedge(e1, e2);
                    r = new Rotor4(bv, x);

                    mainRot /= r;
                    matchRot /= r;
                }
            }

            // 4D Rotation - Right Click
            else if (Input.GetMouseButton(1))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    // Rotate with a circular gesture around global zw plane
                    if (Input.mousePosition.y < Screen.height / 2) x *= -0.5f;
                    if (Input.mousePosition.x < Screen.width / 2) y *= -0.5f;

                    e1 = new Vector4(0, 0, 1, 0);
                    e2 = new Vector4(0, 0, 0, 1);
                    bv = Bivector4.Wedge(e1, e2);
                    r = new Rotor4(bv, x + y);

                    mainRot /= r;

                    // 3D Component
                    if (UIController.Four_to_Three)
                    {
                        e1 = new Vector4(1, 0, 0, 0);
                        e2 = new Vector4(0, 1, 0, 0);
                        bv = Bivector4.Wedge(e1, e2);
                        r = new Rotor4(bv, x + y);

                        miniRot /= r;
                    }
                }
                else
                {
                    // Rotate with vertical gesture around global yw plane
                    e1 = new Vector4(0, 1, 0, 0);
                    e2 = new Vector4(0, 0, 0, 1);
                    bv = Bivector4.Wedge(e1, e2);
                    r = new Rotor4(bv, y);

                    mainRot /= r;

                    // Rotate with horizontal gesture around global xw plane
                    e1 = new Vector4(-1, 0, 0, 0);
                    e2 = new Vector4(0, 0, 0, 1);
                    bv = Bivector4.Wedge(e1, e2);
                    r = new Rotor4(bv, x);

                    mainRot /= r;

                    // 3D Component
                    if (UIController.Four_to_Three)
                    {
                        e1 = new Vector4(1, 0, 0, 0);
                        e2 = new Vector4(0, 0, 1, 0);
                        bv = Bivector4.Wedge(e1, e2);
                        r = new Rotor4(bv, y);

                        miniRot /= r;

                        e1 = new Vector4(0, 1, 0, 0);
                        e2 = new Vector4(0, 0, 1, 0);
                        bv = Bivector4.Wedge(e1, e2);
                        r = new Rotor4(bv, x);

                        miniRot /= r;
                    }
                }
            }
            mainRot.Normalise();
            matchRot.Normalise();

            SetMainObjectRotation();
            SetMatchObjectRotation();

            if (UIController.Four_to_Three)
            {
                miniRot.Normalise();
                SetMiniObjectRotation();
            }
        }
    }
}