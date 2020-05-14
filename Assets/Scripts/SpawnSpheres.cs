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
    public float minRotationSpeed = 10.0f;
    public float maxRotationSpeed = 40.0f;
    public float minNewSphereSize = 0.5f;
    public float maxNewSphereSize = 1.0f;

    private int numSpheres = 0;
    private Renderer sphereRenderer;
    private ParentController parentController;

    void Start()
    {
        sphereRenderer = sphere.GetComponentInChildren<Renderer>();
        parentController = parent.gameObject.GetComponent<ParentController>();
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
        rotation.angle = Random.Range(minRotationSpeed, maxRotationSpeed);
        rotation.counterClockWise |= Random.value > 0.5;

        ScaleIn scaleIn = s.GetComponent<ScaleIn>();
        scaleIn.endAmount = Random.Range(minNewSphereSize, maxNewSphereSize);

        numSpheres++;
        parentController.StartGrowing();
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
