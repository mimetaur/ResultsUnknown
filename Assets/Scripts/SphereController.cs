using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereAudioController)), RequireComponent(typeof(Rotate)), RequireComponent(typeof(ScaleIn))]
public class SphereController : MonoBehaviour
{
    public float glowFadeDuration = 1.0f;
    private SphereAudioController sphereAudioController;
    private Rotate rotate;
    private ScaleIn scaleIn;
    private SphereManager sphereManager;
    private Material material;
    private bool isActive = false;

    void Awake()
    {
        sphereManager = GameObject.Find("GameManager").GetComponent<SphereManager>();
        sphereAudioController = GetComponent<SphereAudioController>();
        rotate = GetComponent<Rotate>();
        scaleIn = GetComponent<ScaleIn>();

        var meshRenderer = GetComponentInChildren<MeshRenderer>();
        material = meshRenderer.material;

        SetInitalValues();
    }

    public bool IsActive()
    {
        return isActive;
    }


    private void SetInitalValues()
    {
        transform.position = sphereManager.SpawnPosition;
        transform.localScale = Vector3.zero;
        rotate.angle = sphereManager.GetRandomRotation();
        rotate.counterClockWise |= Random.value > 0.5f;
        scaleIn.endAmount = sphereManager.GetRandomScale();
    }

    public void Activate()
    {
        isActive = true;
        sphereAudioController.Play();
        material.SetFloat("_Glow_Intensity", 1f);
    }

    public void Deactivate()
    {
        isActive = false;
        sphereAudioController.Stop();
        material.SetFloat("_Glow_Intensity", 0f);
    }

    public float GetSphereCountNormalized()
    {
        return GameUtils.Map(sphereManager.NumSpheres, 0, sphereManager.maxSpheres, 0f, 1f);
    }

    public float GetSphereRotationNormalized()
    {
        return GameUtils.Map(rotate.angle, sphereManager.minRotationSpeed, sphereManager.maxRotationSpeed, 0f, 1f);
    }

    public float GetSphereSizeNormalized()
    {
        var scale = transform.localScale.y;
        return GameUtils.Map(scale, 0f, 1f, sphereManager.minSphereSize, sphereManager.maxSphereSize);
    }

}
