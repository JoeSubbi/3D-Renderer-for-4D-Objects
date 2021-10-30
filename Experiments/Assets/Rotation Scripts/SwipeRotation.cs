using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeRotation : MonoBehaviour
{
    private Renderer rend;
    public Rotor3 total = new Rotor3(1, 0, 0, 0);
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

        rend.material.SetFloat("_YZ", total.b12);
        rend.material.SetFloat("_XZ", total.b01);
        rend.material.SetFloat("_XY", total.b02);
        
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
                //xy rotation
                if (Mathf.Abs(x) > Mathf.Abs(2 * y))
                {
                    Bivector3 bv = Bivector3.Wedge(new Vector3(1, 0, 0), new Vector3(0, 1, 0));
                    Rotor3 r = new Rotor3(bv, x);
                    total *= r;
                }

                //xz rotation
                if (Mathf.Abs(y) > Mathf.Abs(2 * x))
                {
                    Bivector3 bv = Bivector3.Wedge(new Vector3(1, 0, 0), new Vector3(0, 0, 1));
                    Rotor3 r = new Rotor3(bv, y);
                    total *= r;
                }
            }
        }
    }
}
