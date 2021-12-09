using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabRotation : MonoBehaviour
{
    private Vector4 e1 = new Vector4(0, 0, 0, 0);
    private Vector4 e2 = new Vector4(0, 0, 0, 0);
    public Rotor4 total = new Rotor4(1, 0, 0, 0, 0, 0, 0, 0);

    private Vector3 plane = new Vector3(0, 0, 0);
    private Quaternion qTotal = new Quaternion(1, 0, 0, 0);
    private LineRenderer line;
    private Renderer image;

    public bool wRotation = false;
    private Vector3 normal = new Vector3(0, 0, 0);
    private Vector3 point = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        //4D Object
        image = GameObject.Find("Plane").GetComponent<Renderer>();

        //Guide line when rotating grab ball
        line = gameObject.AddComponent<LineRenderer>();
        Vector3[] initLinePos = new Vector3[2] { Vector3.zero, Vector3.zero };
        line.SetPositions(initLinePos);
        line.startWidth = 0.01f;
        line.endWidth = 0.01f;

    }

    // Update is called once per frame
    void Update()
    {
        //Rotate 4D Object
        image.material.SetFloat("_A",  total.a);
        image.material.SetFloat("_XY", total.bxy);
        image.material.SetFloat("_XZ", total.bxz);
        image.material.SetFloat("_YZ", total.byz);
        image.material.SetFloat("_XW", total.bxw);
        image.material.SetFloat("_YW", total.byw);
        image.material.SetFloat("_ZW", total.bzw);
        image.material.SetFloat("_XYZW", total.bxyzw);

        total.Normalise();
        qTotal.Normalize();

        if (Input.GetKeyDown("space")){
            wRotation = !wRotation;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            total = new Rotor4(1, 0, 0, 0, 0, 0, 0, 0);
            qTotal = new Quaternion(1, 0, 0, 0);
            transform.rotation = qTotal;
        }

        // Colour the grab ball for 3D or 4D rotation
        if (!wRotation)
        {
            GameObject.Find("arc xy").GetComponent<Renderer>().material.SetColor("_Colour", new Vector4(0.2f, 0.44f, 1f, 1f));
            GameObject.Find("arc yz").GetComponent<Renderer>().material.SetColor("_Colour", new Vector4(1f, 0.4f, 0.4f, 1f));
            GameObject.Find("arc zx").GetComponent<Renderer>().material.SetColor("_Colour", new Vector4(0.5f, 1f, 0.5f, 1f));
        }
        else
        {
            GameObject.Find("arc xy").GetComponent<Renderer>().material.SetColor("_Colour", new Vector4(0.2f, 1f, 0.95f, 1f));
            GameObject.Find("arc yz").GetComponent<Renderer>().material.SetColor("_Colour", new Vector4(1f, 0.4f, 0.9f, 1f));
            GameObject.Find("arc zx").GetComponent<Renderer>().material.SetColor("_Colour", new Vector4(1f, 0.95f, 0.5f, 1f));
        }
    }
    
    void OnGUI()
    {
        Event e = Event.current;
        if (e.isMouse)
        {
            //Get Mouse Movements
            int speed = 50;
            float x = e.delta.x / speed;
            float y = e.delta.y / speed;

            //See what axis the object should rotate on
            if (e1 == e2 && Input.GetMouseButtonDown(0))
            {
                CheckHit();
            }
            else if (e1 != e2)
            {
                // Define direction for intuitive rotation with grab ball
                Vector3 perp = qTotal * plane;
                if (perp.x == 1) normal.x = 0;
                if (perp.y == 1) normal.z = 0;
                if (perp.z == 1) normal.y = 0;
                normal.Normalize();

                Vector3 tangent = Vector3.Cross(perp, normal);
                if (tangent.magnitude * perp.magnitude == Vector3.Dot(tangent, perp)) tangent = Quaternion.AngleAxis(90, normal) * tangent;

                tangent.Normalize();
                x *= -tangent.x;
                y *= -tangent.y;

                // Guide Line
                line.enabled = true;

                line.SetPosition(0, point-tangent*5);
                line.SetPosition(1, point+tangent*5);

                //Rotate object
                Bivector4 bv = Bivector4.Wedge(e1, e2);
                Rotor4 r = new Rotor4(bv, x+y);
                total *= r;

                //Rotate grab ball
                if (!wRotation)
                {
                    qTotal *= Quaternion.AngleAxis(RadToDeg(x + y), plane);
                    transform.rotation = qTotal;
                }
            }
            //If not dragging mouse, disbale specified plane of rotation
            if (Input.GetMouseButtonUp(0))
            {
                e1 = new Vector4(0, 0, 0, 0);
                e2 = new Vector4(0, 0, 0, 0);

                plane = new Vector3(0, 0, 0);

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
                        e1 = new Vector4(1, 0, 0, 0);
                        e2 = new Vector4(0, 1, 0, 0);

                        plane = new Vector3(0, 0, 1);
                        line.material = GameObject.Find("arc xy").GetComponent<Renderer>().material;
                        break;
                    case "arc zx":
                        e1 = new Vector4(1, 0, 0, 0);
                        e2 = new Vector4(0, 0, 1, 0);

                        plane = new Vector3(0, 1, 0);
                        line.material = GameObject.Find("arc zx").GetComponent<Renderer>().material;
                        break;
                    case "arc yz":
                        e1 = new Vector4(0,-1, 0, 0);
                        e2 = new Vector4(0, 0, 1, 0);

                        plane = new Vector3(1, 0, 0);
                        line.material = GameObject.Find("arc yz").GetComponent<Renderer>().material;
                        break;
                    default:
                        e1 = new Vector4(0, 0, 0, 0);
                        e2 = new Vector4(0, 0, 0, 0);

                        plane = new Vector3(0, 0, 0);
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

                        plane = new Vector3(0, 0, 1);
                        line.material = GameObject.Find("arc xy").GetComponent<Renderer>().material;
                        break;
                    case "arc zx":
                        e1 = new Vector4(0, 1, 0, 0);
                        e2 = new Vector4(0, 0, 0, 1);

                        plane = new Vector3(0, 1, 0);
                        line.material = GameObject.Find("arc zx").GetComponent<Renderer>().material;
                        break;
                    case "arc yz":
                        e1 = new Vector4(1, 0, 0, 0);
                        e2 = new Vector4(0, 0, 0, 1);

                        plane = new Vector3(1, 0, 0);
                        line.material = GameObject.Find("arc yz").GetComponent<Renderer>().material;
                        break;
                    default:
                        e1 = new Vector4(0, 0, 0, 0);
                        e2 = new Vector4(0, 0, 0, 0);

                        plane = new Vector3(0, 0, 0);
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