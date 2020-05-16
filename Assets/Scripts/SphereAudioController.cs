using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource)), RequireComponent(typeof(SphereController))]
public class SphereAudioController : MonoBehaviour
{
    public float basePitch = 1.0f;
    public float pitchRangeDown = 0.5f;
    public float pitchRangeUp = 1.0f;

    private SphereController sc;
    private AudioSource audioSource;

    void Awake()
    {
        sc = GetComponent<SphereController>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
        audioSource.pitch = GameUtils.Map(sc.GetSphereRotateAroundSpeedNormalized(), 0f, 1.0f, basePitch - pitchRangeDown, basePitch + pitchRangeUp);

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }

    }

    public void Stop()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

    }
}
