using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpheres : MonoBehaviour
{

    public GameObject sphere;
    public Transform parent;
    public float overallSize = 5.0f;
    public float rate = 5.0f;
    public int maxSpheres = 11;
    private int numSpheres = 0;

    void Start()
    {
        InvokeRepeating("SpawnSphere", 2.0f, rate);
    }

    void SpawnSphere()
    {
        if (numSpheres > maxSpheres) return;

        GameObject s = Instantiate(sphere);
        s.transform.position = parent.position + (Random.insideUnitSphere * overallSize);
        s.transform.localScale = Vector3.zero;

        Rotate rotation = s.GetComponent<Rotate>();
        rotation.angle = Random.Range(30, 60);
        rotation.counterClockWise |= Random.value > 0.5;

        ScaleIn scaleIn = s.GetComponent<ScaleIn>();
        scaleIn.length = Random.Range(0.5f, 10.0f);

        numSpheres++;
    }
}