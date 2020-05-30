using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereManager : MonoBehaviour
{
    [SerializeField] private int maxSpheres = 40;
    [SerializeField] private float gravityAmount = -0.1f;
    [SerializeField] private float minTimeUntilChangeActive = 3f;
    [SerializeField] private float maxTimeUntilChangeActive = 10f;

    private List<SphereController> spheres = new List<SphereController>();
    private SpawnManager spawnManager;

    public static SphereManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        Physics.gravity = new Vector3(0, gravityAmount, 0);
        spawnManager = SpawnManager.Instance;
    }

    public void StoppedSpawning()
    {
        float timeUntilChangeActive = Random.Range(minTimeUntilChangeActive, maxTimeUntilChangeActive);
        InvokeRepeating("ChangeActiveSphere", timeUntilChangeActive, timeUntilChangeActive);
    }

    public void AddSphere(GameObject newSphere)
    {
        var s = newSphere.GetComponent<SphereController>();
        spheres.Add(s);

        if (spheres.Count == 1)
        {
            ActivateSphere(s);
        }
        float timeUntilChangeActive = Random.Range(minTimeUntilChangeActive, maxTimeUntilChangeActive);
        Invoke("ChangeActiveSphere", timeUntilChangeActive);
    }

    public void KillSphere(SphereController sp)
    {
        spheres.Remove(sp);
        sp.gameObject.SetActive(false);
    }

    public GameObject GetParentSphere()
    {
        return spawnManager.Parent.gameObject;
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
        return (spawnManager.MinRotationSpeed, spawnManager.MaxRotationSpeed);
    }

    public (float min, float max) GetSphereSizeRange()
    {
        return (spawnManager.MinSphereSize, spawnManager.MaxSphereSize);
    }

    public (float min, float max) GetSphereRotateAroundSpeedRange()
    {
        return (spawnManager.MinRotateAroundSpeed, spawnManager.MaxRotateAroundSpeed);
    }

    private SphereController GetActiveSphere()
    {
        return spheres.Find(s => s.IsActive());
    }

    private void ActivateSphere(SphereController s)
    {
        if (s)
        {
            s.Activate();
        }
    }

    private void DeactivateActiveSphere()
    {
        var s = GetActiveSphere();
        if (s)
        {
            s.Deactivate();
        }

    }

    public void ChangeActiveSphere()
    {
        if (spheres.Count > 1)
        {
            var previousSphere = GetActiveSphere();
            var activeSphere = spheres[Random.Range(0, spheres.Count)];
            if (previousSphere)
            {
                if (activeSphere != previousSphere && !activeSphere.HasTouchedFloor)
                {
                    DeactivateActiveSphere();
                    ActivateSphere(activeSphere);
                }
            }
            else
            {
                if (!activeSphere.HasTouchedFloor)
                {
                    ActivateSphere(activeSphere);
                }

            }
        }
    }

    public void ChangeActiveSphereTo(SphereController newActiveSphere)
    {
        DeactivateActiveSphere();
        ActivateSphere(newActiveSphere);
        CancelInvoke("ChangeActiveSphere");
        float timeUntilChangeActive = Random.Range(minTimeUntilChangeActive, maxTimeUntilChangeActive);
        Invoke("ChangeActiveSphere", timeUntilChangeActive);
    }
}
