using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public Vector3 rot3 = new Vector3(.0f, .0f, .0f);
    public Vector3 rot4 = new Vector3(.0f, .0f, .0f);
    private Renderer rend;
    public Vector3 prevRot = new Vector3(.0f, .0f, .0f);
    
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        rend.material.SetFloat("_XY", rot3.z); //Z
        rend.material.SetFloat("_ZX", rot3.y); //Y
        rend.material.SetFloat("_YZ", rot3.x); //X

        rend.material.SetFloat("_WX", rot4.x);
        rend.material.SetFloat("_WY", rot4.y);
        rend.material.SetFloat("_WZ", rot4.z);

    }

    Vector4 RotationMatrix(float a)
    {
        float s = Mathf.Sin(a);
        float c = Mathf.Cos(a);
        return new Vector4(c, s, -s, c);
    }

    /** 
     * \brief   Matrix Multiplication when given a 2x2 matrix
     * 
     * \param   mat rotation matrix
     * \param   a   axis to rotate from
     * \param   b   axis to rotate towards 
     *
     */
    Vector2 RotMatMul(Vector4 mat, float a, float b)
    {
        Vector2 rotation = new Vector2(a * mat[0] + b * mat[1],
                                       a * mat[2] + b * mat[3]);
        return rotation;
    }

    void OnGUI()
    {
        Event e = Event.current;
        if (e.isMouse)
        {
            //Debug.Log(e.delta);
            int speed = 100;
            rot3.x -= e.delta.y/speed;
            rot3.y -= e.delta.x/speed;
            Debug.Log(rot3);
        }
    }

}
