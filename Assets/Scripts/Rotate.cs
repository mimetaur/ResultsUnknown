using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Rotate : MonoBehaviour
{
    [Range(0, 360)] [SerializeField] private float angle;
    [Range(0, 1.0f)] [SerializeField] private float slowdownFactor = 5f;
    [SerializeField] private bool counterClockWise = false;
    [SerializeField] private bool usePhysicsRotation = true;

    private bool active = true;
    private Rigidbody myRigidbody;

    public bool Active { get => active; set => active = value; }
    public float Angle { get => angle; set => angle = value; }
    public bool CounterClockWise { get => counterClockWise; set => counterClockWise = value; }

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
        if (CounterClockWise) axis *= -1;

        float angleThisFrame = Angle * Time.deltaTime * slowdownFactor;
        Vector3 torque = axis * angleThisFrame;
        myRigidbody.AddTorque(torque);
    }

    void DoRotation()
    {
        Vector3 axis = transform.forward;
        if (CounterClockWise) axis *= -1;

        float angleThisFrame = Angle * Time.deltaTime;
        Quaternion rotation = Quaternion.AngleAxis(angleThisFrame, axis);


        Quaternion ourLastRotation = transform.rotation;

        transform.rotation = rotation * ourLastRotation;
    }
}
