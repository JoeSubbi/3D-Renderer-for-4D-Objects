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

    public GameObject icon;

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            icon.SetActive(true);
        }
        else icon.SetActive(false);
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
        if (e.isMouse && !onUIElement)
        {
            // Get Mouse Movement
            x = e.delta.x / speed;
            y = e.delta.y / speed;

            // 3D Rotation - Left Click
            if (Input.GetMouseButton(0))
            {
                if (Input.GetKey(KeyCode.Space))
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
                if (Input.GetKey(KeyCode.Space))
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
                        e1 = new Vector4(-1, 0, 0, 0);
                        e2 = new Vector4( 0, 1, 0, 0);
                        bv = Bivector4.Wedge(e1, e2);
                        r  = new Rotor4(bv, x + y);

                        ObjectController.miniRot *= r;
                    }
                }
                else
                {
                    // Rotate with vertical gesture around global yw plane
                    e1 = new Vector4(0, 1, 0, 0);
                    e2 = new Vector4(0, 0, 0, 1);
                    bv = Bivector4.Wedge(e1, e2);
                    r  = new Rotor4(bv, y);

                    // Rotate with horizontal gesture around global xw plane
                    e1 = new Vector4(-1, 0, 0, 0);
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
