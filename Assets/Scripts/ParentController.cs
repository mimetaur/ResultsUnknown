using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentController : MonoBehaviour
{
    public float growAmount = 0.1f;
    public float growDuration = 1.0f;

    public void StartGrowing()
    {
        StartCoroutine("Grow");
    }

    private IEnumerator Grow()
    {
        Vector3 easeStart = transform.localScale;
        Vector3 easeGoal = easeStart * (1.0f + growAmount);
        float easeSpeed = growDuration / 100.0f;

        for (float perc = 0; perc <= 1f; perc += 0.01f)
        {
            transform.localScale = Mathfx.Berp(easeStart, easeGoal, perc);
            yield return new WaitForSeconds(easeSpeed);
        }
    }
}
