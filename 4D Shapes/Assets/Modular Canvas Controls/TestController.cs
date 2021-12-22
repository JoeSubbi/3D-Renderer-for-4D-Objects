using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;
using System.Linq;

public class TestController : MonoBehaviour
{
    // External Triggers to load stuff between tests
    public bool progression_graph = false;
    public bool trigger = false;

    // Test Results
    public double accuracy = 0.0;     // Accuracy of user
    public double time = 0.0;         // Time to complete the task

    // Start is called before the first frame update
    void Start()
    {
        // Initialise Experiment
        StateController.PopulateDictionaries();
        StateController.ShuffleRepresentations();
        StateController.BuildDatapath();

        // Build file
        BuildJsonTemplate();

        // Load JSON file
        JSONNode node;
        using (StreamReader r = new StreamReader(Path.Combine(StateController.Datapath, StateController.Filename)))
        {
            //read in the json
            var json = r.ReadToEnd();

            //reformat the json into dictionary style convention
            node = JSON.Parse(json);
        }
    }

    private void BuildJsonTemplate()
    {
        File.WriteAllText(StateController.Datapath + StateController.Filename,
            @"{
    ""Control"":{
        ""Shape_Match"":{},
        ""Rotation_Match"":{},
        ""Pose_Match"":{}
    },
    ""Timeline"":{
        ""Shape_Match"":{},
        ""Rotation_Match"":{},
        ""Pose_Match"":{}
    },
    ""Multi-View"":{
        ""Shape_Match"":{},
        ""Rotation_Match"":{},
        ""Pose_Match"":{}
    },
    ""4D-3D"":{
        ""Shape_Match"":{},
        ""Rotation_Match"":{},
        ""Pose_Match"":{}
    }
}"
        );
    }
    
    void Update()
    {
        // Trigger - a change should occur, because the user has triggered the next stage
        if (trigger)
        {
            // Set Representation
            int current_representation = StateController.rep_order[StateController.rep_index];
            Debug.Log(StateController.representations[current_representation]);

            // Load Test
            switch (StateController.test)
            {
                case 0:
                    ShapeMatch();
                    break;
                case 1:
                    RotationMatch();
                    break;
                case 2:
                    PoseMatch();
                    break;
                case 3:
                    EndRepresentation();
                    break;
                default:
                    break;
            }
        }
        trigger = false;
    }

    /* JSON STUFF
    // Load JSON file
    JSONNode node;
    using (StreamReader r = new StreamReader(Path.Combine(datapath, filename)))
    {
        //read in the json
        var json = r.ReadToEnd();

        //reformat the json into dictionary style convention
        node = JSON.Parse(json);
    }
    File.WriteAllText(datapath + filename, node.ToString());
    */

    /*
    {
        ""Shape"": 0,
        ""Rotation"":[0,0,0,0,0,0],
        ""Texture"": 0,
        ""Time"": 0,
        ""Accuracy"": 0
    },
    */

    private void Save(double time, double accuracy)
    {
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

        // Name Test
        string test_name_id = test_name + StateController.test_count;

        /*
        Dictionary<string, int> str_int_test = new Dictionary<string, int>();
        str_int_test.Add("Shape", shape);
        str_int_test.Add("Texture", texture);

        Dictionary<string, double> str_flo_test = new Dictionary<string, double>();
        str_flo_test.Add("Time", time); 
        str_flo_test.Add("Accuracy", accuracy);

        Dictionary<string, bool[6]> rot_test = new Dictionary<string, bool[6]>();
        rot_test.Add("Rotation", rotation);
        */
        JSONNode temp = JSON.Parse("{}");
        node[rep_name][test_name].Add(test_name_id, temp);

        JSONNode test_node = node[rep_name][test_name][test_name_id];
        test_node.Add("Shape", StateController.shape);
        test_node.Add("Texture", StateController.texture);
        test_node.Add("Time", time);
        test_node.Add("Accuracy", accuracy);
        //JSONArray rot_json = rotation;
        //node["Control"][test_name][-1]["Rotation"].put(rot_json);

        Debug.Log(node[rep_name].ToString());

        File.WriteAllText(StateController.Datapath + StateController.Filename, node.ToString());
    }

    void ShapeMatch()
    {
        // Set up shape match
        Debug.Log("Shape Match " + StateController.test_count);
        StateController.test_count += 1;

        // Set shape
        // Set texture
        // Set Rotation (if test_count > 5)

        // call function from ObjectController to set random orientation
        // call function from ObjectController to set random W

        // Load quiz
        Save(0.1, 0.1);

        // If end of pose match tests, move on to next test
        if (StateController.test_count == StateController.MAX_TESTS)
        {
            StateController.test += 1;
            StateController.test_count = 0;
        }
    }

    void ShapeMatchQuiz()
    {

    }

    void RotationMatch()
    {
        // Set up shape match
        Debug.Log("Rotation Match " + StateController.test_count);
        StateController.test_count += 1;

        // Set shape
        // Set texture
        // Set Rotation

        // Load quiz

        // If end of pose match tests, move on to next test
        if (StateController.test_count == StateController.MAX_TESTS)
        {
            StateController.test += 1;
            StateController.test_count = 0;
        }
    }

    void RotationMatchQuiz()
    {

    }

    void PoseMatch()
    {
        // Set up pose match
        Debug.Log("Pose Match " + StateController.test_count);
        StateController.test_count += 1;

        // Set shape
        // Set texture

        // call function from ObjectController to set rotation of randomly posed shape

        // Load quiz

        // If end of pose match tests, move on to next test
        if (StateController.test_count == StateController.MAX_TESTS)
        {
            StateController.test += 1;
            StateController.test_count = 0;
        }
    }

    void PoseMatchQuiz()
    {

    }

    void EndRepresentation()
    {
        // If there are still representations left to do, 
        // move to the next represention
        // Reset test to start from shape match again
        if (StateController.rep_index != StateController.rep_order.Length - 1)
        {
            StateController.rep_index += 1;
            StateController.test = 0;
            Debug.Log("End of Representation");
        }
        else
        {
            //trigger end event
            Debug.Log("End!");
        }

        // Go to graph: progression_graph = true;
    }
}
