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
        int pady = 10;
        int padx = 25;

        //Height of canvas - 60 buffer on top and bottom
        //                 - gap of 10 above and below each panel
        float y = (canvas.sizeDelta.y - 120 - (options.Length+1)*pady) / options.Length ;
        //Maintain a 4.5:3 aspect ratio
        float x = (y / 3) * 4.5f;

        container.sizeDelta = new Vector2(x+pady+padx, -120);
        container.anchoredPosition = new Vector2(0, -60);

        Vector2 size = new Vector2(x, y);
        foreach (RectTransform shape in options)
        {
            shape.sizeDelta = size;
        }
        options[0].anchoredPosition = new Vector2(padx, 0);
        options[1].anchoredPosition = new Vector2(padx,  y + 10);
        options[2].anchoredPosition = new Vector2(padx, -y - 10);
        options[3].anchoredPosition = new Vector2(padx,  2*y + 2*10);
        options[4].anchoredPosition = new Vector2(padx, -2*y - 2*10);
    }
}
