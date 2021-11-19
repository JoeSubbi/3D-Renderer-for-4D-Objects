using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // Visualisations
    public bool Four_to_Three;
    public bool Multi_View;
    public bool Timeline;
    public bool Onion_Skin;

    public Shader Four_to_Three_Shader;
    public Shader Multi_View_Shader;
    public Shader Timeline_Shader;
    public Shader Onion_Skin_Shader;
    public GameObject Window;
    private Renderer rend;
    public Slider wSlider;

    // Shape
    public int shape;

    // Tests
    public bool Shape_Match;
    public bool Pose_Match;

    // UI Elements
    public GameObject MiniWindow;
    public GameObject MatchWindow;
    public GameObject ShapeOptions;

    public RectTransform container;

    // Start is called before the first frame update
    void Start()
    {
        rend = Window.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Set Shape
        rend.material.SetInt("_Shape", shape);
        MiniWindow.GetComponent<Image>().material.SetInt("_Shape", shape);
        MatchWindow.GetComponent<Image>().material.SetInt("_Shape", shape);

        rend.material.SetFloat("_W", wSlider.value);

        // Update Shape Positons
        rend.material.SetFloat("_Z", 0);
        rend.material.SetFloat("_X", 0);
        rend.material.SetFloat("_Y", 0);

        // Representation
        if (Four_to_Three)
        {
            // If the shape match container exists, 
            // shift over the object and the mini 3D window
            if (Shape_Match)
            {
                //Move the window
                float padx = container.sizeDelta.x / 4;
                RectTransform rect = MiniWindow.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(container.sizeDelta.x + padx,
                                                    rect.anchoredPosition.y);
                rect.sizeDelta = new Vector2(rect.sizeDelta.x - padx, rect.sizeDelta.y);
                //Move the object IN the winodw
                MiniWindow.GetComponent<Image>().material.SetFloat("_X",
                                                                   MiniWindow.GetComponent<Image>().material.GetFloat("_X") + padx * 3);
                rend.material.SetFloat("_X", -1.25f);
            }
            else
            {
                MiniWindow.GetComponent<RectTransform>().anchoredPosition = new Vector2(60,
                                                                                        MiniWindow.GetComponent<RectTransform>().anchoredPosition.y);
                rend.material.SetFloat("_X", -0.75f);
            }

            MatchWindow.GetComponent<Image>().material.SetFloat("_X",
                                                                MatchWindow.GetComponent<Image>().material.GetFloat("_X") + 40);
            rend.material.shader = Four_to_Three_Shader;
            MiniWindow.SetActive(true);
        }
        else if (Multi_View)
        {
            rend.material.shader = Multi_View_Shader;
            rend.material.SetFloat("_Z", -1.8f);
            if (Shape_Match)
                rend.material.SetFloat("_X", -0.2f);
            if (Pose_Match)
            {
                rend.material.SetFloat("_X", -0.4f);
                MatchWindow.GetComponent<Image>().material.SetFloat("_Y",
                                                                    MatchWindow.GetComponent<Image>().material.GetFloat("_Y") - 30);
                MatchWindow.GetComponent<Image>().material.SetFloat("_Z",
                                                                    MatchWindow.GetComponent<Image>().material.GetFloat("_Z") - 20);
            }
            MiniWindow.SetActive(false);
        }
        else if (Timeline)
        {
            rend.material.shader = Timeline_Shader;
            rend.material.SetFloat("_Z", -5f);
            rend.material.SetFloat("_X", 0.6f);
            if (Shape_Match)
            {
                rend.material.SetFloat("_X", -0.6f);
                rend.material.SetFloat("_Z", -7f);
            }
            if (Pose_Match)
            {
                MatchWindow.GetComponent<Image>().material.SetFloat("_X", - GetComponent<RectTransform>().sizeDelta.y / 17);
                MatchWindow.GetComponent<Image>().material.SetFloat("_Z",
                                                                    MatchWindow.GetComponent<Image>().material.GetFloat("_Z") + 20); 
                MatchWindow.GetComponent<Image>().material.SetFloat("_Y",
                                                                    MatchWindow.GetComponent<Image>().material.GetFloat("_Y") + 10);
            }
            MiniWindow.SetActive(false);
        }
        else if (Onion_Skin)
        {
            rend.material.shader = Onion_Skin_Shader;
            MiniWindow.SetActive(false);
        }
        else
        {
            rend.material.shader = Four_to_Three_Shader;
            MiniWindow.SetActive(false);
        }

        // Mode
        // Select the correct shape menu
        if (Shape_Match) 
            ShapeOptions.SetActive(true);
        else 
            ShapeOptions.SetActive(false);
        // Randomly Posed Object window
        if (Pose_Match)
            MatchWindow.SetActive(true);
        else
            MatchWindow.SetActive(false);
    }
}
