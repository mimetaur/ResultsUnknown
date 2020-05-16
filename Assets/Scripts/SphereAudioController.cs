using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource)), RequireComponent(typeof(RotateAround)), RequireComponent(typeof(SphereController))]
public class SphereAudioController : MonoBehaviour
{
    public float basePitch = 1.0f;
    public float pitchRangeDown = 0.5f;
    public float pitchRangeUp = 1.0f;

    private SphereController sc;
    private AudioSource audioSource;
    private RotateAround rotateAround;

    void Start()
    {
        sc = GetComponent<SphereController>();
        audioSource = GetComponent<AudioSource>();
        rotateAround = GetComponent<RotateAround>();

        audioSource.pitch = GameUtils.Map(sc.GetSphereRotationNormalized(), 0f, 1.0f, basePitch - pitchRangeDown, basePitch + pitchRangeUp);

        audioSource.Play();
    }
}
