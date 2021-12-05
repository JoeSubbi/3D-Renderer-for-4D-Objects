using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;

public class Test : MonoBehaviour
{
    // Array of the order representations will occur in

    // Datapath
    private string datapath;
    private string filename;

    // Start is called before the first frame update
    void Start()
    {
        if (Application.isEditor)
            datapath = "Assets/TestOutput/";
        else
            datapath = Application.persistentDataPath;

        // Initial Prep
        // Determine Representation Order

        // Control
        // 4D to 3D
        // Timeline
        // Multi-View

        // Check for and or create persistent datapath
        DirectoryInfo dir = CreateDataPath();

        // Read existing file names to see who the next user is
        int id = GetLastUser(dir)+1;

        // Create File
        FileStream file = CreateFile(id);
        Debug.Log("Created " + filename);
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

        Debug.Log(node["Control"]["Shape_Match"]["Shape"]);
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
            ""Texture"": 0,
            ""Time"": 0,
            ""Accuracy"": 0
        },
        ""Rotation_Match"":{
            ""Shape"": 0,
            ""Texture"": 0,
            ""Time"": 0,
            ""Accuracy"": 0
        },
        ""Pose_Match"":{
            ""Shape"": 0,
            ""Texture"": 0,
            ""Time"": 0,
            ""Accuracy"": 0
        }
    },
    ""Timeline"":{
        ""Shape_Match"":{
            ""Shape"": 0,
            ""Texture"": 0,
            ""Time"": 0,
            ""Accuracy"": 0
        },
        ""Rotation_Match"":{
            ""Shape"": 0,
            ""Texture"": 0,
            ""Time"": 0,
            ""Accuracy"": 0
        },
        ""Pose_Match"":{
            ""Shape"": 0,
            ""Texture"": 0,
            ""Time"": 0,
            ""Accuracy"": 0
        }
    },
    ""Multi-View"":{
        ""Shape_Match"":{
            ""Shape"": 0,
            ""Texture"": 0,
            ""Time"": 0,
            ""Accuracy"": 0
        },
        ""Rotation_Match"":{
            ""Shape"": 0,
            ""Texture"": 0,
            ""Time"": 0,
            ""Accuracy"": 0
        },
        ""Pose_Match"":{
            ""Shape"": 0,
            ""Texture"": 0,
            ""Time"": 0,
            ""Accuracy"": 0
        }
    },
    ""4D-3D"":{
        ""Shape_Match"":{
            ""Shape"": 0,
            ""Texture"": 0,
            ""Time"": 0,
            ""Accuracy"": 0
        },
        ""Rotation_Match"":{
            ""Shape"": 0,
            ""Texture"": 0,
            ""Time"": 0,
            ""Accuracy"": 0
        },
        ""Pose_Match"":{
            ""Shape"": 0,
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

    }

    void ShapeMatch()
    {

    }

    void ShapeMatchQuiz()
    {

    }

    void RotationMatch()
    {

    }

    void RotationMatchQuiz()
    {

    }

    void PoseMatch()
    {

    }

    void PoseMatchQuiz()
    {

    }
}
