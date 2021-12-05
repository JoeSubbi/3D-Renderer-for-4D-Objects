using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectController : MonoBehaviour
{
    public GameObject main;
    public GameObject mini;
    public GameObject match;

    private Renderer mainRenderer;
    private Renderer miniRenderer;
    private Renderer matchRenderer;

    public int shape;
    public int effect;
    public Slider wSlider;

    //public Rotor4 matchRot;
    //public Rotor4 mainRot;

    // Start is called before the first frame update
    void Start()
    {
        mainRenderer = main.GetComponent<Renderer>();
        miniRenderer = mini.GetComponent<Renderer>();
        matchRenderer = match.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Set object W position
        mainRenderer.material.SetFloat("_W", wSlider.value);

        // Set object shape
        mainRenderer.material.SetInt("_Shape", shape);
        miniRenderer.material.SetInt("_Shape", shape);
        matchRenderer.material.SetInt("_Shape", shape);

        // Set object texture
        mainRenderer.material.SetInt("_Effect", effect);
        matchRenderer.material.SetInt("_Effect", effect);

        // Set object rotation
    }
}
