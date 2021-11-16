using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasWindow : MonoBehaviour
{
    public RectTransform MiniWindow;
    public RectTransform MatchWindow;

    private RectTransform canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 aspectRatio = new Vector2(16, 9);
        Vector2 aspectRatio = new Vector2(4, 3);

        float x = canvas.sizeDelta.x / 2.5f;
        float y = ((x / aspectRatio.x) * aspectRatio.y);
        if (y > canvas.sizeDelta.y / 3)
        {
            y = canvas.sizeDelta.y / 3;
            x = ((y / aspectRatio.y) * aspectRatio.x);
        }

        Vector2 size = new Vector2(x, y);
        MiniWindow.sizeDelta  = size;
        MatchWindow.sizeDelta = size;

        //Position of 3D object
        // Does not move when window is max size + constant offset - width of window when window scales with canvas
        float X = -canvas.sizeDelta.x / 10f - (canvas.sizeDelta.x / 2.5f - x) + 60 - x / 2f;
        // Moves with y - the scale of the window with a constant y + a constant
        float Y = -(canvas.sizeDelta.y / 3f) - (canvas.sizeDelta.y / 3 - y) / 2 + 100;
        // Move Z back as the window gets smaller
        float Z = -(x / 9 + y / 16) / .3f;
        Y -= (Z/3.5f) + 40;
        MiniWindow.GetComponent<Image>().material.SetFloat("_X", X);
        MiniWindow.GetComponent<Image>().material.SetFloat("_Y", Y);
        MiniWindow.GetComponent<Image>().material.SetFloat("_Z", Z);

        //Position of Randomly Posed Object
        // Does not move when window is max size + constant offset - width of window when window scales with canvas
        X = -canvas.sizeDelta.x / 10f - (canvas.sizeDelta.x / 2.5f - x) + 60 - x / 2f;
        // Moves with y - the scale of the window with a constant y + a constant
        Y = (canvas.sizeDelta.y / 3f) + (canvas.sizeDelta.y / 3 - y) / 2;
        // Move Z back as the window gets smaller
        Z = -(x / 9 + y / 16) / .3f;
        Y -= (Z / 3.5f) + 40;
        MatchWindow.GetComponent<Image>().material.SetFloat("_X", X);
        MatchWindow.GetComponent<Image>().material.SetFloat("_Y", Y);
        MatchWindow.GetComponent<Image>().material.SetFloat("_Z", Z);
    }
}
