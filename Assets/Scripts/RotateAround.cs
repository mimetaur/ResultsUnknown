using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class RotateAround : MonoBehaviour
{
    public Transform target;
    public float angle;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // void Update()
    // {
    //     transform.RotateAround(target.position, Vector3.up, angle);
    // }

    void FixedUpdate()
    {
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.up);
        rb.MovePosition(q * (rb.transform.position - target.position) + target.position);
        rb.MoveRotation(rb.transform.rotation * q);
    }
}
