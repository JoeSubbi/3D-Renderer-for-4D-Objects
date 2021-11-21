     using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShapeOptions : MonoBehaviour
{
    public RectTransform container;
    public RectTransform[] options;

    private RectTransform canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //Padding between panels
        int padxr = 8; //Left
        int padxl = 30; //Right
        int pady  = 8;   //Above and Below

        //Height of canvas - 60 buffer on top and bottom
        //                 - gap of pady above and below each panel
        float y = (canvas.sizeDelta.y - 120 - (options.Length+1)*pady) / options.Length ;
        //Maintain a 4.5:3 aspect ratio
        float x = (y / 3) * 4.5f;

        container.sizeDelta = new Vector2(x+padxr+padxl, -120);
        container.anchoredPosition = new Vector2(0, -60);

        Vector2 size = new Vector2(x, y);
        for (int i = 0; i< options.Length; i++)
        {
            options[i].sizeDelta = size;
            options[i].anchoredPosition = new Vector2(padxl, i*-y + (i+1) * -pady);
        }
    }
}
