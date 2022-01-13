using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timer;
    public GameObject RepIntro;
    public GameObject TestIntro;
    public static bool running = false;

    // Update is called once per frame
    void Update()
    {
        if (RepIntro.activeSelf || TestIntro.activeSelf)
        {
            StateController.start_time = Time.time;
            timer.text = string.Format("00:00:00");
            running = false;
        }
        else
        {
            float time = Time.time - StateController.start_time;
            int h = (int)(time * 100) % 100;
            int s = (int)time % 60;
            int m = (int)(time % 3600) / 60;
            timer.text = string.Format("{0:00}:{1:00}.{2:00}", m, s, h);
            running = true;
        }
        
    }
}
