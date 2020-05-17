using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Rotate : MonoBehaviour
{
    [Range(0, 360)]
    public float angle;

    [Range(0, 1.0f)]
    public float slowdownFactor = 5f;

    public bool counterClockWise = false;
    public bool usePhysicsRotation = true;
    private bool active = true;

    private Rigidbody myRigidbody;

    public bool Active { get => active; set => active = value; }

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!Active) return;
        if (myRigidbody.isKinematic)
        {
            DoRotation();
        }
        else
        {
            if (usePhysicsRotation)
            {
                DoPhysicsRotation();
            }
            else
            {
                DoRotation();
            }
        }
    }

    void DoPhysicsRotation()
    {
        Vector3 axis = transform.forward;
        if (counterClockWise) axis *= -1;

        float angleThisFrame = angle * Time.deltaTime * slowdownFactor;
        Vector3 torque = axis * angleThisFrame;
        myRigidbody.AddTorque(torque);
    }

    void DoRotation()
    {
        Vector3 axis = transform.forward;
        if (counterClockWise) axis *= -1;

        float angleThisFrame = angle * Time.deltaTime;
        Quaternion rotation = Quaternion.AngleAxis(angleThisFrame, axis);


        Quaternion ourLastRotation = transform.rotation;

        transform.rotation = rotation * ourLastRotation;
    }
}
