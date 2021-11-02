using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WSlider : MonoBehaviour
{
    public float w = 0;
    public int shape = 0;
    private Renderer rend4;
    private Image im3;
    private Slider s1;
    private Slider s2;

    private Toggle[] b;

    // Start is called before the first frame update
    void Start()
    {
        rend4 = GameObject.Find("3D").GetComponent<Renderer>();
        im3 = GameObject.Find("2D").GetComponent<Image>();
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
        im3.material.SetFloat("_Shape", shape);

        //Rotation of the shapes
        Rotate();
        //Time.time

        //Position 2D Object according to screen
        Vector2 canvasSize = GameObject.Find("Canvas").GetComponent<RectTransform>().sizeDelta;

        s1.GetComponent<RectTransform>().sizeDelta = new Vector2(20, canvasSize.y * 20 - 20);
        s2.GetComponent<RectTransform>().sizeDelta = new Vector2(20, canvasSize.y * 20 - 20);

        GameObject.Find("2D").GetComponent<RectTransform>().sizeDelta = new Vector2(canvasSize.x / 2.5f, canvasSize.y / 2.3f);
        float x = (-canvasSize.x / 3.55f);
        float z = (-canvasSize.y * 1.1f) + 7 + (canvasSize.y * 0.35f);
        im3.material.SetFloat("_X", x);
        im3.material.SetFloat("_Z", z);
    }

    private void Rotate()
    {
        //Rotation of the shapes
        //YZ
        if (b[0].isOn)
        {
            rend4.material.SetFloat("_ZY", Time.time);
        }
        else
        {
            rend4.material.SetFloat("_ZY", 0);
        }
        //XZ
        if (b[1].isOn)
        {
            rend4.material.SetFloat("_XZ", Time.time);
        }
        else
        {
            rend4.material.SetFloat("_XZ", 0);
        }
        //XY
        if (b[2].isOn)
        {
            rend4.material.SetFloat("_XY", Time.time);
        }
        else
        {
            rend4.material.SetFloat("_XY", 0);
        }
        //XW
        if (b[3].isOn)
        {
            rend4.material.SetFloat("_XW", Time.time);
            im3.material.SetFloat("_XY", Time.time);
        }
        else
        {
            rend4.material.SetFloat("_XW", 0);
            im3.material.SetFloat("_XY", 0);
        }
        //YW
        if (b[4].isOn)
        {
            rend4.material.SetFloat("_YW", Time.time);
            im3.material.SetFloat("_XZ", Time.time);
        }
        else
        {
            rend4.material.SetFloat("_YW", 0);
            im3.material.SetFloat("_XZ", 0);
        }
        //ZW
        if (b[5].isOn)
        {
            rend4.material.SetFloat("_ZW", Time.time);
            im3.material.SetFloat("_ZY", Time.time);
        }
        else
        {
            rend4.material.SetFloat("_ZW", 0);
            im3.material.SetFloat("_ZY", 0);
        }
    }
}
