using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasWindow : MonoBehaviour
{
    public RectTransform MiniWindow;
    public RectTransform MatchWindow;

    private RectTransform canvas;

    public Slider wSlider;
    private RectTransform wSliderRect;
    public RectTransform axesPanel;

    private Vector2 resolution;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<RectTransform>();
        wSliderRect = wSlider.GetComponent<RectTransform>();
        
        // Set scale for UI elements
        resolution = new Vector2(Screen.width, Screen.height);
        Rescale();
    }

    // Update is called once per frame
    void Update()
    {
        // If screen has changed, rescale the UI elements
        if (resolution.x != Screen.width || resolution.y != Screen.height)
        {
            Rescale();

            resolution.x = Screen.width;
            resolution.y = Screen.height;
        }
    }

    private void Rescale()
    {
        // Maintain 4:3 ratio for MiniWindow
        Vector2 aspectRatio = new Vector2(4, 3);

        float x = canvas.sizeDelta.x / 2.5f;
        float y = ((x / aspectRatio.x) * aspectRatio.y);
        if (y > canvas.sizeDelta.y / 3)
        {
            y = canvas.sizeDelta.y / 3;
            x = ((y / aspectRatio.y) * aspectRatio.x);
        }
        MiniWindow.sizeDelta = new Vector2(x, y);

        // Maintain a 1:1 ratio for Match Window
        aspectRatio = new Vector2(1, 1);
        float scale = 2f;

        x = canvas.sizeDelta.x / scale;
        y = ((x / aspectRatio.x) * aspectRatio.y);
        if (y > canvas.sizeDelta.y / scale)
        {
            y = canvas.sizeDelta.y / scale;
            x = ((y / aspectRatio.y) * aspectRatio.x);
        }
        MatchWindow.sizeDelta = new Vector2(x, y);

        // Distance from edge of screen
        float buffer = Mathf.Min(canvas.sizeDelta.x / 10, canvas.sizeDelta.y / 10);

        MiniWindow.anchoredPosition = new Vector2(buffer, buffer);
        MatchWindow.anchoredPosition = new Vector2(buffer
                                                   + MiniWindow.sizeDelta.x / 2
                                                   - MatchWindow.sizeDelta.x / 2,
                                                   -buffer/2);

        wSliderRect.sizeDelta = new Vector2(wSliderRect.sizeDelta.x, -2 * buffer);
        wSliderRect.anchoredPosition = new Vector2(-(buffer + 10), 0);

        axesPanel.anchoredPosition = new Vector2(-2*buffer, buffer);
    }
}
