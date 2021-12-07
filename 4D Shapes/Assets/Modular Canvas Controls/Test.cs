using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;
using System.Linq;

public class Test : MonoBehaviour
{
    // Datapath
    private string datapath;
    private string filename;

    // Experiment Properties
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
    private int test    = 0;
    private int texture = 0;
    private int shape   = 0;
    // Array of the order representations will occur in
    private int[] repOrder = new int[] { 0, 1, 2, 3 };

    // Rotation
    // Boolean Array for rotation in YZ, XZ, XY, XW, YW, ZW
    private bool[] rotation = new bool[] { false, false, false, false, false, false };

    // Experiment Status
    private int rep_index  = 0;
    private int test_count = 0;
    private int MAX_TESTS  = 3;

    // External Triggers to load stuff between tests
    public bool progression_graph = false;
    public bool trigger = false;

    // Dictionaries to easily convert IDs to JSON Keys
    private Dictionary<int, string> representations = new Dictionary<int, string>();
    private Dictionary<int, string> tests = new Dictionary<int, string>();

    // Start is called before the first frame update
    void Start()
    {
        // Initialise Experiment
        representations.Add(0, "Control");
        representations.Add(1, "Multi-View");
        representations.Add(2, "Timeline");
        representations.Add(3, "4D-3D");

        tests.Add(0, "Shape_Match");
        tests.Add(1, "Rotation_Match");
        tests.Add(2, "Pose_Match");

        // Shuffle representation order
        for(int i=0; i<repOrder.Length -1; i++)
        {
            int j = Random.Range(0, repOrder.Length);
            int temp = repOrder[i];
            repOrder[i] = repOrder[j];
            repOrder[j] = temp;
        }

        // Prepare datapath for test results
        if (Application.isEditor)
            datapath = "Assets/TestOutput/";
        else
            datapath = Application.persistentDataPath;

        // Check for and or create persistent datapath
        DirectoryInfo dir = CreateDataPath();

        // Read existing file names to see who the next user is
        int id = GetLastUser(dir)+1;

        // Create File
        FileStream file = CreateFile(id);
        file.Close();

        // Build file
        BuildJsonTemplate();

        // Load JSON file
        JSONNode node;
        using (StreamReader r = new StreamReader(Path.Combine(datapath, filename)))
        {
            //read in the json
            var json = r.ReadToEnd();

            //reformat the json into dictionary style convention
            node = JSON.Parse(json);
        }
    }

    private DirectoryInfo CreateDataPath()
    {
        if (!Directory.Exists(datapath))
            return Directory.CreateDirectory(datapath);
        return new DirectoryInfo(datapath);
    }

    private int GetLastUser(DirectoryInfo dir)
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
    
    private FileStream CreateFile(int id)
    {
        filename = id + ".json";
        string file = Path.Combine(datapath, filename);
        return File.Create(file);
    }

