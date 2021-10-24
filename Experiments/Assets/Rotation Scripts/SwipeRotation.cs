using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeRotation : MonoBehaviour
{
    private Renderer rend;
    public Rotor4 total = new Rotor4();

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {        
        rend.material.SetFloat("_S",  total.s);

        rend.material.SetFloat("_ZY", total.bzy);
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
            if (Input.GetMouseButton(0))
            {
                Rotor4 local = new Rotor4();
                //x to y rotation
                local = Rotor4.GeometricProduct(new Vector4(1, 0, 0, 0), new Vector4(0, 1, 0, 0));
                total *= local; 

                Debug.Log("local: "+local.ToString());
                Debug.Log("total: "+total.ToString());
            }
        }
    }
}
