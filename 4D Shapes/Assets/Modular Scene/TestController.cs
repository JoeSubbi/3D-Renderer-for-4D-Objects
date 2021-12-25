using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using System.IO;
using System.Linq;

public class TestController : MonoBehaviour
{
    // StateController requirements that need to be set externally
    public string ModularScene;
    public Canvas ModularSceneCanvas;

    // External Triggers to load stuff between tests
    public bool progression_graph = false;
    public bool trigger = false;

    // Test Results
    public double accuracy = 0.0;     // Accuracy of user
    public double time = 0.0;         // Time to complete the task


    // Start is called before the first frame update
    void Start()
    {
        // Initialise StateController parameters
        StateController.ModularScene = ModularScene;
        StateController.ModularSceneCanvas = ModularSceneCanvas;

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
        //Save(0.1, 0.1);

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
