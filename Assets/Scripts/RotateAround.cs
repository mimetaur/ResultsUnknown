using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class RotateAround : MonoBehaviour
{
    public Transform target;
    public float angle;
    private Rigidbody rb;
    private bool isActive = true;

    public bool IsActive { get => isActive; set => isActive = value; }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!IsActive) return;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.up);
        rb.MovePosition(q * (rb.transform.position - target.position) + target.position);
        rb.MoveRotation(rb.transform.rotation * q);
    }
}
