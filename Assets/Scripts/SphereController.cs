using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereAudioController)), RequireComponent(typeof(Rotate)), RequireComponent(typeof(RotateAround)), RequireComponent(typeof(ScaleIn))]
public class SphereController : MonoBehaviour
{
    public float glowFadeDuration = 1.0f;

    private bool isActive = false;
    private SphereAudioController sphereAudioController;
    private Rotate rotate;
    private RotateAround rotateAround;
    private ScaleIn scaleIn;
    private SphereManager sphereManager;
    private Material material;


    void Awake()
    {
        sphereManager = GameObject.Find("GameManager").GetComponent<SphereManager>();
        sphereAudioController = GetComponent<SphereAudioController>();
        rotate = GetComponent<Rotate>();
        scaleIn = GetComponent<ScaleIn>();
        rotateAround = GetComponent<RotateAround>();

        var meshRenderer = GetComponentInChildren<MeshRenderer>();
        material = meshRenderer.material;
    }

    public bool IsActive()
    {
        return isActive;
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
        var range = sphereManager.GetSphereCountRange();
        return GameUtils.Map(range.count, 0, range.max, 0f, 1f);
    }

    public float GetSphereRotationNormalized()
    {
        var range = sphereManager.GetSphereRotationSpeedRange();
        return GameUtils.Map(rotate.angle, range.min, range.max, 0f, 1f);
    }

    public float GetSphereSizeNormalized()
    {
        var range = sphereManager.GetSphereSizeRange();
        var scale = transform.localScale.y;
        return GameUtils.Map(scale, range.min, range.max, 0f, 1f);
    }

    public float GetSphereRotateAroundSpeedNormalized()
    {
        var range = sphereManager.GetSphereRotateAroundSpeedRange();
        return GameUtils.Map(rotateAround.angle, range.min, range.max, 0f, 1f);
    }

}
