using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeRotation : MonoBehaviour
{
    private Vector4 e1 = new Vector4(0, 0, 0, 0);
    private Vector4 e2 = new Vector4(0, 0, 0, 0);
    //public Rotor3 total = new Rotor3(1, 0, 0, 0);
    public Rotor4 total = new Rotor4(1, 0, 0, 0, 0, 0, 0);

    private Renderer rend;
    public float speed = 50;


    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {        
        rend.material.SetFloat("_A",  total.a);

        //rend.material.SetFloat("_YZ", total.b12);
        //rend.material.SetFloat("_XZ", total.b02);
        //rend.material.SetFloat("_XY", total.b01);

        rend.material.SetFloat("_YZ", total.byz);
        rend.material.SetFloat("_XZ", total.bxz);
        rend.material.SetFloat("_XY", total.bxy);
        rend.material.SetFloat("_XW", total.bxw);
        rend.material.SetFloat("_YW", total.byw);
        rend.material.SetFloat("_ZW", total.bzw);

        total.Normalise();
    }

    void OnGUI()
    {
        Event e = Event.current;
        if (e.isMouse)
        {
            float x = -e.delta.x / speed;
            float y = -e.delta.y / speed;

            if (Input.GetMouseButton(0))
            {
                
                if (Input.GetKey(KeyCode.LeftShift))
                {

                    e1 = new Vector4(1, 0, 0, 0);
                    e2 = new Vector4(0, 1, 0, 0);
                    Bivector4 bv = Bivector4.Wedge(e1, e2);
                    Rotor4 r = new Rotor4(bv, x+y);
                    total *= r;
                }
                else
                {
                    //xy rotation
                    if (Mathf.Abs(y) > Mathf.Abs(2 * x))
                    {

                        e1 = new Vector4(0, 1, 0, 0);
                        e2 = new Vector4(0, 0, 1, 0);
                        Bivector4 bv = Bivector4.Wedge(e1, e2);
                        Rotor4 r = new Rotor4(bv, y);
                        total *= r;
                    }

                    //xz rotation
                    if (Mathf.Abs(x) > Mathf.Abs(2 * y))
                    {
                        e1 = new Vector4(1, 0, 0, 0);
                        e2 = new Vector4(0, 0, 1, 0);
                        Bivector4 bv = Bivector4.Wedge(e1, e2);
                        Rotor4 r = new Rotor4(bv, x);
                        total *= r;
                    }
                }
            }
        }
    }
}
