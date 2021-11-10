using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcBall : MonoBehaviour
{
    private Vector3 e1 = new Vector3(0, 0, 0);
    private Vector3 e2 = new Vector3(0, 0, 0);
    public Rotor3 total = new Rotor3(1, 0, 0, 0);

    private Renderer image;
    private Vector3 hit_old = new Vector3(0, 0, 1);
    private Vector3 hit_new = new Vector3(0, 0, 1);


    // Start is called before the first frame update
    void Start()
    {
        image = GameObject.Find("Plane").GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        image.material.SetFloat("_A", total.a);
        image.material.SetFloat("_XY", total.b02);
        image.material.SetFloat("_XZ", total.b01);
        image.material.SetFloat("_YZ", total.b12);

        total.Normalise();

        if (Input.GetMouseButton(0))
        {
            hit_old = hit_new;
            CheckHit();

            Rotor3 r = Rotor3.GeoProd(hit_old, hit_new);
            total *= r;//r.Rotate(total);
        }
    }

    private void CheckHit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            hit_new = hit.point;
        }
    }
}
