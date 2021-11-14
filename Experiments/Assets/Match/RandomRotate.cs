using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotate : MonoBehaviour
{
    private Renderer rend;
    private Rotor4 RandomRotor = new Rotor4(1,0,0,0,0,0,0);
    private Material matchMat;

    // Start is called before the first frame update
    void Start()
    {
        RandomRotor = new Rotor4(1, Random.Range(0, 6.28318f), Random.Range(0, 6.28318f), Random.Range(0, 6.28318f),
                                 0,0,0);
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
    }

    // Update is called once per frame
    void Update()
    {
        float a = matchMat.GetFloat("_A");
        float yz = matchMat.GetFloat("_YZ");
        float xz = matchMat.GetFloat("_XZ");
        float xy = matchMat.GetFloat("_XY");
        float xw = matchMat.GetFloat("_XW");
        float yw = matchMat.GetFloat("_YW");
        float zw = matchMat.GetFloat("_ZW");

        if (range(RandomRotor.a, a, 0.1f) &&
            range(RandomRotor.byz, yz, 0.1f) &&
            range(RandomRotor.bxz, xz, 0.1f) &&
            range(RandomRotor.bxy, xy, 0.1f) &&
            range(RandomRotor.bxw, xw, 0.1f) &&
            range(RandomRotor.byw, yw, 0.1f) &&
            range(RandomRotor.bzw, zw, 0.1f))
            Debug.Log("True");
    }

    private bool range(float a, float b, float epsilon)
    {
        if (Mathf.Abs(a) - Mathf.Abs(b) <= epsilon) return true;
        return false;
    }
}
