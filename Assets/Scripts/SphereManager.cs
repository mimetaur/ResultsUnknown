using System.Collections;
using System.Collections.Generic;
using Array = System.Array;
using UnityEngine;

[RequireComponent(typeof(SpawnSpheres))]
public class SphereManager : MonoBehaviour
{
    public float minRotationSpeed = 10.0f;
    public float maxRotationSpeed = 40.0f;
    public float minSphereSize = 0.5f;
    public float maxSphereSize = 1.0f;
    public int maxSpheres = 40;
    public float sphereChangeSpeed = 10.0f;

    private List<SphereController> spheres = new List<SphereController>();
    private Vector3 spawnPosition;
    private int numSpheres = 0;

    private SpawnSpheres spawnSpheres;
    private GameObject spherePrefab;
    private Transform parent;
    private Transform floor;
    private Renderer sphereRenderer;

    public Vector3 SpawnPosition { get => spawnPosition; set => spawnPosition = value; }
    public int NumSpheres { get => numSpheres; set => numSpheres = value; }

    void Awake()
    {
        spawnSpheres = GetComponent<SpawnSpheres>();
        spherePrefab = spawnSpheres.sphere;
        parent = spawnSpheres.parent;
        floor = spawnSpheres.floor;

        sphereRenderer = spherePrefab.GetComponentInChildren<Renderer>();
        InvokeRepeating("ChangeActiveSphere", sphereChangeSpeed, sphereChangeSpeed);
    }

    public float GetRandomRotation()
    {
        return Random.Range(minRotationSpeed, maxRotationSpeed);
    }

    public float GetRandomScale()
    {
        return Random.Range(minSphereSize, maxSphereSize);
    }

    public void Spawn()
    {
        if (!CanSpawn()) return;

        SpawnPosition = parent.transform.position + GetValidSpawnPosition();
        GameObject p = Instantiate(spherePrefab);
        var s = p.GetComponent<SphereController>();
        spheres.Add(s);

        if (spheres.Count == 1)
        {
            ActivateSphere(s);
        }
        Debug.Log("Number of spheres: " + spheres.Count);
    }

    private SphereController GetActiveSphere()
    {
        return spheres.Find(s => s.IsActive());
    }

    private void ActivateSphere(SphereController s)
    {
        s.Activate();
    }

    private void DeactivateAllSpheres()
    {
        spheres.ForEach(s => s.Deactivate());
    }

    private void ChangeActiveSphere()
    {
        var previousActiveSphere = GetActiveSphere();
        int index = Random.Range(0, spheres.Count);
        var activeSphere = spheres[index];
        if (activeSphere && activeSphere != previousActiveSphere)
        {
            DeactivateAllSpheres();
            ActivateSphere(activeSphere);
        }
    }

    private bool CanSpawn()
    {
        return spheres.Count < maxSpheres;
    }

    private bool CheckValidPosition(Vector3 pos)
    {
        bool greaterThanMinDistance = Vector3.Distance(parent.position, pos) > spawnSpheres.proximityToParentThreshold;
        bool aboveFloor = pos.y > (floor.position.y + sphereRenderer.bounds.size.y + spawnSpheres.paddingAboveFloor);
        return greaterThanMinDistance && aboveFloor;
    }

    private Vector3 GetValidSpawnPosition()
    {
        bool isValidPosition = false;
        Vector3 validPosition = new Vector3();

        while (!isValidPosition)
        {
            Vector3 randomVector = Random.insideUnitSphere * spawnSpheres.spawnAreaSize;
            isValidPosition = CheckValidPosition(randomVector);
            if (isValidPosition)
            {
                validPosition = randomVector;
            }
        }
        return validPosition;
    }
}
