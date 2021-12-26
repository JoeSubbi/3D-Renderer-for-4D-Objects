﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // Visualisations
    public static bool Four_to_Three = true;
    public static bool Multi_View = false;
    public static bool Timeline = false;

    public Shader Control_Shader;
    public Shader Multi_View_Shader;
    public Shader Timeline_Shader;
    public GameObject Window;
    private Renderer mainRenderer;

    // Tests
    public static bool Shape_Match = false;
    public static bool Rotation_Match = false;
    public static bool Pose_Match = false;

    // Method of rotation
    public bool grabBall;
    public GameObject GrabBall;
    public GameObject SwipeEmpty;

    // UI Elements
    public GameObject MiniWindow;
    public GameObject MatchWindow;
    public GameObject ShapeOptionContainer;
    public GameObject RotationOptionContainer;

    private RectTransform miniRect;
    private RectTransform matchRect;
    private RectTransform shapeOptionRect;
    private RectTransform rotationOptionRect;
    private RectTransform canvas;

    public Slider wSlider;
    private RectTransform wSliderRect;
    public RectTransform axesPanel;
    public RectTransform submitButton;

    // Resolution of Screen
    private Vector2 resolution;

    // Padding between UI elements and edge of screen
    private float buffer;

    // Start is called before the first frame update
    void Awake()
    {

        // Get components
        canvas = GetComponent<RectTransform>();
        mainRenderer = Window.GetComponent<Renderer>();
        miniRect = MiniWindow.GetComponent<RectTransform>();
        matchRect = MatchWindow.GetComponent<RectTransform>();
        shapeOptionRect = ShapeOptionContainer.GetComponent<RectTransform>();
        rotationOptionRect = RotationOptionContainer.GetComponent<RectTransform>();
        wSliderRect = wSlider.GetComponent<RectTransform>();

        // Set scale for UI elements
        resolution = new Vector2(Screen.width, Screen.height);
        Rescale();

        // Initialise StateController parameters
        StateController.ModularSceneCanvas = GetComponent<Canvas>();

        // Set method of rotation
        SwitchRotation(grabBall);

        // Set up booleans describing which test is enabled
        SetTest();
    }

    // Update is called once per frame
    void Update()
    {
        // Set up representation
        if (Four_to_Three)
            FourToThreeRepresentation();
        else if (Multi_View)
            MultiViewRepresentation();
        else if (Timeline)
            TimelineRepresentation();
        else
            ControlRepresentation();

        // Set up test UI features
        ModeUI();

        // Rescale UI if screen size has changed
        if (resolution.x != Screen.width || resolution.y != Screen.height)
        {
            Rescale();

            resolution.x = Screen.width;
            resolution.y = Screen.height;
        }
    }


    public void SwitchRotation(bool grabBall)
    {
        if (grabBall)
        {
            GrabBall.SetActive(true);
            SwipeEmpty.SetActive(false);
        }
        else
        {
            GrabBall.SetActive(false);
            SwipeEmpty.SetActive(true);
        }
    }

    // Set up test UI features
    public void ModeUI()
    {
        // shape menu
        if (Shape_Match)
            ShapeOptionContainer.SetActive(true);
        else
            ShapeOptionContainer.SetActive(false);
        // rotation menu
        if (Rotation_Match)
            RotationOptionContainer.SetActive(true);
        else
            RotationOptionContainer.SetActive(false);
        // Randomly Posed Object window
        if (Pose_Match)
            MatchWindow.SetActive(true);
        else
            MatchWindow.SetActive(false);
    }

    // Set test booleans
    public void SetTest()
    {
        switch (StateController.test)
        {
            case 0:
                Shape_Match = true;
                Rotation_Match = false;
                Pose_Match = false;
                break;
            case 1:
                Shape_Match = false;
                Rotation_Match = true;
                Pose_Match = false;
                break;
            case 2:
                Shape_Match = false;
                Rotation_Match = false;
                Pose_Match = true;
                break;
            default:
                Shape_Match = false;
                Rotation_Match = false;
                Pose_Match = false;
                break;
        }
    }

    // Set up representations
    private void FourToThreeRepresentation()
    {
        // Set Shader
        mainRenderer.material.shader = Control_Shader;

        // Constent Object Positions
        mainRenderer.material.SetFloat("_Y", 0);
        mainRenderer.material.SetFloat("_Z", 0);

        // Set main and mini object positions when match test is active
        if (Shape_Match || Rotation_Match)
        {
            mainRenderer.material.SetFloat("_X", -1f);
            miniRect.anchoredPosition = new Vector2(shapeOptionRect.sizeDelta.x + buffer / 2, buffer);

            // Position Grab Ball
            GrabBall.transform.position = new Vector3(1, 0, 0);
        }

        // Otherwise set default position
        else
        {
            mainRenderer.material.SetFloat("_X", -0.75f);
            miniRect.anchoredPosition = new Vector2(buffer, buffer);

            // Position Grab Ball
            GrabBall.transform.position = new Vector3(0.75f, 0, 0);
        }

        // Set match object positions
        if (Pose_Match)
        {
            matchRect.anchoredPosition = new Vector2(buffer
                                                + miniRect.sizeDelta.x / 2
                                                - matchRect.sizeDelta.x / 2,
                                                -buffer / 2);
        }

        // Set appropriate representation
        MiniWindow.SetActive(true);

        // Scale Grab Ball
        GrabBall.transform.localScale = new Vector3(1, 1, 1);
    }

    private void MultiViewRepresentation()
    {
        // Set Shader
        mainRenderer.material.shader = Multi_View_Shader;

        // Constent Object Positions
        mainRenderer.material.SetFloat("_Z", -3);
        mainRenderer.material.SetFloat("_Y", -0.35f);

        // Set main object positions when match test is active
        if (Shape_Match || Rotation_Match)
        {
            mainRenderer.material.SetFloat("_X", -0.1f);

            // Position Grab Ball
            GrabBall.transform.position = new Vector3(-1.4f, 1.15f, 0);
        }

        // Set main and match object positions when pose test is active
        else if (Pose_Match)
        {
            mainRenderer.material.SetFloat("_X", -0.4f);
            matchRect.anchoredPosition = new Vector2(buffer / 2 - buffer / 10,
                                                     0);
            // Position and Grab Ball
            GrabBall.transform.position = new Vector3(-1.1f, 1.15f, 0);
        }

        // Otherwise set default position
        else
        {
            mainRenderer.material.SetFloat("_X", 0);

            // Position Grab Ball
            GrabBall.transform.position = new Vector3(-1.5f, 1.15f, 0);
        }

        // Disable 3D counterpart
        MiniWindow.SetActive(false);

        // Scale Grab Ball
        GrabBall.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
    }

    private void TimelineRepresentation()
    {
        // Set Shader
        mainRenderer.material.shader = Timeline_Shader;

        // Constent Object Positions
        mainRenderer.material.SetFloat("_Y", 0);

        // Set main object positions when match test is active
        if (Shape_Match || Rotation_Match)
        {
            mainRenderer.material.SetFloat("_X", -0.4f);
            mainRenderer.material.SetFloat("_Z", -7f);

            // Position and Scale Grab Ball
            GrabBall.transform.position = new Vector3(0.16f, 0, 0);
            GrabBall.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        }

        // Otherwise set default position
        else
        {
            mainRenderer.material.SetFloat("_X", 0.6f);
            mainRenderer.material.SetFloat("_Z", -5f);

            // Position and Scale Grab Ball
            GrabBall.transform.position = new Vector3(-0.28f, 0, 0);
            GrabBall.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        }

        // Set match object positions
        if (Pose_Match)
        {
            Vector2 position = new Vector2(canvas.sizeDelta.x / 2f - matchRect.sizeDelta.x / 2
                                           - 50 * canvas.sizeDelta.y / canvas.sizeDelta.x, buffer / 2);
            matchRect.anchoredPosition = position;
        }

        // Disable 3D counterpart
        MiniWindow.SetActive(false);
    }

    private void ControlRepresentation()
    {
        // Set Shader
        mainRenderer.material.shader = Control_Shader;

        // Constent Object Positions
        mainRenderer.material.SetFloat("_Z", 0);
        mainRenderer.material.SetFloat("_X", 0);
        mainRenderer.material.SetFloat("_Y", 0);

        // Set match object positions
        if (Pose_Match)
            matchRect.anchoredPosition = new Vector2(buffer / 2, -buffer / 2);

        // Disable 3D counterpart
        MiniWindow.SetActive(false);

        // Position and Scale Grab Ball
        GrabBall.transform.position = new Vector3(0, 0, 0);
        GrabBall.transform.localScale = new Vector3(1, 1, 1);
    }

    public void SetRepresentation(int r)
    {
        switch (r)
        {
            case 1:
                FourToThreeRepresentation();
                break;
            case 2:
                MultiViewRepresentation();
                break;
            case 3:
                TimelineRepresentation();
                break;
            default:
                ControlRepresentation();
                break;
        }
    }


    // Scale UI
    private void Rescale()
    {
        // Set padding between UI elements and edge of screen
        buffer = Mathf.Min(canvas.sizeDelta.x / 10, canvas.sizeDelta.y / 10);

        // Maintain 4:3 ratio for MiniWindow (3D-4D)
        Vector2 aspectRatio = new Vector2(4, 3);

        float x = canvas.sizeDelta.x / 2.5f;
        float y = ((x / aspectRatio.x) * aspectRatio.y);
        if (y > canvas.sizeDelta.y / 3)
        {
            y = canvas.sizeDelta.y / 3;
            x = ((y / aspectRatio.y) * aspectRatio.x);
        }
        miniRect.sizeDelta = new Vector2(x, y);

        // Maintain a 1:1 ratio for Match Window (Pose_Match)
        aspectRatio = new Vector2(1, 1);
        float scale = 2f;

        x = canvas.sizeDelta.x / scale;
        y = ((x / aspectRatio.x) * aspectRatio.y);
        if (y > canvas.sizeDelta.y / scale)
        {
            y = canvas.sizeDelta.y / scale;
            x = ((y / aspectRatio.y) * aspectRatio.x);
        }
        matchRect.sizeDelta = new Vector2(x, y);

        wSliderRect.sizeDelta = new Vector2(wSliderRect.sizeDelta.x, -2 * buffer);
        wSliderRect.anchoredPosition = new Vector2(-(buffer + 10), 0);

        axesPanel.anchoredPosition = new Vector2(-2 * buffer, buffer);

        submitButton.sizeDelta = new Vector2(2 * buffer, buffer);
        submitButton.anchoredPosition = new Vector2(-2 * buffer, -buffer);
    }
}
