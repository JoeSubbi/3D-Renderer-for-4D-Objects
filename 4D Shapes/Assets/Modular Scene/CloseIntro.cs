using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseIntro : MonoBehaviour
{
    public void Begin()
    {
        StateController.time = 0;
        transform.parent.gameObject.SetActive(false);
    }
}
