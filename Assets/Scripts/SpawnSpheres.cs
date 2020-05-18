using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereManager))]
public class SpawnSpheres : MonoBehaviour
{
    public GameObject spherePrefab;
    public Transform parentSphere;
    public Transform floor;

    public float minRotationSpeed = 10.0f;
    public float maxRotationSpeed = 40.0f;
    public float minSphereSize = 0.5f;
    public float maxSphereSize = 1.0f;
    public float minRotateAroundSpeed = 0.5f;
    public float maxRotateAroundSpeed = 2f;

    public float spawnMaxHeight = 5f;
    public float spawnMinHeight = 1.0f;
    public float spawnCircleWidth = 5f;

    public float spawnAreaSize = 5.0f;
    public float proximityToParentThreshold = 1.0f;
    public float initialDelay = 2.0f;
    public float rate = 5.0f;
    public float paddingAboveFloor = 0.05f;

    private SphereManager sphereManager;
    private ParentController parent;

    public ParentController Parent { get => parent; set => parent = value; }

    void Start()
    {
        sphereManager = GetComponent<SphereManager>();
        Parent = parentSphere.gameObject.GetComponent<ParentController>();
        InvokeRepeating("Spawn", initialDelay, rate);
    }

    private void Spawn()
    {
        if (sphereManager.CanSpawn())
        {
            GameObject newSphere = Instantiate(spherePrefab);
            newSphere.transform.position = parentSphere.transform.position + GetValidSpawnPositionFor(newSphere);
            transform.localScale = Vector3.zero;

            var rotate = newSphere.GetComponent<Rotate>();
            rotate.angle = GetRandomRotation();
            rotate.counterClockWise |= Random.value > 0.5f;

            var scaleIn = newSphere.GetComponent<ScaleIn>();
            scaleIn.endAmount = GetRandomScale();

            var rotateAround = newSphere.GetComponent<RotateAround>();
            rotateAround.target = parentSphere;
            rotateAround.angle = GetRandomRotateAroundSpeed();

            sphereManager.AddSphere(newSphere);
            Parent.StartGrowing();
        }
    }

    private float GetRandomRotateAroundSpeed()
    {
        return Random.Range(minRotateAroundSpeed, maxRotateAroundSpeed);
    }

    private float GetRandomRotation()
    {
        return Random.Range(minRotationSpeed, maxRotationSpeed);
    }

    private float GetRandomScale()
    {
        return Random.Range(minSphereSize, maxSphereSize);
    }

    private bool CheckValidPosition(Vector3 pos, Bounds bounds)
    {
        bool greaterThanMinDistance = Vector3.Distance(parentSphere.position, pos) > proximityToParentThreshold;
        bool aboveFloor = pos.y > (floor.position.y + bounds.size.y + paddingAboveFloor);
        return greaterThanMinDistance && aboveFloor;
    }

    public Vector3 GetValidSpawnPositionFor(GameObject newSphere)
    {
        bool isValidPosition = false;
        Vector3 validPosition = new Vector3();
        Bounds sphereBounds = newSphere.GetComponentInChildren<Renderer>().bounds;

        while (!isValidPosition)
        {
            float spawnHeight = Random.Range(spawnMinHeight, spawnMaxHeight);
            Vector2 spawnCircle = Random.insideUnitCircle * spawnCircleWidth;
            Vector3 randomPos = new Vector3(spawnCircle.x, spawnHeight, spawnCircle.y);

            isValidPosition = CheckValidPosition(randomPos, sphereBounds);
            if (isValidPosition)
            {
                validPosition = randomPos;
            }
        }
        return validPosition;
    }
}
