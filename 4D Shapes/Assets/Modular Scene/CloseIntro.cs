using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseIntro : MonoBehaviour
{
    public GameObject TaskIntro;

    public void Next()
    {
        StateController.start_time = Time.time;
        transform.parent.gameObject.SetActive(false);

        TaskIntro.SetActive(true);
    }

    public void Begin()
    {
        StateController.start_time = Time.time;
        transform.parent.gameObject.SetActive(false);
    }
}
