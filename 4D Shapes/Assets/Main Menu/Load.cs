using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Load : MonoBehaviour
{
    public string scene;
    private bool drag = true;

    public void Loader()
    {
        if (!drag)
        SceneManager.LoadScene(scene);
    }

    void OnGUI()
    {
        Event e = Event.current;
        if (e.isMouse)
        {
            if (e.delta.x == 0) drag = false;
            else drag = true;
        }
    }
}
