using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class RotateAround : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float angle;

    private Rigidbody rb = null;
    private bool isActive = true;

    public bool IsActive { get => isActive; set => isActive = value; }
    public float Angle { get => angle; set => angle = value; }
    public Transform Target { get => target; set => target = value; }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!IsActive) return;
        Quaternion q = Quaternion.AngleAxis(Angle, Vector3.up);
        rb.MovePosition(q * (rb.transform.position - Target.position) + Target.position);
        rb.MoveRotation(rb.transform.rotation * q);
    }
}
