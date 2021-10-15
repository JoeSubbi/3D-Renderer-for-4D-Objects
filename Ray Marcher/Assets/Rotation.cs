using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    private Renderer rend;
    private Quaternion total3 = new Quaternion(0, 0, 0, 1);
    private Quaternion total4 = new Quaternion(0, 0, 0, 1);

    //extra controls
    private bool fdr = false;
    private bool roll = false;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //4D Rotation
        if (Input.GetMouseButton(1) || Input.GetKey("w"))
            fdr = true;
        else
            fdr = false;
        //Rolling along Z
        if (Input.GetKey("z"))
            roll = true;
        else
            roll = false;

        rend.material.SetFloat("_X",  total3.x);
        rend.material.SetFloat("_Y",  total3.y);
        rend.material.SetFloat("_Z",  total3.z);
        rend.material.SetFloat("_Q3", total3.w);

        rend.material.SetFloat("_WX", total4.x);
        rend.material.SetFloat("_WY", total4.y);
        rend.material.SetFloat("_WZ", total4.z);
        rend.material.SetFloat("_Q4", total4.w);

        total3.Normalize();

    }

    void OnGUI()
    {
        Event e = Event.current;
        if (e.isMouse)
        {
            float x = Mathf.Round(e.delta.x * 100) / 100;
            float y = Mathf.Round(e.delta.y * 100) / 100;

            int speed = 2;
            if (roll)
            {

                Vector2 mousePos = Input.mousePosition;
                if (mousePos.x > Screen.width / 2) y *= -1;
                if (mousePos.y < Screen.height / 2) x *= -1;
                if (fdr)
                {
                    float z = -x + y;
                    total4 *= Quaternion.AngleAxis(z / speed, new Vector3(0, 0, 1));
                }
                else
                {
                    float z = -x + y;
                    total3 *= Quaternion.AngleAxis( z/speed, new Vector3(0,0,1));
                }
            }
            else
            {
                if (fdr)
                {
                    total4 *= Quaternion.AngleAxis(-x / speed, new Vector3(0, 1, 0));
                    total4 *= Quaternion.AngleAxis(y / speed, new Vector3(1, 0, 0));
                }
                else
                {

                    total3 *= Quaternion.AngleAxis(-x/speed, new Vector3(0,1,0));
                    total3 *= Quaternion.AngleAxis( y/speed, new Vector3(1,0,0));

                }
            }
        }
    }

}
