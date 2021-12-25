using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurveyReader : MonoBehaviour
{
    public GameObject ShapeUI;
    public GameObject RotationUI;
    public GameObject PoseUI;

    //Shape Match
    public Slider ShapeConfidance;
    public ToggleGroup ShapeBehaviour;
    // Rotate Match
    public Slider RotateConfidance;
    public ToggleGroup RotateBehaviour;
    // Pose Match
    public Slider PoseConfidance;

    void Awake()
    {
        Debug.Log("Loaded Survey Scene");
        // Set survey based on test
        switch (StateController.test)
        {
            case 0:
                ShapeUI.SetActive(true);
                RotationUI.SetActive(false);
                PoseUI.SetActive(false);
                break;
            case 1:
                ShapeUI.SetActive(false);
                RotationUI.SetActive(true);
                PoseUI.SetActive(false);
                break;
            case 2:
                ShapeUI.SetActive(false);
                RotationUI.SetActive(false);
                PoseUI.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void Save()
    {
        // Save relevant content
        switch (StateController.test)
        {
            case 0:
                GetShapeMatchContent();
                break;
            case 1:
                GetRotationMatchContent();
                break;
            case 2:
                GetPoseMatchContent();
                break;
            default:
                break;
        }
    }

    public void GetShapeMatchContent()
    {
        float confidance = ShapeConfidance.value;
        string behaviour = "N/A";

        // May have several selected toggles
        foreach (Toggle toggle in ShapeBehaviour.ActiveToggles())
            behaviour = toggle.name;
    }

    public void GetRotationMatchContent()
    {
        float confidance = RotateConfidance.value;
        string behaviour = "N/A";

        // May have several selected toggles
        foreach (Toggle toggle in RotateBehaviour.ActiveToggles())
            behaviour = toggle.name;
        Debug.Log(behaviour);
    }

    public void GetPoseMatchContent()
    {
        float confidance = PoseConfidance.value;
    }
}
