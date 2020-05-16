using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereManager))]
public class SpawnSpheres : MonoBehaviour
{
    public GameObject sphere;
    public Transform parent;
    public Transform floor;

    public float spawnAreaSize = 5.0f;
    public float proximityToParentThreshold = 1.0f;
    public float initialDelay = 2.0f;
    public float rate = 5.0f;
    public float paddingAboveFloor = 0.05f;

    private SphereManager sphereManager;
    private ParentController parentController;

    void Start()
    {
        sphereManager = GetComponent<SphereManager>();
        parentController = parent.gameObject.GetComponent<ParentController>();
        InvokeRepeating("SpawnSphere", initialDelay, rate);
    }

    private void SpawnSphere()
    {
        sphereManager.Spawn();
        parentController.StartGrowing();
    }
}
