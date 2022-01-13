using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotate : MonoBehaviour
{
    private Renderer rend;
    private Rotor4 RandomRotor = new Rotor4(1,0,0,0,0,0,0,0);
    private Rotor4 RandomU = new Rotor4(1, 0, 0, 0, 0, 0, 0, 0);
    private Material matchMat;
    private Vector4 rand;

    // Start is called before the first frame update
    void Start()
    {
        RandomRotor = new Rotor4(1, Random.Range(0, 6.28318f), Random.Range(0, 6.28318f), Random.Range(0, 6.28318f),
                                 0,0,0,0);
        RandomU = RandomRotor;
        RandomRotor.Normalise();

        rend = GetComponent<Renderer>();

        rend.material.SetFloat("_A",  RandomRotor.a);
        rend.material.SetFloat("_YZ", RandomRotor.byz);
        rend.material.SetFloat("_XZ", RandomRotor.bxz);
        rend.material.SetFloat("_XY", RandomRotor.bxy);
        rend.material.SetFloat("_XW", RandomRotor.bxw);
        rend.material.SetFloat("_YW", RandomRotor.byw);
        rend.material.SetFloat("_ZW", RandomRotor.bzw);

        matchMat = GameObject.Find("Right").GetComponent<Renderer>().material;
        rand = new Vector4(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f));
    }

    // Update is called once per frame
    void Update()
    {
        Vector4 swipeRand = SwipeRotation.total.Rotate(rand);
        Vector4 randoRand = RandomRotor.Rotate(rand);
        float a = Mathf.Acos((Vector4.Dot(swipeRand, randoRand)) / (swipeRand.magnitude * randoRand.magnitude));
        Debug.Log(a);

        if (range(RandomRotor.a, SwipeRotation.total.a, 0.1f) &&
            range(RandomRotor.byz, SwipeRotation.total.byz, 0.1f) &&
            range(RandomRotor.bxz, SwipeRotation.total.bxz, 0.1f) &&
            range(RandomRotor.bxy, SwipeRotation.total.bxy, 0.1f) &&
            range(RandomRotor.bxw, SwipeRotation.total.bxw, 0.1f) &&
            range(RandomRotor.byw, SwipeRotation.total.byw, 0.1f) &&
            range(RandomRotor.bzw, SwipeRotation.total.bzw, 0.1f))
            Debug.Log("True");
    }

    private bool range(float a, float b, float epsilon)
    {
        if (Mathf.Abs(a) - Mathf.Abs(b) <= epsilon) return true;
        return false;
    }
}
