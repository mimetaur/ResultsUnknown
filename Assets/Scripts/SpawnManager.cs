using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject spherePrefab = null;
    [SerializeField] private Transform parentSphere = null;
    [SerializeField] private Transform floor = null;
    [SerializeField] private float minRotationSpeed = 10.0f;
    [SerializeField] private float maxRotationSpeed = 40.0f;
    [SerializeField] private float minSphereSize = 0.5f;
    [SerializeField] private float maxSphereSize = 1.0f;
    [SerializeField] private float minRotateAroundSpeed = 0.5f;
    [SerializeField] private float maxRotateAroundSpeed = 2f;
    [SerializeField] private float spawnMaxHeight = 5f;
    [SerializeField] private float spawnMinHeight = 1.0f;
    [SerializeField] private float spawnCircleWidth = 5f;
    [SerializeField] private float proximityToParentThreshold = 1.0f;
    [SerializeField] private float initialDelay = 2.0f;
    [SerializeField] private float rate = 5.0f;
    [SerializeField] private float paddingAboveFloor = 0.05f;
    [SerializeField] private bool doEndSpawning = true;
    [SerializeField] private float spawnForDuration = 180f;

    private SphereManager sphereManager;
    private ParentController parent;
    private bool isSpawning = true;

    public ParentController Parent { get => parent; private set => parent = value; }
    public float MinRotationSpeed { get => minRotationSpeed; private set => minRotationSpeed = value; }
    public float MaxRotationSpeed { get => maxRotationSpeed; private set => maxRotationSpeed = value; }
    public float MinSphereSize { get => minSphereSize; private set => minSphereSize = value; }
    public float MaxSphereSize { get => maxSphereSize; private set => maxSphereSize = value; }
    public float MinRotateAroundSpeed { get => minRotateAroundSpeed; private set => minRotateAroundSpeed = value; }
    public float MaxRotateAroundSpeed { get => maxRotateAroundSpeed; private set => maxRotateAroundSpeed = value; }

    public static SpawnManager Instance { get; private set; }
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
        sphereManager = SphereManager.Instance;
        Parent = parentSphere.gameObject.GetComponent<ParentController>();
        InvokeRepeating("Spawn", initialDelay, rate);
        if (doEndSpawning)
        {
            Invoke("EndSpawning", spawnForDuration);
        }

    }

    private void EndSpawning()
    {
        isSpawning = false;
        sphereManager.StoppedSpawning();
    }

    private void Spawn()
    {
        if (sphereManager.CanSpawn() && isSpawning)
        {
            GameObject newSphere = Instantiate(spherePrefab);
            newSphere.transform.position = parentSphere.transform.position + GetValidSpawnPositionFor(newSphere);
            transform.localScale = Vector3.zero;

            var rotate = newSphere.GetComponent<Rotate>();
            rotate.Angle = GetRandomRotation();
            rotate.CounterClockWise |= Random.value > 0.5f;

            var scaleIn = newSphere.GetComponent<ScaleIn>();
            scaleIn.EndAmount = GetRandomScale();

            var rotateAround = newSphere.GetComponent<RotateAround>();
            rotateAround.Target = parentSphere;
            rotateAround.Angle = GetRandomRotateAroundSpeed();

            sphereManager.AddSphere(newSphere);
            Parent.StartGrowing();
        }
    }

    private float GetRandomRotateAroundSpeed()
    {
        return Random.Range(MinRotateAroundSpeed, MaxRotateAroundSpeed);
    }

    private float GetRandomRotation()
    {
        return Random.Range(MinRotationSpeed, MaxRotationSpeed);
    }

    private float GetRandomScale()
    {
        return Random.Range(MinSphereSize, MaxSphereSize);
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
