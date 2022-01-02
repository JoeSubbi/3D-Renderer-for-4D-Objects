using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using System.IO;
using System.Linq;

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

    // Save the current content of the survey scene depending on what representation is used
    public void Save()
    {
        // Set up JSON Node
        JSONNode node;
        using (StreamReader r = new StreamReader(Path.Combine(StateController.Datapath, StateController.Filename)))
        {
            //read in the json
            var json = r.ReadToEnd();

            //reformat the json into dictionary style convention
            node = JSON.Parse(json);
        }

        // Get Representation
        string rep_name = StateController.representations[StateController.rep_index];
        // Get Test
        string test_name = StateController.tests[StateController.test];
        // Combine Test with iteration of test e.g ShapeMatch7
        string survey_name_id = test_name + StateController.test_count + "_Survey";

        // Build Dictionary of test parameters and performance
        JSONNode temp = JSON.Parse("{}");
        node[rep_name][test_name].Add(survey_name_id, temp);

        JSONNode survey_node = node[rep_name][test_name][survey_name_id];

        // Get results from survey and add them to dictionary
        Dictionary<string, string> results = new Dictionary<string, string>();
        switch (StateController.test)
        {
            case 0:
                results = GetShapeMatchContent();
                break;
            case 1:
                results = GetRotationMatchContent();
                break;
            case 2:
                results = GetPoseMatchContent();
                break;
            default:
                break;
        }
        foreach (KeyValuePair<string, string> entry in results)
            survey_node.Add(entry.Key, entry.Value);

        // Write out JSON with new test parameters and performance
        File.WriteAllText(Path.Combine(StateController.Datapath, StateController.Filename), node.ToString());

        // Set test parameters
        StateController.SetTestParameters();
    }

    public Dictionary<string, string> GetShapeMatchContent()
    {
        float confidance = ShapeConfidance.value;
        string behaviour = "N/A";

        // May have several selected toggles
        foreach (Toggle toggle in ShapeBehaviour.ActiveToggles())
            behaviour = toggle.name;

        // Build dictionary that will populate json file
        Dictionary<string, string> result = new Dictionary<string, string>();
        result.Add("confidance", confidance.ToString());
        result.Add("behaviour", behaviour);
        return result;
    }

    public Dictionary<string, string> GetRotationMatchContent()
    {
        float confidance = RotateConfidance.value;
        string behaviour = "N/A";

        // May have several selected toggles
        foreach (Toggle toggle in RotateBehaviour.ActiveToggles())
            behaviour = toggle.name;

        // Build dictionary that will populate json file
        Dictionary<string, string> result = new Dictionary<string, string>();
        result.Add("confidance", confidance.ToString());
        result.Add("behaviour", behaviour);
        return result;
    }

    public Dictionary<string, string> GetPoseMatchContent()
    {
        float confidance = PoseConfidance.value;

        // Build dictionary that will populate json file
        Dictionary<string, string> result = new Dictionary<string, string>();
        result.Add("confidance", confidance.ToString());
        return result;
    }
}
