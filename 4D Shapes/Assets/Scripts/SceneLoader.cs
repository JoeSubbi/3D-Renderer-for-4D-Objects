using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string next_scene;

    public void LoadScene()
    {
        SceneManager.LoadScene(next_scene);
    }

    public void SaveTest()
    {
        StateController.SaveTest();
    }
}
