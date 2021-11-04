using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public RectTransform container;
    public int speed = 1;
    private float x_speed = 0f;
    private float pos = -2f;


    public GameObject background;
    private Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = background.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        rend.material.SetFloat("_W", pos+3);
    }

    void OnGUI()
    {
        Event e = Event.current;
        if (e.isMouse)
        {
            x_speed = e.delta.x * speed;
            Vector2 canvasSize = GameObject.Find("Canvas").GetComponent<RectTransform>().sizeDelta;

            if (Input.GetMouseButton(0))
            {
                //if (container.position.x <= canvasSize.x / 2 && x_speed > 0)
                {
                    container.Translate(x_speed, 0, 0);
                    pos += ((x_speed / 3100) * 5);
                }
            }
        }
    }
}