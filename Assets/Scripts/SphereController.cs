﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereAudioController)), RequireComponent(typeof(Rotate)), RequireComponent(typeof(RotateAround)), RequireComponent(typeof(ScaleIn)), RequireComponent(typeof(Rigidbody))]
public class SphereController : MonoBehaviour
{
    public float glowFadeDuration = 1.0f;
    public float glowMax = 0.5f;
    public float shrinkFadeDuration = 3.0f;

    private bool isDead = false;
    private bool isActive = false;
    private bool hasCollided = false;
    private bool hasTouchedFloor = false;
    private SphereAudioController sphereAudioController;
    private Rigidbody rb;
    private Rotate rotate;
    private RotateAround rotateAround;
    private ScaleIn scaleIn;
    private SphereManager sphereManager;
    private Material material;

    public bool IsDead { get => isDead; set => isDead = value; }
    public bool HasTouchedFloor { get => hasTouchedFloor; set => hasTouchedFloor = value; }
    public bool HasCollided { get => hasCollided; set => hasCollided = value; }

    void Awake()
    {
        sphereManager = GameObject.Find("GameManager").GetComponent<SphereManager>();
        sphereAudioController = GetComponent<SphereAudioController>();
        rb = GetComponent<Rigidbody>();
        rotate = GetComponent<Rotate>();
        scaleIn = GetComponent<ScaleIn>();
        rotateAround = GetComponent<RotateAround>();

        var meshRenderer = GetComponentInChildren<MeshRenderer>();
        material = meshRenderer.material;
    }

    public GameObject GetParentSphere()
    {
        return sphereManager.GetParentSphere();
    }

    public bool IsActive()
    {
        return isActive;
    }

    public void Activate()
    {
        if (!IsActive())
        {
            sphereAudioController.Play();
            StartCoroutine(FadeGlow(FadeDirection.Up));
            isActive = true;
        }
    }

    public void Deactivate()
    {
        if (IsActive())
        {
            StartCoroutine(FadeGlow(FadeDirection.Down));
            sphereAudioController.Stop();
            isActive = false;
        }
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

    private IEnumerator FadeGlow(FadeDirection direction)
    {
        const int numFadeSteps = 100;
        float stepAmount = 1f / numFadeSteps;

        float easeSpeed = glowFadeDuration / numFadeSteps;
        float easeStart = 0f;
        float easeGoal = glowMax;

        if (direction == FadeDirection.Down)
        {
            easeStart = glowMax;
            easeGoal = 0f;
        }

        for (float pct = 0; pct <= 1f; pct += stepAmount)
        {
            float intensity;
            if (direction == FadeDirection.Down)
            {
                intensity = Mathfx.Sinerp(easeStart, easeGoal, pct);
            }
            else
            {
                intensity = Mathfx.Coserp(easeStart, easeGoal, pct);
            }

            material.SetFloat("_Glow_Intensity", intensity);
            yield return new WaitForSeconds(easeSpeed);
        }
    }

    private void KillOnShrinkComplete()
    {
        IsDead = true;
        sphereManager.KillSphere(this);
    }

    private IEnumerator Shrink(System.Action onComplete)
    {
        float easeSpeed = shrinkFadeDuration / 100.0f;
        Vector3 easeStart = transform.localScale;
        Vector3 easeGoal = Vector3.zero;

        for (float perc = 0; perc <= 1f; perc += 0.01f)
        {
            transform.localScale = Mathfx.Coserp(easeStart, easeGoal, perc);
            yield return new WaitForSeconds(easeSpeed);
        }
        onComplete();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Floor")
        {
            rotate.Active = false;
            rotateAround.IsActive = false;
            HasCollided = true;
            Deactivate();

            if (!HasTouchedFloor)
            {
                sphereAudioController.PlayCollideSound();
            }
            HasTouchedFloor = true;
        }
        else if (col.gameObject.tag == "ChildSphere")
        {
            if (IsActive())
            {
                var newActiveSphere = col.gameObject.GetComponent<SphereController>();
                sphereManager.ChangeActiveSphereTo(newActiveSphere);
            }
            HasCollided = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ParentSphere" && !IsActive())
        {
            rotate.Active = false;
            rotateAround.IsActive = false;
            Deactivate();
            StartCoroutine(Shrink(KillOnShrinkComplete));
        }
        HasCollided = true;
    }
}
