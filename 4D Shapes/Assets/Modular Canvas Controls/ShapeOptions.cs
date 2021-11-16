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
        

        float y = (canvas.sizeDelta.y - 120 - 40) / 5 ;
        float x = (y / 3) * 4.5f;

        container.sizeDelta = new Vector2(x+20, 0);
        Vector2 size = new Vector2(x, y);

        foreach (RectTransform shape in options)
        {
            shape.sizeDelta = size;
        }
        options[0].anchoredPosition = new Vector2(10, 0);
        options[1].anchoredPosition = new Vector2(10,  y + 10);
        options[2].anchoredPosition = new Vector2(10, -y - 10);
        options[3].anchoredPosition = new Vector2(10,  2*y + 2*10);
        options[4].anchoredPosition = new Vector2(10, -2*y - 2*10);
    }
}
