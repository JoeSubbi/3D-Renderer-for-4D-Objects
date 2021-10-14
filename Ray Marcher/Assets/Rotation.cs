using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Rotation : MonoBehaviour
{
    public float PI = 3.141592653589793f;//23846264338327950

    public Vector3 rot3 = new Vector3(.0f, .0f, .0f);
    public Vector3 rot4 = new Vector3(.0f, .0f, .0f);
    private Renderer rend;
    public Vector3 prevRot = new Vector3(.0f, .0f, .0f);
    private bool fdr = false;
    private bool roll = false;
    
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        rot3.x = 0;
        rot3.y = 0;
        rot3.z = 0;

        rot4.x = 0;
        rot4.y = 0;
        rot4.z = 0;
    }

    void NormaliseRotation()
    {
        rot3.x = rot3.x % (2 * PI);
        rot3.y = rot3.y % (2 * PI);
        rot3.z = rot3.z % (2 * PI);

        rot4.x = rot4.x % (2 * PI);
        rot4.y = rot4.y % (2 * PI);
        rot4.z = rot4.z % (2 * PI);
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
        NormaliseRotation();

        rend.material.SetFloat("_XY", rot3.z);
        rend.material.SetFloat("_ZX", rot3.y);
        rend.material.SetFloat("_YZ", rot3.x);

        rend.material.SetFloat("_WX", rot4.x);
        rend.material.SetFloat("_WY", rot4.y);
        rend.material.SetFloat("_WZ", rot4.z);

    }

    void OnGUI()
    {
        Event e = Event.current;
        if (e.isMouse)
        {
            float x = Mathf.Round(e.delta.x * 100) / 100;
            float y = Mathf.Round(e.delta.y * 100) / 100;

            int speed = 100;
            if (roll)
            {

                Vector2 mousePos = Input.mousePosition;
                if (mousePos.x > Screen.width / 2) y *= -1;
                if (mousePos.y < Screen.height / 2) x *= -1;
                if (fdr)
                {
                    rot4.z -= x / speed;
                    rot4.z += y / speed;
                }
                else
                {
                    rot3.z -= x / speed;
                    rot3.z += y / speed;
                }
            }
            else
            {
                if (fdr)
                {
                    rot4.x += y / speed;
                    rot4.y -= x / speed;
                    Debug.Log("w" + rot4);
                }
                else
                {
                    rot3.x += y / speed;
                    rot3.y -= x / speed;
                }
            }
            Debug.Log("3D Rotation:" + rot3);
            Debug.Log("4D Rotation:" + rot4);
        }
    }

}
