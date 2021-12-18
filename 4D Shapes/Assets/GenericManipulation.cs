using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenericManipulation : MonoBehaviour
{
    public float w = 0;
    public int shape = 0;
    private Renderer rend4;
    private Slider s1;
    private Slider s2;

    private Toggle[] b;
    private Rotor4 rotor = new Rotor4(1,0,0,0,0,0,0,0);

    // Start is called before the first frame update
    void Start()
    {
        rend4 = GameObject.Find("3D").GetComponent<Renderer>();
        s1 = GameObject.Find("Slider").GetComponent<Slider>();
        s2 = GameObject.Find("Shape").GetComponent<Slider>();

        b = new Toggle[]{
                GameObject.Find("YZ Rotation").GetComponent<Toggle>(),
                GameObject.Find("XZ Rotation").GetComponent<Toggle>(),
                GameObject.Find("XY Rotation").GetComponent<Toggle>(),
                GameObject.Find("XW Rotation").GetComponent<Toggle>(),
                GameObject.Find("YW Rotation").GetComponent<Toggle>(),
                GameObject.Find("ZW Rotation").GetComponent<Toggle>()
            };
    }

    // Update is called once per frame
    void Update()
    {
        //W Position of object
        w = s1.value;
        rend4.material.SetFloat("_W", w);

        //Shape to display
        shape = (int)s2.value;
        rend4.material.SetFloat("_Shape", shape);

        //Rotation of the shapes
        Rotate();

        //Position 2D Object according to screen
        Vector2 canvasSize = GameObject.Find("Canvas").GetComponent<RectTransform>().sizeDelta;

        s1.GetComponent<RectTransform>().sizeDelta = new Vector2(20, canvasSize.y * 20 - 20);
        s2.GetComponent<RectTransform>().sizeDelta = new Vector2(20, canvasSize.y * 20 - 20);
    }

    private void Rotate()
    {
        //Rotation of the shapes
        //YZ
        if (b[0].isOn) buildRotor(new Vector4(0, 1, 0, 0), new Vector4(0, 0, 1, 0));
        //XZ
        if (b[1].isOn) buildRotor(new Vector4(1, 0, 0, 0), new Vector4(0, 0, 1, 0));
        //XY
        if (b[2].isOn) buildRotor(new Vector4(1, 0, 0, 0), new Vector4(0, 1, 0, 0));
        //XW
        if (b[3].isOn) buildRotor(new Vector4(1, 0, 0, 0), new Vector4(0, 0, 0, 1));
        //YW
        if (b[4].isOn) buildRotor(new Vector4(0, 1, 0, 0), new Vector4(0, 0, 0, 1));
        //ZW
        if (b[5].isOn) buildRotor(new Vector4(0, 0, 1, 0), new Vector4(0, 0, 0, 1));

        rend4.material.SetFloat("_A", rotor.a); 
        rend4.material.SetFloat("_YZ", rotor.byz);
        rend4.material.SetFloat("_XZ", rotor.bxz);
        rend4.material.SetFloat("_XY", rotor.bxy);
        rend4.material.SetFloat("_XW", rotor.bxw);
        rend4.material.SetFloat("_YW", rotor.byw);
        rend4.material.SetFloat("_ZW", rotor.bzw);
        rend4.material.SetFloat("_XYZW", rotor.bxyzw);
    }

    private Rotor4 buildRotor(Vector4 e1, Vector4 e2)
    {
        Bivector4 bv = Bivector4.Wedge(e1, e2);
        Rotor4 r = new Rotor4(bv, Time.deltaTime);
        rotor *= r;
        rotor.Normalise();
        return rotor;
    }
}