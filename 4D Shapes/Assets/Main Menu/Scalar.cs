using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scalar : MonoBehaviour
{
    public RectTransform[] panels;
    
    // Update is called once per frame
    void Update()
    {
        Vector2 canvasSize = GameObject.Find("Canvas").GetComponent<RectTransform>().sizeDelta;
        foreach (RectTransform panel in panels)
        {
            float pos = Mathf.Abs(panel.position.x - canvasSize.x/2)+1;
            if (pos > 400)
            {
                pos = 400;
            }
            float x = 620-(pos*0.620f);
            float y = 360-(pos*0.360f);
            panel.sizeDelta = new Vector2(x, y);
        }

    }
}
