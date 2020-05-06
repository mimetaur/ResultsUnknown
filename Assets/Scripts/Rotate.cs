using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [Range(0, 360)]
    public float angle;

    [Range(0, 1.0f)]
    public float slowdownFactor = 0.1f;

    public bool counterClockWise = false;
    public bool usePhysicsRotation = true;

    private Rigidbody myRigidbody;

    // Use this for initialization
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
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
