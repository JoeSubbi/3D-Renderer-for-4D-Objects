﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SimpleJSON;
using System.IO;
using System.Linq;

public static class StateController
{
    // Datapath
    public static string Datapath;
    public static string Filename;

    // Experiment State Properties
    /* Representations
     * 0 - Control
     * 1 - Multi-View
     * 2 - Timeline
     * 3 - 4D-3D
     * 
     * Tests
     * 0 - Shape_Match
     * 1 - Rotation_Match
     * 2 - Pose_Match
     * 
     * Texture
     * 0 - Diffuse Lighting
     * 1 - RGBW
     * 2 - Pattern Red+ Blue-
     * 3 - RGBW Pattern
     * 
     * Shape
     * 0 - Sphere
     * 1 - Cube
     * 2 - WCone
     * 3 - YCone
     * 4 - Torus
     * 5 - WCapsule
     * 6 - XCapsule
     * 7 - Tetrahedron
     */

    // Array of the order representations will occur in
    public static int[] rep_order = new int[] { 0, 1, 2, 3 };
    // Experiment Status
    public static int test = 1;       // current experiment test
    public static int rep_index = 0;  // Index of rep_order - used to index rep_order
    public static int test_count = 0; // How many iterations of a specific test
    public const int MAX_TESTS = 3;   // Total number of iterations for each test

    // Dictionaries to easily convert IDs to JSON Keys
    public static Dictionary<int, string> representations = new Dictionary<int, string>();
    public static Dictionary<int, string> tests = new Dictionary<int, string>();

    // Test Parameters
    public static int shape = 0;      // Current shape
    public static int texture = 0;    // Current enabled texture
    // Boolean Array for planes of constant rotation in YZ, XZ, XY, XW, YW, ZW
    public static bool[] rotations = new bool[] { false, false, false, false, false, false };
    // Test Performance
    private static ulong time = 0;

    // Externally set parameters
    public static Canvas ModularSceneCanvas;



    // Populate dictionaries to easily convert IDs to JSON keys
    public static void PopulateDictionaries()
    {
        representations.Add(0, "Control");
        representations.Add(1, "Multi-View");
        representations.Add(2, "Timeline");
        representations.Add(3, "4D-3D");

        tests.Add(0, "Shape_Match");
        tests.Add(1, "Rotation_Match");
        tests.Add(2, "Pose_Match");
    }

    // Shuffle order the representations will occur in
    public static void ShuffleRepresentations()
    {
        for (int i = 0; i < rep_order.Length - 1; i++)
        {
            int j = Random.Range(0, rep_order.Length);
            int temp = rep_order[i];
            rep_order[i] = rep_order[j];
            rep_order[j] = temp;
        }
    }

    // Set up datapath to test results file based on test ID
    public static void BuildDatapath()
    {
        // Prepare datapath for test results
        if (Application.isEditor)
            Datapath = "Assets/TestOutput/";
        else
            Datapath = Application.persistentDataPath;

        // Check for and or create persistent datapath
        DirectoryInfo dir = CreateDataPath();

        // Read existing file names to see who the next user is
        int id = GetLastUser(dir) + 1;

        // Create File
        FileStream file = CreateFile(id);
        file.Close();
    }

    // Datapth and file helper functions
    private static DirectoryInfo CreateDataPath()
    {
        if (!Directory.Exists(Datapath))
            return Directory.CreateDirectory(Datapath);
        return new DirectoryInfo(Datapath);
    }

    private static int GetLastUser(DirectoryInfo dir)
    {
        if (dir.GetFiles("*.json").Length == 0)
            return 0;

        int id = 0;
        foreach (FileInfo file in dir.GetFiles("*.json"))
        {
            int id_temp = int.Parse(file.Name.Split('.')[0]);
            if (id_temp > id) id = id_temp;
        }
        return id;
    }

    private static FileStream CreateFile(int id)
    {
        Filename = id + ".json";
        string file = Path.Combine(Datapath, Filename);
        return File.Create(file);
    }

    private static void BuildJsonTemplate()
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

    // Initialise State Controller
    public static void Initialise()
    {
        // Initialise Experiment
        StateController.PopulateDictionaries();
        StateController.ShuffleRepresentations();
        StateController.BuildDatapath();

        // Build file
        BuildJsonTemplate();
    }


    // Trigger - Submit
    // Bring user from test to survey page
    public static void SaveTest()
    {
        // Check datapath was initialised
        if (Datapath == null)
            Initialise();

        // Check type of test to know what to save
        switch (test)
        {
            case 0:
                SaveShapeMatchTest();
                break;
            case 1:
                SaveRotationMatchTest();
                break;
            case 2:
                SavePoseMatchTest();
                break;
            default:
                break;
        }
    }

