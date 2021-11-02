using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZSlider : MonoBehaviour
{
    public float z = 0;
    public int shape = 0;
    private Renderer rend3;
    private Image im2;
    private Slider s1;
    private Slider s2;

    private Toggle[] b;

    // Start is called before the first frame update
    void Start()
    {
        rend3 = GameObject.Find("3D").GetComponent<Renderer>();
        im2 = GameObject.Find("2D").GetComponent<Image>();
        s1 = GameObject.Find("Slider").GetComponent<Slider>();
        s2 = GameObject.Find("Shape").GetComponent<Slider>();

        b = new Toggle[]{
                GameObject.Find("X Rotation").GetComponent<Toggle>(),
                GameObject.Find("Y Rotation").GetComponent<Toggle>(),
                GameObject.Find("Z Rotation").GetComponent<Toggle>()
            };
    }

    // Update is called once per frame
    void Update()
    {
        //Z Position of object
        z = s1.value;
        rend3.material.SetFloat("_Z", z);
        im2.material.SetFloat("_Z", z);

        //Shape to display
        shape = (int)s2.value;
        rend3.material.SetFloat("_Shape", shape);
        im2.material.SetFloat("_Shape", shape);

        //Rotation of the shapes
        //X
        if (b[0].isOn)
        {
            rend3.material.SetFloat("_ZY", Time.time);
            im2.material.SetFloat("_ZY", Time.time);
        }
        else
        {
            rend3.material.SetFloat("_ZY", 0);
            im2.material.SetFloat("_ZY", 0);
        }
        //Y
        if (b[1].isOn)
        {
            rend3.material.SetFloat("_XZ", Time.time);
            im2.material.SetFloat("_XZ", Time.time);
        }
        else
        {
            rend3.material.SetFloat("_XZ", 0);
            im2.material.SetFloat("_XZ", 0);
        }
        //Z
        if (b[2].isOn)
        {
            rend3.material.SetFloat("_XY", Time.time);
            im2.material.SetFloat("_XY", Time.time);
        }
        else
        {
            rend3.material.SetFloat("_XY", 0);
            im2.material.SetFloat("_XY", 0);
        }

        //Position 2D Object according to screen
        Vector2 canvasSize = GameObject.Find("Canvas").GetComponent<RectTransform>().sizeDelta;
        s1.GetComponent<RectTransform>().sizeDelta = new Vector2(20, canvasSize.y * 20 - 20);
        s2.GetComponent<RectTransform>().sizeDelta = new Vector2(20, canvasSize.y * 20 - 20);
        GameObject.Find("2D").GetComponent<RectTransform>().sizeDelta = new Vector2(canvasSize.x / 3, (float)(canvasSize.y / 2.4));

        float x = (float)((-canvasSize.x / 3) + 0.3);
        float y = (float)((-canvasSize.y / 3) + 0.4);
        im2.material.SetFloat("_X", x);
        im2.material.SetFloat("_Y", y);
    }
}