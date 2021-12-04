﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShapeOptions : MonoBehaviour
{
    public RectTransform container;
    public RectTransform[] options;

    private RectTransform canvas;

    public ToggleGroup ToggleGroup;

    private Vector2 resolution;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<RectTransform>();

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

        LogToggle();
    }

    private void LogToggle()
    {
        // May have several selected toggles
        foreach (Toggle toggle in ToggleGroup.ActiveToggles())
        {
            Debug.Log(toggle);
        }
    }

    //Rescale and position UI elements
    private void Rescale()
    {
        //Buffer from edge of screen
        float buffer = Mathf.Min(canvas.sizeDelta.x / 10, canvas.sizeDelta.y / 10);

        //Padding between panels
        int padxr = 8; //Left
        int padxl = 30; //Right
        int pady = 8;   //Above and Below

        //Height of canvas - 60 buffer on top and bottom
        //                 - gap of pady above and below each panel
        float y = (canvas.sizeDelta.y - 2 * buffer - (options.Length + 1) * pady) / options.Length;
        //Maintain a 4.5:3 aspect ratio
        float x = (y / 3) * 4.5f;

        container.sizeDelta = new Vector2(x + padxr + padxl, -2 * buffer);
        container.anchoredPosition = new Vector2(0, -buffer);

        Vector2 size = new Vector2(x, y);
        for (int i = 0; i < options.Length; i++)
        {
            options[i].sizeDelta = size;
            options[i].anchoredPosition = new Vector2(padxl, i * -y + (i + 1) * -pady);
        }
    }
}
