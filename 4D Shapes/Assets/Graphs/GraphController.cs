using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;
using System.Linq;

public class GraphController : MonoBehaviour
{
    public GameObject OptionResults;
    public GameObject PoseResults;

    // Start is called before the first frame update
    void Awake()
    {
        // Set up JSON Node
        JSONNode node;
        //using (StreamReader r = new StreamReader(Path.Combine(StateController.Datapath, StateController.Filename)))
        using (StreamReader r = new StreamReader("Assets/TestOutput/1.json"))
        {
            //read in the json
            var json = r.ReadToEnd();

            //reformat the json into dictionary style convention
            node = JSON.Parse(json);
        }

        // Loop through every representation
        foreach (KeyValuePair<string, JSONNode> representation in node)
        {
            string representationObject;
            
            // Relate representation to barchart bar gameobjects
            switch (representation.Key)
            {
                case "Multi-View":
                    representationObject = "Multi";
                    break;
                case "Control":
                    representationObject = "Control";
                    break;
                case "Timeline":
                    representationObject = "Timeline";
                    break;
                case "4D-3D":
                    representationObject = "3-4";
                    break;
                default:
                    representationObject = "";
                    break;
            }

            float shape_match = 0;
            foreach (KeyValuePair<string, JSONNode> test in representation.Value["Shape_Match"])
            {
                if ( !test.Key.Contains("_Survey"))
                {
                    if (test.Value["Loaded Shape"] == test.Value["Selected Shape"])
                    {
                        shape_match++;
                    }
                }
            }

            float rotate_match = 0;
            foreach (KeyValuePair<string, JSONNode> test in representation.Value["Rotation_Match"])
            {
                if (!test.Key.Contains("_Survey"))
                {
                    for (int i = 0; i < 6; i++)
                    {
                        if ((bool)test.Value["Loaded Rotation"][i] == (bool)test.Value["Selected Rotation"][i])
                            rotate_match ++;
                    }
                }
            }

            float pose_match = 0;
            foreach (KeyValuePair<string, JSONNode> test in representation.Value["Pose_Match"])
            {
                if (!test.Key.Contains("_Survey"))
                {
                    pose_match += ( test.Value["Accuracy"] / 3.14159f ) * 360;
                }
            }
            pose_match /= 3;

            // Set the heights of the bar charts
            OptionResults.transform.Find(representationObject).gameObject
                .transform.Find(representationObject + "-S").gameObject
                .GetComponent<RectTransform>().sizeDelta = new Vector2(30, shape_match * 66); 
            
            OptionResults.transform.Find(representationObject).gameObject
                .transform.Find(representationObject + "-R").gameObject
                .GetComponent<RectTransform>().sizeDelta = new Vector2(30, rotate_match * 11f);
            
            PoseResults.transform.Find(representationObject).gameObject
                .transform.Find(representationObject + "-P").gameObject
                .GetComponent<RectTransform>().sizeDelta = new Vector2(60, pose_match * 33/18);
        }
    }

}
