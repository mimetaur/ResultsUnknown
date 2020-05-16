using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource)), RequireComponent(typeof(SphereController))]
public class SphereAudioController : MonoBehaviour
{
    public float basePitch = 1.0f;
    public float pitchRangeDown = 0.5f;
    public float pitchRangeUp = 1.0f;
    public float audioFadeDuration = 3.0f;

    private SphereController sc;
    private AudioSource audioSource;
    private float minVolume = 0f;
    private float maxVolume = 1f;

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
            StartCoroutine(FadeAudio(FadeDirection.Up, DoNothingOnComplete));
        }

    }

    public void Stop()
    {
        if (audioSource.isPlaying)
        {
            StartCoroutine(FadeAudio(FadeDirection.Down, StopAudioOnComplete));
        }

    }

    // callback action when fade coroutine is complete
    private void DoNothingOnComplete()
    {
        // do nothing
    }

    // callback action when fade coroutine is complete
    private void StopAudioOnComplete()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private IEnumerator FadeAudio(FadeDirection direction, System.Action onComplete)
    {
        const int numFadeSteps = 100;
        float stepAmount = 1f / numFadeSteps;

        float easeSpeed = audioFadeDuration / numFadeSteps;
        float easeStart = minVolume;
        float easeGoal = maxVolume;

        if (direction == FadeDirection.Down)
        {
            easeStart = maxVolume;
            easeGoal = minVolume;
        }

        for (float pct = 0; pct <= 1f; pct += stepAmount)
        {
            float volumeLevel;
            if (direction == FadeDirection.Down)
            {
                volumeLevel = Mathfx.Sinerp(easeStart, easeGoal, pct);
            }
            else
            {
                volumeLevel = Mathfx.Coserp(easeStart, easeGoal, pct);
            }
            audioSource.volume = volumeLevel;

            yield return new WaitForSeconds(easeSpeed);
        }
        onComplete();
    }
}
