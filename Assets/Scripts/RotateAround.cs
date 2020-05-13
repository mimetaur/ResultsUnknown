using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{

    public Transform target;
    public float angle;

    // Use this for initialization
    void Start()
    {
        if (gameObject.tag == "ChildSphere" || target == null)
        {
            target = GameObject.Find("ParentIcoSphere").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(target.position, Vector3.up, angle);
    }
}