    private void BuildJsonTemplate()
    {
        File.WriteAllText(datapath + filename,
            @"{
    ""Control"":{
        ""Shape_Match"":{
            ""Shape"": 0,
            ""Rotation"":[0,0,0,0,0,0],
            ""Texture"": 0,
            ""Time"": 0,
            ""Accuracy"": 0
        },
        ""Rotation_Match"":{
            ""Shape"": 0,
            ""Rotation"":[0,0,0,0,0,0],
            ""Texture"": 0,
            ""Time"": 0,
            ""Accuracy"": 0
        },
        ""Pose_Match"":{
            ""Shape"": 0,
            ""Rotation"":[0,0,0,0,0,0],
            ""Texture"": 0,
            ""Time"": 0,
            ""Accuracy"": 0
        }
    },
    ""Timeline"":{
        ""Shape_Match"":{
            ""Shape"": 0,
            ""Rotation"":[0,0,0,0,0,0],
            ""Texture"": 0,
            ""Time"": 0,
            ""Accuracy"": 0
        },
        ""Rotation_Match"":{
            ""Shape"": 0,
            ""Rotation"":[0,0,0,0,0,0],
            ""Texture"": 0,
            ""Time"": 0,
            ""Accuracy"": 0
        },
        ""Pose_Match"":{
            ""Shape"": 0,
            ""Rotation"":[0,0,0,0,0,0],
            ""Texture"": 0,
            ""Time"": 0,
            ""Accuracy"": 0
        }
    },
    ""Multi-View"":{
        ""Shape_Match"":{
            ""Shape"": 0,
            ""Rotation"":[0,0,0,0,0,0],
            ""Texture"": 0,
            ""Time"": 0,
            ""Accuracy"": 0
        },
        ""Rotation_Match"":{
            ""Shape"": 0,
            ""Rotation"":[0,0,0,0,0,0],
            ""Texture"": 0,
            ""Time"": 0,
            ""Accuracy"": 0
        },
        ""Pose_Match"":{
            ""Shape"": 0,
            ""Rotation"":[0,0,0,0,0,0],
            ""Texture"": 0,
            ""Time"": 0,
            ""Accuracy"": 0
        }
    },
    ""4D-3D"":{
        ""Shape_Match"":{
            ""Shape"": 0,
            ""Rotation"":[0,0,0,0,0,0],
            ""Texture"": 0,
            ""Time"": 0,
            ""Accuracy"": 0
        },
        ""Rotation_Match"":{
            ""Shape"": 0,
            ""Rotation"":[0,0,0,0,0,0],
            ""Texture"": 0,
            ""Time"": 0,
            ""Accuracy"": 0
        },
        ""Pose_Match"":{
            ""Shape"": 0,
            ""Rotation"":[0,0,0,0,0,0],
            ""Texture"": 0,
            ""Time"": 0,
            ""Accuracy"": 0
        }
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
            int current_representation = repOrder[rep_index];
            Debug.Log(representations[current_representation]);

            // Load Test
            switch (test)
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
    Debug.Log(node["Control"]["Shape_Match"]["Rotation"]);
    File.WriteAllText(datapath + filename, node.ToString());
    */

    void ShapeMatch()
    {
        // Set up shape match
        Debug.Log("Shape Match " + test_count);
        test_count += 1;

        // Set shape
        // Set texture
        // Set Rotation (if test_count > 5)

        // call function from ObjectController to set random orientation
        // call function from ObjectController to set random W

        // Load quiz

        // If end of pose match tests, move on to next test
        if (test_count == MAX_TESTS)
        {
            test += 1;
            test_count = 0;
        }
    }

    void ShapeMatchQuiz()
    {

    }

    void RotationMatch()
    {
        // Set up shape match
        Debug.Log("Rotation Match " + test_count);
        test_count += 1;

        // Set shape
        // Set texture
        // Set Rotation

        // Load quiz

        // If end of pose match tests, move on to next test
        if (test_count == MAX_TESTS)
        {
            test += 1;
            test_count = 0;
        }
    }

    void RotationMatchQuiz()
    {

    }

    void PoseMatch()
    {
        // Set up pose match
        Debug.Log("Pose Match " + test_count);
        test_count += 1;

        // Set shape
        // Set texture

        // call function from ObjectController to set rotation of randomly posed shape

        // Load quiz

        // If end of pose match tests, move on to next test
        if (test_count == MAX_TESTS)
        {
            test += 1;
            test_count = 0;
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
        if (rep_index != repOrder.Length - 1)
        {
            rep_index += 1;
            test = 0;
            Debug.Log("End of Representation");
        }
        else
        {
            //trigger end event
            Debug.Log("End!");
        }

        // Go to graph: progression_graph = true;
    }

    // Functions for external classes to access information
    public int GetShape()
    {
        return shape;
    }

    public int GetTexture()
    {
        return texture;
    }

    public bool[] GetRotation()
    {
        return rotation;
    }
}
