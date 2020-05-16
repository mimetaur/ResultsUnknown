using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rotate)), RequireComponent(typeof(ScaleIn))]
public class SphereController : MonoBehaviour
{
    private Rotate rotate;
    private ScaleIn scaleIn;
    private SphereManager sphereManager;

    void Awake()
    {
        sphereManager = GameObject.Find("GameManager").GetComponent<SphereManager>();
        rotate = GetComponent<Rotate>();
        scaleIn = GetComponent<ScaleIn>();

        transform.position = sphereManager.SpawnPosition;
        transform.localScale = Vector3.zero;

        rotate.angle = sphereManager.GetRandomRotation();
        rotate.counterClockWise |= Random.value > 0.5;

        scaleIn.endAmount = sphereManager.GetRandomScale();
    }

    public float GetSphereCountNormalized()
    {
        return GameUtils.Map(sphereManager.NumSpheres, 0, sphereManager.maxSpheres, 0f, 1.0f);
    }

    public float GetSphereRotationNormalized()
    {
        return GameUtils.Map(rotate.angle, sphereManager.minRotationSpeed, sphereManager.maxRotationSpeed, 0f, 1.0f);
    }
}
