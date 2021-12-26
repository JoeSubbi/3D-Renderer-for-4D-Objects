using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;
using System.Linq;

public class StateInitialiser : MonoBehaviour
{
    // Attached to first scene - Start Menu
    // Loads the datapath and other parameters 
    // of the static class - StateController
    void Start()
    {
        StateController.Initialise();
    }
}
