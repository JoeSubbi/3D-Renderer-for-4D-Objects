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
    public static float limit = 30;

    // Update is called once per frame
    void Update()
    {
        if (RepIntro.activeSelf || TestIntro.activeSelf)
        {
            StateController.start_time = Time.time;
            timer.text = FormatTime(limit);
            running = false;
        }
        else
        {
            float time = limit - (Time.time - StateController.start_time);
            timer.text = FormatTime(time);
            TimeLimit(time);
            running = true;
        }
    }

    private void TimeLimit(float time)
    {
        if (time <= 0)
            transform.parent.GetComponent<Button>().onClick.Invoke();
    }

    private string FormatTime(float time)
    {
        int h = (int)(time * 100) % 100;
        int s = (int)time % 60;
        int m = (int)(time % 3600) / 60;
        return string.Format("{0:00}:{1:00}.{2:00}", m, s, h);
    }
}