    public static void SaveShapeMatchTest()
    {
        // Set up JSON Node
        JSONNode node;
        using (StreamReader r = new StreamReader(Path.Combine(Datapath, Filename)))
        {
            //read in the json
            var json = r.ReadToEnd();

            //reformat the json into dictionary style convention
            node = JSON.Parse(json);
        }

        // Get Representation
        string rep_name = representations[rep_index];
        // Get Test
        string test_name = tests[test];
        // Combine Test with iteration of test e.g ShapeMatch7
        string test_name_id = test_name + test_count;

        // Build Dictionary of test parameters and performance
        JSONNode temp = JSON.Parse("{}");
        node[rep_name][test_name].Add(test_name_id, temp);

        JSONNode test_node = node[rep_name][test_name][test_name_id];
        test_node.Add("Loaded Shape", shape);
        string selectedShape = ModularSceneCanvas.GetComponent<MultipleChoiceOptions>().LogShape();
        test_node.Add("Selected Shape", selectedShape);

        test_node.Add("Texture", texture);
        test_node.Add("Time", time);
        
        // Write out JSON with new test parameters and performance
        File.WriteAllText(Datapath + Filename, node.ToString());
    }

    public static void SaveRotationMatchTest()
    {
        // Set up JSON Node
        JSONNode node;
        using (StreamReader r = new StreamReader(Path.Combine(Datapath, Filename)))
        {
            //read in the json
            var json = r.ReadToEnd();

            //reformat the json into dictionary style convention
            node = JSON.Parse(json);
        }

        // Get Representation
        string rep_name = representations[rep_index];
        // Get Test
        string test_name = tests[test];
        // Combine Test with iteration of test e.g ShapeMatch7
        string test_name_id = test_name + test_count;

        // Build Dictionary of test parameters and performance
        JSONNode temp = JSON.Parse("{}");
        node[rep_name][test_name].Add(test_name_id, temp);

        JSONNode test_node = node[rep_name][test_name][test_name_id];
        test_node.Add("Shape", shape);
        test_node.Add("Texture", texture);

        test_node.Add("Loaded Rotation", JSON.Parse(RotationToString(rotations)) );
        bool[] rotation = ModularSceneCanvas.GetComponent<MultipleChoiceOptions>().LogRotation();
        test_node.Add("Selected Rotation", JSON.Parse(RotationToString(rotation)) );

        test_node.Add("Time", time);

        // Write out JSON with new test parameters and performance
        File.WriteAllText(Datapath + Filename, node.ToString());
    }

    private static string RotationToString(bool[] rotation)
    {
        return "[" + rotation[0] + "," + rotation[1] + "," + rotation[2] +
               "," + rotation[3] + "," + rotation[4] + "," + rotation[5] + "]";
    }

    public static void SavePoseMatchTest()
    {
        // Set up JSON Node
        JSONNode node;
        using (StreamReader r = new StreamReader(Path.Combine(Datapath, Filename)))
        {
            //read in the json
            var json = r.ReadToEnd();

            //reformat the json into dictionary style convention
            node = JSON.Parse(json);
        }

        // Get Representation
        string rep_name = representations[rep_index];
        // Get Test
        string test_name = tests[test];
        // Combine Test with iteration of test e.g ShapeMatch7
        string test_name_id = test_name + test_count;

        // Build Dictionary of test parameters and performance
        JSONNode temp = JSON.Parse("{}");
        node[rep_name][test_name].Add(test_name_id, temp);

        JSONNode test_node = node[rep_name][test_name][test_name_id];
        test_node.Add("Shape", shape);
        test_node.Add("Texture", texture);
        test_node.Add("Time", time);

        test_node.Add("Accuracy", 0);

        // Write out JSON with new test parameters and performance
        File.WriteAllText(Datapath + Filename, node.ToString());
    }

    // Trigger - Submit
    // Bring user form survey page to scene with a graph to show accuracy improvement
    public static void BuildGraphs()
    {
        // Build graph(s)?
    }

    // Trigger - Next
    // Bring user to next test
    public static void SetupTest()
    {
        // Set the object properties and prep UI properties
        // Shape Match
        if (test == 0)
        {
            shape = Random.Range(0, 7);
            texture = 0;
            ObjectController.w = Random.Range(-0.8f, 0.8f);
            ObjectController.SetRandMainObjectRotation();
        }
        // Rotation Match
        else if (test == 1)
        {
            shape = Random.Range(1, 7);
            texture = Random.Range(0, 3);

            // Set which planes to rotate about
            rotations = new bool[] {Random.value >= 0.3, Random.value >= 0.3,
                                    Random.value >= 0.3,
                                    Random.value >= 0.3, Random.value >= 0.3,
                                    Random.value >= 0.3
                                };
        }
        // Pose Match
        else if (test == 2)
        {
            shape = Random.Range(1, 7);
            texture = Random.Range(1, 3);

            ObjectController.SetRandMatchObjectRotation();
        }
        // Set object properties
        ObjectController.SetObjectShape(shape);
        ObjectController.SetObjectTexture(texture);

        // Activate/Deactivate UI elements based on the mode
        ModularSceneCanvas.GetComponent<UIController>().ModeUI();

        // Set the Representation
        ModularSceneCanvas.GetComponent<UIController>().SetRepresentation(rep_order[rep_index]);
    }

}
