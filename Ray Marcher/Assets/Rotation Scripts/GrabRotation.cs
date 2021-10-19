using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabRotation : MonoBehaviour
{
    private Vector3 plane = new Vector3(0, 0, 0);
    private Vector3 normal = new Vector3(0, 0, 0);
    public Quaternion total;
    private Renderer image;

    // Start is called before the first frame update
    void Start()
    {
        image = GameObject.Find("Plane").GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        image.material.SetFloat("_X",  total.x);
        image.material.SetFloat("_Y",  total.y);
        image.material.SetFloat("_Z",  total.z);
        image.material.SetFloat("_Q3", total.w);
        
        total.Normalize();
    }

    void OnGUI()
    {
        Event e = Event.current;
        if (e.isMouse)
        {
            float x = Mathf.Round(e.delta.x * 100) / 100;
            float y = Mathf.Round(e.delta.y * 100) / 100;
            int speed = 3;

            if (plane.magnitude == 0 && Input.GetMouseButtonDown(0))
            {
                CheckHit();
            }
            else if (plane.magnitude != 0)
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
                total *= Quaternion.AngleAxis(-x * speed, plane);
                //total *= Quaternion.AngleAxis(-y * speed, plane);
                transform.rotation = total;
            }
            if (Input.GetMouseButtonUp(0))
            {
                plane.x = 0;
                plane.y = 0;
                plane.z = 0;
            }
        }
    }
    
    private void CheckHit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            switch (hit.collider.name)
            {
                case "arc xy":
                    plane.z = 1;
                    Debug.Log("XY");
                    break;
                case "arc zx":
                    plane.y = 1;
                    Debug.Log("ZX");
                    break;
                case "arc yz":
                    plane.x = 1;
                    Debug.Log("YZ");
                    break;
                default:
                    plane.x = 0;
                    plane.y = 0;
                    plane.z = 0;
                    break;
            }
        }
        normal = hit.normal;
    }
}