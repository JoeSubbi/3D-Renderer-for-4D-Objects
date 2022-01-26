using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeRotation : MonoBehaviour
{
    // User Interaction
    private int speed = 50;
    private float x;
    private float y;
    public  bool onUIElement = false;

    // Rotor construction
    private Vector4 e1 = new Vector4(0, 0, 0, 0);
    private Vector4 e2 = new Vector4(0, 0, 0, 0);
    private Rotor4 r;
    private Bivector4 bv;

    // UX
    public GameObject icon;
    private bool x_only = false;
    private bool y_only = false;
    private bool z = false;

    /*
    // rotation guide line
    private LineRenderer line;

    void Awake()
    {
        //Guide line when rotating grab ball
        line = gameObject.AddComponent<LineRenderer>();
        Vector3[] initLinePos = new Vector3[2] { Vector3.zero, Vector3.zero };
        line.SetPositions(initLinePos);
        line.startWidth = 0.01f;
        line.endWidth = 0.01f;
    }
    */

    void Update()
    {
        x_only = Input.GetKey("x");
        y_only = Input.GetKey("y");

        z = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey("z"));
        icon.SetActive(z);

    }

    // Update rotor based on mouse manipulation
    void OnGUI()
    {        
        // Check if user is over the slider UI element
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            onUIElement = EventSystem.current.currentSelectedGameObject.name == "HyperPlanePosition";
        }
        else onUIElement = false;

        Event e = Event.current;
        if (e.isMouse && !onUIElement && !UIController.Rotation_Match)
        {

            x = (e.delta.x / speed);
            y = (e.delta.y / speed);

            // 3D Rotation - Left Click
            if (Input.GetMouseButton(0))
            {
                if (z)
                {
                    // Rotate with a circular gesture around global XY plane
                    if (Input.mousePosition.y < Screen.height / 2) x *= -0.5f;
                    if (Input.mousePosition.x < Screen.width  / 2) y *= -0.5f;

                    e1 = new Vector4(-1, 0, 0, 0);
                    e2 = new Vector4( 0, 1, 0, 0);
                    bv = Bivector4.Wedge(e1, e2);
                    r  = new Rotor4(bv, x + y);
                }
                else
                {
                    // Get Mouse Movement
                    if (y_only)
                        y = 0;
                    if (x_only)
                        x = 0;

                    // Rotate with vertical gesture around global YZ plane
                    e1 = new Vector4(0,-1, 0, 0);
                    e2 = new Vector4(0, 0, 1, 0);
                    bv = Bivector4.Wedge(e1, e2);
                    r  = new Rotor4(bv, y);

                    // Rotate with horizontal gesture around global XZ plane
                    e1 = new Vector4(-1, 0, 0, 0);
                    e2 = new Vector4(0, 0, 1, 0);
                    bv = Bivector4.Wedge(e1, e2);
                    r *= new Rotor4(bv, x);
                }
                ObjectController.mainRot *= r;

                if (UIController.Pose_Match)
                    ObjectController.matchRot *= r;
            }

            // 4D Rotation - Right Click
            else if (Input.GetMouseButton(1))
            {
                if (z)
                {
                    // Rotate with a circular gesture around global zw plane
                    if (Input.mousePosition.y < Screen.height / 2) x *= -0.5f;
                    if (Input.mousePosition.x < Screen.width  / 2) y *= -0.5f;

                    e1 = new Vector4(0, 0, 1, 0);
                    e2 = new Vector4(0, 0, 0, 1);
                    bv = Bivector4.Wedge(e1, e2);
                    r  = new Rotor4(bv, x + y);

                    ObjectController.mainRot *= r;

                    // 3D Component
                    if (UIController.Four_to_Three)
                    {
                        e1 = new Vector4( 1, 0, 0, 0);
                        e2 = new Vector4( 0, 1, 0, 0);
                        bv = Bivector4.Wedge(e1, e2);
                        r  = new Rotor4(bv, x + y);

                        ObjectController.miniRot *= r;
                    }
                }
                else
                {
                    // Get Mouse Movement
                    if (y_only)
                        x = 0;
                    if (x_only)
                        y = 0;

                    // Rotate with vertical gesture around global yw plane
                    e1 = new Vector4(0, 1, 0, 0);
                    e2 = new Vector4(0, 0, 0, 1);
                    bv = Bivector4.Wedge(e1, e2);
                    r  = new Rotor4(bv, y);

                    // Rotate with horizontal gesture around global xw plane
                    e1 = new Vector4(1, 0, 0, 0);
                    e2 = new Vector4(0, 0, 0, 1);
                    bv = Bivector4.Wedge(e1, e2);
                    r *= new Rotor4(bv, x);

                    ObjectController.mainRot *= r;

                    // 3D Component
                    if (UIController.Four_to_Three)
                    {
                        e1 = new Vector4(1, 0, 0, 0);
                        e2 = new Vector4(0, 0, 1, 0);
                        bv = Bivector4.Wedge(e1, e2);
                        r  = new Rotor4(bv, y);

                        e1 = new Vector4(0, 1, 0, 0);
                        e2 = new Vector4(0, 0, 1, 0);
                        bv = Bivector4.Wedge(e1, e2);
                        r *= new Rotor4(bv, x);

                        ObjectController.miniRot *= r;
                    }
                }
            }
            ObjectController.mainRot.Normalise();
            ObjectController.SetMainObjectRotation();

            if (UIController.Four_to_Three)
            {
                ObjectController.miniRot.Normalise();
                ObjectController.SetMiniObjectRotation();
            }
            if (UIController.Pose_Match)
            {
                ObjectController.matchRot.Normalise();
                ObjectController.SetMatchObjectRotation();
            }
        }
    }

}
