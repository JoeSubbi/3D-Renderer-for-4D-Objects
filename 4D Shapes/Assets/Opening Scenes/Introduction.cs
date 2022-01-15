using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Introduction : MonoBehaviour
{
    public RectTransform intro_script;
    public Renderer background;
    public Slider scroll_bar;
 
    private RectTransform canvas;
    private float max;
    private int direction;
    private float y = -10;
    public float speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<RectTransform>();
        if (intro_script.sizeDelta.y > canvas.sizeDelta.y)
        {
            max = intro_script.sizeDelta.y - canvas.sizeDelta.y/2;
        }
    }

    void Update()
    {
        y = (scroll_bar.value * (max-10))-10;
        intro_script.anchoredPosition = new Vector2(intro_script.anchoredPosition.x, y);
    }

    void OnGUI()
    {
        y -= Input.mouseScrollDelta.y * speed;
        if (intro_script.anchoredPosition.y > max && Input.mouseScrollDelta.y < 0)
            y = max;
        else if (intro_script.anchoredPosition.y < 10 && Input.mouseScrollDelta.y > 0)
            y = 10;
        scroll_bar.value = (y+10)/(max-10);

        background.material.SetFloat("_W", 5.5f * (y / max)-3);
    }
}
