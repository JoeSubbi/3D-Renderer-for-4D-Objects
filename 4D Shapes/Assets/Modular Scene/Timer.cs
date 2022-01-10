using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timer;

    // Update is called once per frame
    void Update()
    {
        float time = Time.time - StateController.start_time;
        int h = (int)(time * 100) % 100;
        int s = (int)time % 60;
        int m = (int)(time % 3600) / 60;
        timer.text = string.Format("{0:00}:{1:00}.{2:00}", m, s, h);
    }
}
