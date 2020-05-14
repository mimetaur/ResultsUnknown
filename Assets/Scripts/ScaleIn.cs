using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleIn : MonoBehaviour
{
    public float endAmount = 0.75f;
    public float duration = 2.0f;

    private float t = 0.0f;

    private Vector3 easeStart = Vector3.zero;
    private Vector3 easeEnd;

    // Use this for initialization
    void Start()
    {
        easeEnd = new Vector3(endAmount, endAmount, endAmount);
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if (t > duration) t = duration;

        float percentage = t / duration;

        transform.localScale = Mathfx.Berp(easeStart, easeEnd, percentage);
    }
}
