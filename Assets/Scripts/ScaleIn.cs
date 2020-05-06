using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleIn : MonoBehaviour
{
    public float endAmount = 0.75f;
    public float length = 2.0f;

    private float t = 0.0f;

    private Vector3 lerpStart = Vector3.zero;
    private Vector3 lerpEnd;

    // Use this for initialization
    void Start()
    {
        lerpEnd = new Vector3(endAmount, endAmount, endAmount);
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if (t > length) t = length;

        float percentage = t / length;

        transform.localScale = Vector3.Lerp(lerpStart, lerpEnd, percentage);
    }
}