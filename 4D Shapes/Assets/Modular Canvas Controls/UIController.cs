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
        // Update Shape Positons
        rend.material.SetFloat("_Z", 0);
        rend.material.SetFloat("_X", 0);
        rend.material.SetFloat("_Y", 0);

        // Representation
        if (Four_to_Three)
        {
            if (Shape_Match)
            {
                MiniWindow.GetComponent<RectTransform>().anchoredPosition = new Vector2(container.sizeDelta.x + 60,
                                                                                        MiniWindow.GetComponent<RectTransform>().anchoredPosition.y);
                MiniWindow.GetComponent<Image>().material.SetFloat("_X",
                                                                      MiniWindow.GetComponent<Image>().material.GetFloat("_X") + container.sizeDelta.x);
                rend.material.SetFloat("_X", -1);
            }
            else
            {
                MiniWindow.GetComponent<RectTransform>().anchoredPosition = new Vector2(60,
                                                                                        MiniWindow.GetComponent<RectTransform>().anchoredPosition.y);
                rend.material.SetFloat("_X", 0);
            }
            rend.material.shader = Four_to_Three_Shader;
            MiniWindow.SetActive(true);
        }
        else if (Multi_View)
        {
            rend.material.shader = Multi_View_Shader;
            rend.material.SetFloat("_Z", -2.5f);
            MiniWindow.SetActive(false);
        }
        else if (Timeline)
        {
            rend.material.shader = Timeline_Shader;
            rend.material.SetFloat("_Z", -7f);
            rend.material.SetFloat("_X", 0.6f);
            if (Shape_Match)
                rend.material.SetFloat("_X", -0.6f);
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
