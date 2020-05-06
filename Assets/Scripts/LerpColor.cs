using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpColor : MonoBehaviour
{
    public GameObject child;
    public Color otherColor;
    private Color startColor;
    public float speed = 1.0f;
    //public float length = 2.0f;

    //private float t = 0.0f;

    //private Vector3 lerpStart = Vector3.zero;
    //private Vector3 lerpEnd;

    private Material mat;

    // Use this for initialization
    void Start()
    {

        mat = child.GetComponent<Renderer>().material;
        startColor = mat.color;
    }

    // Update is called once per frame
    void Update()
    {
        //t += Time.deltaTime;
        //if (t > length) t = length;

        //float percentage = t / length;

        //transform.localScale = Vector3.Lerp(lerpStart, lerpEnd, percentage);
        mat.color = Color.Lerp(startColor, otherColor, Mathf.Sin(Time.time * speed));
    }
}
