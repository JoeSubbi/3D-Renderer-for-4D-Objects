using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // Visualisations
    public static bool Four_to_Three = false;
    public static bool Multi_View = true;
    public static bool Timeline = false;

    public Shader Control_Shader;
    public Shader Multi_View_Shader;
    public Shader Timeline_Shader;
    public GameObject Window;
    private Renderer mainRenderer;

    // Tests
    public static bool Shape_Match = false;
    public static bool Rotation_Match = false;
    public static bool Pose_Match = true;

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

    private float buffer;

    // Start is called before the first frame update
    void Start()
    {
        // Get components
        canvas = GetComponent<RectTransform>();
        mainRenderer = Window.GetComponent<Renderer>();
        miniRect = MiniWindow.GetComponent<RectTransform>();
        matchRect = MatchWindow.GetComponent<RectTransform>();
        shapeOptionRect = ShapeOptionContainer.GetComponent<RectTransform>();
        rotationOptionRect = RotationOptionContainer.GetComponent<RectTransform>();

        // padding between UI elements and the edge
        buffer = Mathf.Min(canvas.sizeDelta.x / 10, canvas.sizeDelta.y / 10);

        // Set method of rotation
        SwitchRotation(grabBall);
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
}
