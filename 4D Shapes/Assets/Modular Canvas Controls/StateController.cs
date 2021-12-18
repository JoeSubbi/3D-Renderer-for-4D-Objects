using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public static int test = 0;       // current experiment test
    // Array of the order representations will occur in
    public static int[] repOrder = new int[] { 0, 1, 2, 3 };

    // Experiment Status
    public static int rep_index = 0;  // Index of repOrder - which representation is being shown
    public static int test_count = 0; // How many iterations of a specific test
    public const int MAX_TESTS = 3;  // Total number of iterations for each test

    // Dictionaries to easily convert IDs to JSON Keys
    public static Dictionary<int, string> representations = new Dictionary<int, string>();
    public static Dictionary<int, string> tests = new Dictionary<int, string>();

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

    public static void ShuffleRepresentations()
    {
        // Shuffle representation order
        for (int i = 0; i < repOrder.Length - 1; i++)
        {
            int j = Random.Range(0, repOrder.Length);
            int temp = repOrder[i];
            repOrder[i] = repOrder[j];
            repOrder[j] = temp;
        }
    }

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

}
