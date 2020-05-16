using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpawnSpheres))]
public class SphereManager : MonoBehaviour
{
    public int maxSpheres = 40;
    public float sphereChangeSpeed = 10.0f;
    private List<SphereController> spheres = new List<SphereController>();
    private SpawnSpheres spawner;

    void Awake()
    {
        spawner = GetComponent<SpawnSpheres>();
        InvokeRepeating("ChangeActiveSphere", sphereChangeSpeed, sphereChangeSpeed);
    }

    public void AddSphere(GameObject newSphere)
    {
        var s = newSphere.GetComponent<SphereController>();
        spheres.Add(s);

        if (spheres.Count == 1)
        {
            ActivateSphere(s);
        }
        Debug.Log("Number of spheres: " + spheres.Count);
    }

    public bool CanSpawn()
    {
        return spheres.Count < maxSpheres;
    }

    public (int count, int max) GetSphereCountRange()
    {
        return (spheres.Count, maxSpheres);
    }

    public (float min, float max) GetSphereRotationSpeedRange()
    {
        return (spawner.minRotationSpeed, spawner.maxRotationSpeed);
    }

    public (float min, float max) GetSphereSizeRange()
    {
        return (spawner.minSphereSize, spawner.maxSphereSize);
    }

    public (float min, float max) GetSphereRotateAroundSpeedRange()
    {
        return (spawner.minRotateAroundSpeed, spawner.maxRotateAroundSpeed);
    }

    private int GetActiveSphereIndex()
    {
        return spheres.FindIndex(s => s.IsActive());
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
        var previousSphere = GetActiveSphere();
        var activeSphere = spheres[Random.Range(0, spheres.Count)];

        if (activeSphere != previousSphere)
        {
            DeactivateAllSpheres();
            ActivateSphere(activeSphere);
        }
    }
}
