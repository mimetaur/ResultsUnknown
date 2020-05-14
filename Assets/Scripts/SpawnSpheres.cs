using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpheres : MonoBehaviour
{

    public GameObject sphere;
    public Transform parent;
    public Transform floor;

    public float overallSize = 5.0f;
    public float minSize = 1.0f;
    public float initialDelay = 2.0f;
    public float rate = 5.0f;
    public int maxSpheres = 11;
    public int maxAttemptsAtValidPosition = 5;
    public float paddingAboveFloor = 0.05f;

    private int numSpheres = 0;
    private Renderer sphereRenderer;

    void Start()
    {
        sphereRenderer = sphere.GetComponentInChildren<Renderer>();
        InvokeRepeating("SpawnSphere", initialDelay, rate);
    }

    private void SpawnSphere()
    {
        if (numSpheres > maxSpheres) return;

        Vector3 spawnPosition = GetValidSpawnPosition();

        GameObject s = Instantiate(sphere);
        s.transform.position = parent.position + spawnPosition;
        s.transform.localScale = Vector3.zero;

        Rotate rotation = s.GetComponent<Rotate>();
        rotation.angle = Random.Range(30, 60);
        rotation.counterClockWise |= Random.value > 0.5;

        ScaleIn scaleIn = s.GetComponent<ScaleIn>();
        scaleIn.length = Random.Range(0.5f, 10.0f);

        numSpheres++;
    }

    private Vector3 GetValidSpawnPosition()
    {
        bool isValidPosition = false;
        Vector3 validPosition = new Vector3();
        int numAttempts = 0;

        while (!isValidPosition && numAttempts < maxAttemptsAtValidPosition)
        {
            numAttempts++;
            Vector3 randomVector = Random.insideUnitSphere * overallSize;
            isValidPosition = CheckValidPosition(randomVector);
            if (isValidPosition)
            {
                validPosition = randomVector;
            }
        }
        return validPosition;
    }

    private bool CheckValidPosition(Vector3 pos)
    {
        bool greaterThanMinimumDistance = Vector3.Distance(parent.position, pos) > minSize;
        bool aboveFloor = pos.y > floor.position.y + sphereRenderer.bounds.size.y + paddingAboveFloor;
        return greaterThanMinimumDistance && aboveFloor;
    }
}
