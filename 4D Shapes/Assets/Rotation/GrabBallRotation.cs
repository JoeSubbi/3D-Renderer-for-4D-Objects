using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabBallRotation : MonoBehaviour
{
    // User Interaction
    private int speed = 50;
    private float x;
    private float y;
    public bool onUIElement;

    // Rotor construction
    private Vector4 e1 = new Vector4(0, 0, 0, 0);
    private Vector4 e2 = new Vector4(0, 0, 0, 0);
    private Rotor4 r;
    private Bivector4 bv;

    // Rotor construction for 3D counterpart
    private Vector4 e3 = new Vector4(0, 0, 0, 0);
    private Vector4 e4 = new Vector4(0, 0, 0, 0);

    // Grab Ball rotation
    private Vector3 axis = new Vector3(0, 0, 0);
    private Quaternion qTotal = new Quaternion(1, 0, 0, 0);
    // rotation guide line
    private LineRenderer line;

    // Ray information
    private Vector3 point = new Vector3(0, 0, 0);  // Point of contact
    private Vector3 normal = new Vector3(0, 0, 0); // Normal of surface at point
    private Vector3 tangent = new Vector3(0, 0, 0);// Tangent to surface at point

    public bool wRotation = false;
    public GameObject icon;

    // Start is called before the first frame update
    void Start()
    {
        //Guide line when rotating grab ball
        line = gameObject.AddComponent<LineRenderer>();
        Vector3[] initLinePos = new Vector3[2] { Vector3.zero, Vector3.zero };
        line.SetPositions(initLinePos);
        line.startWidth = 0.01f;
        line.endWidth = 0.01f;

        icon.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            wRotation = !wRotation;
        }

        // Colour the grab ball for 3D or 4D rotation
        if (!wRotation)
        {
            GameObject.Find("arc xy").GetComponent<Renderer>().material.SetColor("_Colour", new Vector4(0.2f, 0.44f, 1f, 1f));
            GameObject.Find("arc yz").GetComponent<Renderer>().material.SetColor("_Colour", new Vector4(1f, 0.4f, 0.4f, 1f));
            GameObject.Find("arc xz").GetComponent<Renderer>().material.SetColor("_Colour", new Vector4(0.5f, 1f, 0.5f, 1f));
        }
        else
        {
            GameObject.Find("arc xy").GetComponent<Renderer>().material.SetColor("_Colour", new Vector4(0.2f, 1f, 0.95f, 1f));
            GameObject.Find("arc yz").GetComponent<Renderer>().material.SetColor("_Colour", new Vector4(1f, 0.4f, 0.9f, 1f));
            GameObject.Find("arc xz").GetComponent<Renderer>().material.SetColor("_Colour", new Vector4(1f, 0.95f, 0.5f, 1f));
        }
    }

    void OnGUI()
    {
        Event e = Event.current;
        if (e.isMouse)
        {
            // Get Mouse Movements
            x = e.delta.x / speed;
            y = e.delta.y / speed;

            // See what axis the object should rotate on
            if (e1 == e2 && Input.GetMouseButtonDown(0))
            {
                CheckHit();
            }
            // If the bivector is > 0 we are conducting rotation
            else if (e1 != e2)
            {
                // The normal will perpendicular to the tangent of the arc
                // The axis of rotation will be perpendicular to the plane of rotation
                // Both of these are perpendicular to the tangent
                tangent = Vector3.Cross(qTotal * axis, normal);

                tangent.Normalize();
                x *= tangent.x;
                y *= tangent.y;

                // Guide Line
                line.enabled = true;
                line.SetPosition(0, point - tangent * 5);
                line.SetPosition(1, point + tangent * 5);

                //Rotate object
                bv = Bivector4.Wedge(e1, e2);
                r = new Rotor4(bv, x + y);
                ObjectController.mainRot = ObjectController.mainRot * r;
                ObjectController.mainRot.Normalise();
                ObjectController.SetMainObjectRotation();

                if (!wRotation)
                {
                    //Rotate grab ball
                    qTotal *= Quaternion.AngleAxis(RadToDeg(x + y), axis);
                    transform.rotation = qTotal;

                    //Move pose match object
                    if (UIController.Pose_Match)
                    {
                        ObjectController.matchRot = ObjectController.matchRot * r;
                        ObjectController.matchRot.Normalise();
                        ObjectController.SetMatchObjectRotation();
                    }
                }
                else
                {
                    // Rotate 3D counterpart
                    if (UIController.Four_to_Three)
                    {
                        bv = Bivector4.Wedge(e3, e4);
                        r = new Rotor4(bv, x + y);
                        ObjectController.miniRot = ObjectController.miniRot * r;
                        ObjectController.miniRot.Normalise();
                        ObjectController.SetMiniObjectRotation();
                    }
                }
                
            }

            //If not dragging mouse, disbale specified plane of rotation
            if (Input.GetMouseButtonUp(0))
            {
                e1 = new Vector4(0, 0, 0, 0);
                e2 = new Vector4(0, 0, 0, 0);
                axis = new Vector3(0, 0, 0);
                line.enabled = false;
            }
        }
    }

    private void CheckHit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (!wRotation)
            {
                switch (hit.collider.name)
                {
                    case "arc xy":
                        e1 = new Vector4(-1, 0, 0, 0);
                        e2 = new Vector4(0, 1, 0, 0);

                        axis = new Vector3(0, 0, 1);
                        line.material = GameObject.Find("arc xy").GetComponent<Renderer>().material;
                        break;
                    case "arc xz":
                        e1 = new Vector4(-1, 0, 0, 0);
                        e2 = new Vector4(0, 0, 1, 0);

                        axis = new Vector3(0, 1, 0);
                        line.material = GameObject.Find("arc xz").GetComponent<Renderer>().material;
                        break;
                    case "arc yz":
                        e1 = new Vector4(0,-1, 0, 0);
                        e2 = new Vector4(0, 0, 1, 0);

                        axis = new Vector3(-1, 0, 0);
                        line.material = GameObject.Find("arc yz").GetComponent<Renderer>().material;
                        break;
                    default:
                        e1 = new Vector4(0, 0, 0, 0);
                        e2 = new Vector4(0, 0, 0, 0);

                        axis = new Vector3(0, 0, 0);
                        break;
                }
            }
            else
            {
                switch (hit.collider.name)
                {
                    case "arc xy":
                        e1 = new Vector4(0, 0, 1, 0);
                        e2 = new Vector4(0, 0, 0, 1);

                        e3 = new Vector4(1, 0, 0, 0);
                        e4 = new Vector4(0, 1, 0, 0);

                        axis = new Vector3(0, 0, 1);
                        line.material = GameObject.Find("arc xy").GetComponent<Renderer>().material;
                        break;
                    case "arc xz":
                        e1 = new Vector4(0, 1, 0, 0);
                        e2 = new Vector4(0, 0, 0, 1);

                        e3 = new Vector4(1, 0, 0, 0);
                        e4 = new Vector4(0, 0, 1, 0);

                        axis = new Vector3(0, 1, 0);
                        line.material = GameObject.Find("arc xz").GetComponent<Renderer>().material;
                        break;
                    case "arc yz":
                        e1 = new Vector4(1, 0, 0, 0);
                        e2 = new Vector4(0, 0, 0, 1);

                        e3 = new Vector4(0, 1, 0, 0);
                        e4 = new Vector4(0, 0, 1, 0);

                        axis = new Vector3(1, 0, 0);
                        line.material = GameObject.Find("arc yz").GetComponent<Renderer>().material;
                        break;
                    default:
                        e1 = new Vector4(0, 0, 0, 0);
                        e2 = new Vector4(0, 0, 0, 0);

                        e3 = new Vector4(0, 0, 0, 0);
                        e4 = new Vector4(0, 0, 0, 0);

                        axis = new Vector3(0, 0, 0);
                        break;
                }
            }
        }
        normal = hit.normal;
        point = hit.point;
    }

    private static float RadToDeg(float a)
    {
        return (a / (2 * 3.14159f)) * 360;
    }
}
