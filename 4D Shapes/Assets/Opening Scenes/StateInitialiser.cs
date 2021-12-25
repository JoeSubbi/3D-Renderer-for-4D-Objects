using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;
using System.Linq;

public class StateInitialiser : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Initialise Experiment
        StateController.PopulateDictionaries();
        StateController.ShuffleRepresentations();
        StateController.BuildDatapath();

        // Build file
        BuildJsonTemplate();
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

}
