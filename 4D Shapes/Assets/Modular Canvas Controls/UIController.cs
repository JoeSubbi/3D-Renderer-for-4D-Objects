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

    public Shader Four_to_Three_Shader;
    public Shader Multi_View_Shader;
    public Shader Timeline_Shader;
    public GameObject Window;
    private Renderer rend;

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

    // Start is called before the first frame update
    void Start()
    {
        // Get components
        rend = Window.GetComponent<Renderer>();
        miniRect = MiniWindow.GetComponent<RectTransform>();
        matchRect = MatchWindow.GetComponent<RectTransform>();
        shapeOptionRect = ShapeOptionContainer.GetComponent<RectTransform>();
        rotationOptionRect = RotationOptionContainer.GetComponent<RectTransform>();
        canvas = GetComponent<RectTransform>();

        // Set method of rotation
        SwitchRotation(grabBall);
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

    // Update is called once per frame
    void Update()
    {
        // Update Shape Positons
        rend.material.SetFloat("_Z", 0);
        rend.material.SetFloat("_X", 0);
        rend.material.SetFloat("_Y", 0);

        float buffer = Mathf.Min(canvas.sizeDelta.x / 10, canvas.sizeDelta.y / 10);

        // Representation
        if (Four_to_Three)
        {
            rend.material.shader = Four_to_Three_Shader;

            // If the shape match container exists, 
            // shift over the mini 3D window
            if (Shape_Match || Rotation_Match)
            {
                miniRect.anchoredPosition = new Vector2(shapeOptionRect.sizeDelta.x+buffer/2, buffer);
                rend.material.SetFloat("_X", -1f);
                GrabBall.transform.position = new Vector3(1, 0, 0);
            }
            else
            {
                miniRect.anchoredPosition = new Vector2(buffer, buffer);
                rend.material.SetFloat("_X", -0.75f);
                GrabBall.transform.position = new Vector3(0.75f, 0, 0);
            }
            if (Pose_Match)
            {
                matchRect.anchoredPosition = new Vector2(buffer
                                                   + miniRect.sizeDelta.x / 2
                                                   - matchRect.sizeDelta.x / 2,
                                                   -buffer/2);
            }

            // Set appropriate representation
            MiniWindow.SetActive(true);
            GrabBall.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (Multi_View)
        {
            rend.material.shader = Multi_View_Shader;
            rend.material.SetFloat("_Z", -3f);
            rend.material.SetFloat("_Y",  -0.35f);

            if (Shape_Match || Rotation_Match)
            {
                rend.material.SetFloat("_X", -0.1f);
                GrabBall.transform.position = new Vector3(-1.4f, 1.15f, 0);
            }
            else if (Pose_Match)
            {
                rend.material.SetFloat("_X", -0.4f);
                matchRect.anchoredPosition = new Vector2(buffer / 2 - buffer / 10,
                                                         0);
                GrabBall.transform.position = new Vector3(-1.1f, 1.15f, 0);
            }
            else
            {
                GrabBall.transform.position = new Vector3(-1.5f, 1.15f, 0);
            }

            // Disable 3D counterpart
            MiniWindow.SetActive(false);
            GrabBall.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        }
        else if (Timeline)
        {
            rend.material.shader = Timeline_Shader;
            rend.material.SetFloat("_Z", -5f);
            rend.material.SetFloat("_X", 0.6f);

            if (Shape_Match || Rotation_Match)
            {
                rend.material.SetFloat("_X", -0.4f);
                rend.material.SetFloat("_Z", -7f);
                GrabBall.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                GrabBall.transform.position = new Vector3(0.16f, 0, 0);
            }
            else
            {
                GrabBall.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                GrabBall.transform.position = new Vector3(-0.28f, 0, 0);
            }

            if (Pose_Match)
            {
                Vector2 position = new Vector2(canvas.sizeDelta.x / 2f - matchRect.sizeDelta.x / 2
                                               -50*canvas.sizeDelta.y/ canvas.sizeDelta.x, buffer/2);
                matchRect.anchoredPosition = position;
            }

            // Disable 3D counterpart
            MiniWindow.SetActive(false);
        }
        else // Control - just 4D object
        {
            rend.material.shader = Four_to_Three_Shader;

            // Set match window positon
            if (Pose_Match)
                matchRect.anchoredPosition = new Vector2(buffer/2, -buffer/2);

            // Disable 3D counterpart
            MiniWindow.SetActive(false);
            GrabBall.transform.position = new Vector3(0, 0, 0);
            GrabBall.transform.localScale = new Vector3(1, 1, 1);
        }

        // Mode
        // Select the correct shape menu
        if (Shape_Match) 
            ShapeOptionContainer.SetActive(true);
        else 
            ShapeOptionContainer.SetActive(false);
        // Select the correct rotation menu
        if (Rotation_Match)
            RotationOptionContainer.SetActive(true);
        else
            RotationOptionContainer.SetActive(false);
        // Randomly Posed Object window
        if (Pose_Match)
            MatchWindow.SetActive(true);
        else {
            MatchWindow.SetActive(false);
        }
    }
}
