using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabRotation : MonoBehaviour
{
    private Vector3 e1 = new Vector3(0, 0, 0);
    private Vector3 e2 = new Vector3(0, 0, 0);
    public Rotor3 total = new Rotor3(1, 0, 0, 0);

    private Vector3 plane = new Vector3(0, 0, 0);
    private Quaternion qTotal;

    private Vector3 normal = new Vector3(0, 0, 0);
    private Renderer image;

    // Start is called before the first frame update
    void Start()
    {
        image = GameObject.Find("Plane").GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {

        image.material.SetFloat("_A",  total.a);
        image.material.SetFloat("_XY", total.b02);
        image.material.SetFloat("_XZ", total.b01);
        image.material.SetFloat("_YZ", total.b12);
        
        total.Normalise();
        qTotal.Normalize();
    }

    
    void OnGUI()
    {
        Event e = Event.current;
        if (e.isMouse)
        {

            int speed = 50;
            float x = e.delta.x / speed; //Mathf.Round(e.delta.x * 100) / 100;
            //float y = //Mathf.Round(e.delta.y * 100) / 100;

            if (e1 == e2 && Input.GetMouseButtonDown(0))
            {
                CheckHit();
            }
            else if (e1 != e2)
            {
                /*
                Vector3 t1 = ( Vector3.Cross(normal, Vector3.forward).magnitude > Vector3.Cross(normal, Vector3.up).magnitude ) ? Vector3.Cross(normal, Vector3.forward) : Vector3.Cross(normal, Vector3.up);
                Vector2 mp = Input.mousePosition;

                if (plane.z == 1)
                {
                    x *= t1.x;
                    y *= t1.y;
                }
                else if (plane.y == 1)
                {
                    x *= t1.z;
                    y *= t1.x;
                }
                else if (plane.x == 1)
                {
                    x *= t1.z;
                    y *= t1.y;
                }*/
    
                Bivector3 bv = Bivector3.Wedge(e1, e2);
                Rotor3 r = new Rotor3(bv, x);
                total *= r;

                qTotal *= Quaternion.AngleAxis(-RadToDeg(x), plane);
                transform.rotation = qTotal;
            }
            if (Input.GetMouseButtonUp(0))
            {
                e1 = new Vector3(0, 0, 0);
                e2 = new Vector3(0, 0, 0);

                plane = new Vector3(0, 0, 0);
            }
        }
    }

    /*
    void OnGUI()
    {
        Event e = Event.current;
        if (e.isMouse)
        {
            int speed = 50;
            float x = -e.delta.x / speed;
            float y = -e.delta.y / speed;

            if (Input.GetMouseButton(0))
            {

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    e1 = new Vector3(0, 1, 0);
                    e2 = new Vector3(0, 0, 1);
                    Bivector3 bv = Bivector3.Wedge(e1, e2);
                    Rotor3 r = new Rotor3(bv, x + y);
                    total *= r;
                }
                else
                {
                    //xy rotation
                    if (Mathf.Abs(x) > Mathf.Abs(2 * y))
                    {
                        e1 = new Vector3(1, 0, 0);
                        e2 = new Vector3(0, 1, 0);
                        Bivector3 bv = Bivector3.Wedge(e1, e2);
                        Rotor3 r = new Rotor3(bv, x);
                        total *= r;
                    }

                    //xz rotation
                    if (Mathf.Abs(y) > Mathf.Abs(2 * x))
                    {
                        e1 = new Vector3(1, 0, 0);
                        e2 = new Vector3(0, 0, 1);
                        Bivector3 bv = Bivector3.Wedge(e1, e2);
                        Rotor3 r = new Rotor3(bv, y);
                        total *= r;
                    }
                }
            }
        }
    }
    */

    private void CheckHit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            switch (hit.collider.name)
            {
                case "arc xy":
                    e1 = new Vector3(1, 0, 0);
                    e2 = new Vector3(0, 0, 1);

                    plane = new Vector3(0, 0, 1);
                    break;
                case "arc zx":
                    e1 = new Vector3(-1, 0, 0);
                    e2 = new Vector3( 0, 1, 0);

                    plane = new Vector3(0, 1, 0);
                    break;
                case "arc yz":
                    e1 = new Vector3(0, 1, 0);
                    e2 = new Vector3(0, 0, 1);

                    plane = new Vector3(1, 0, 0);
                    break;
                default:
                    e1 = new Vector3(0, 0, 0);
                    e2 = new Vector3(0, 0, 0);

                    plane = new Vector3(0, 0, 0);
                    break;
            }
        }
        normal = hit.normal;
    }

    private static float RadToDeg(float a)
    {
        return (a / (2 * 3.14159f)) * 360;
    }
}