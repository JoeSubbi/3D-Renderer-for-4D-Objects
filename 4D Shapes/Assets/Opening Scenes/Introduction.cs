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
        max = intro_script.sizeDelta.y/2;
    }

    void Update()
    {
        if (y / max != scroll_bar.value)
            y = scroll_bar.value * max;
        intro_script.anchoredPosition = new Vector2(intro_script.anchoredPosition.x, scroll_bar.value * max - 10);
    }

    void OnGUI()
    {
        y -= Input.mouseScrollDelta.y * speed;
        scroll_bar.value = y / max;
        background.material.SetFloat("_W", 5.5f * (scroll_bar.value)-3);
    }
}
