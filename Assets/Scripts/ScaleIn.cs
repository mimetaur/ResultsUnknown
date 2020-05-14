using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleIn : MonoBehaviour
{
    public float endAmount = 0.75f;
    public float duration = 2.0f;

    // private float t = 0.0f;
    private Vector3 easeStart = Vector3.zero;
    private Vector3 easeEnd;

    // Use this for initialization
    void Start()
    {
        easeEnd = new Vector3(endAmount, endAmount, endAmount);
        transform.localScale = easeStart;
        StartCoroutine("SpawnAndGrow");
    }

    private IEnumerator SpawnAndGrow()
    {
        float easeSpeed = duration / 100.0f;

        for (float perc = 0; perc <= 1f; perc += 0.01f)
        {
            transform.localScale = Mathfx.Berp(easeStart, easeEnd, perc);
            yield return new WaitForSeconds(easeSpeed);
        }
    }
}
