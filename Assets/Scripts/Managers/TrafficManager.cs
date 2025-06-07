using UnityEngine;

public class TrafficManager : MonoBehaviour
{
    [Header("Distance how far elements are spawned")] //
    public float spawnStaticObstacleDistance = 50f;

    public float frontSpawnDynamicObstacleDistance = 100f;
    public float backSpawnDynamicObstacleDistance = -10f;
    public float pointsSpawnDistance = 50f;

    [Space(25)] //
    [Header("How often objects are spawned")]
    public float staticObstacleSpawnIntervalDistance = 50f;

    public float frontDynamicObstacleSpawnIntervalDistance = 50f;
    public float backDynamicObstacleSpawnIntervalDistance = 50f;
    public float pointsSpawnIntervalDistance = 30;

    [Space(25)] //
    [Header("Prefabs")]
    public GameObject staticObstaclePrefab;

    public GameObject dynamicObstaclePrefab;
    public GameObject pointsPrefab;

    [Space(25)] //
    [Header("Other")]
    public float playerPrefabOffsetSpawn = 0.3f;

    public float pointsSpawnPositionX = 5f;
    public float pointsSpawnHeight = 0.5f;
    public float[] laneXPositions = { -4.85f, -1.75f, 1.75f, 4.85f };
    public LayerMask trafficLayer;
    private GameObject car;
    private readonly float raycastHeightOffset = 0.5f;
    private Vector3 boxSize = new(1.8f, 1.0f, 6.5f);
    private Vector3 spawnPosition;
    private Rigidbody rb;
    private Vector3 spawnPosition2; //to be removed in the future for the time being for the reason of debugging
    private float lastSpawnPositionStatic;
    private float lastSpawnPositionFrontDynamic;
    private float lastSpawnPositionBackDynamic;
    private float lastSpawnPositionPoints;
    public static TrafficManager instance;
    private float timerFront;
    private float timerBack;
    public float frontSpawnIntervalGameOver = 0.5f;
    public float backSpawnIntervalGameOver = 0.5f;
    private bool isGameOver = false;

    private void Awake()
    {
        instance = this;
    }

    public void Activate()
    {
        car = PlayerManager.instance.GetPlayerInstance();
        rb = car.GetComponent<Rigidbody>();
        lastSpawnPositionStatic = car.transform.position.z;
        lastSpawnPositionFrontDynamic = car.transform.position.z;
        lastSpawnPositionBackDynamic = car.transform.position.z;

        lastSpawnPositionPoints = car.transform.position.z;
    }

    private void Update()
    {
        SpawnObstacles();
    }

    public void SpawnObstacles()
    {
        if (isGameOver)
        {
            SpawnWhileGameOver();
        }
        else
        {
            CheckSpawnTimers();
        }
    }

    public void CheckSpawnTimers()
    {
        float distanceTraveledStatic = car.transform.position.z - lastSpawnPositionStatic;
        float distanceTraveledFrontDynamic = car.transform.position.z - lastSpawnPositionFrontDynamic;
        float distanceTraveledBackDynamic = car.transform.position.z - lastSpawnPositionBackDynamic;
        float distanceTraveledPints = car.transform.position.z - lastSpawnPositionPoints;

        if (distanceTraveledStatic >= staticObstacleSpawnIntervalDistance)
        {
            SpawnStaticObstacle();
            lastSpawnPositionStatic = car.transform.position.z;
            return;
        }

        if (distanceTraveledFrontDynamic >= frontDynamicObstacleSpawnIntervalDistance)
        {
            SpawnFrontDynamicCar();
            lastSpawnPositionFrontDynamic = car.transform.position.z;
            return;
        }

        if (distanceTraveledBackDynamic >= backDynamicObstacleSpawnIntervalDistance)
        {
            SpawnBackDynamicCar();
            lastSpawnPositionBackDynamic = car.transform.position.z;
            return;
        }

        if (distanceTraveledPints >= pointsSpawnIntervalDistance)
        {
            SpawnPointsPrefab();
            lastSpawnPositionPoints = car.transform.position.z;
            return;
        }
    }

    private void SpawnWhileGameOver()
    {
        timerFront += Time.deltaTime;
        timerBack += Time.deltaTime;

        if (timerFront >= frontSpawnIntervalGameOver)
        {
            SpawnFrontDynamicCar();
            timerFront = 0;
        }

        if (timerBack >= backSpawnIntervalGameOver)
        {
            SpawnBackDynamicCar();
            timerBack = 0;
        }
    }

    public void SpawnStaticObstacle()
    {
        int randomLaneIndex = Random.Range(0, laneXPositions.Length);
        float laneX = laneXPositions[randomLaneIndex];

        spawnPosition = new(laneX, car.transform.position.y,
            car.transform.position.z + spawnStaticObstacleDistance);

        if (!IsSpawnLocationClear(spawnPosition))
        {
            _ = Instantiate(staticObstaclePrefab, spawnPosition, Quaternion.identity);
        }
    }

    public void SpawnFrontDynamicCar()
    {
        int randomLaneIndex = Random.Range(0, laneXPositions.Length);
        float laneX = laneXPositions[randomLaneIndex];
        float randomOffset = Random.Range(-playerPrefabOffsetSpawn, playerPrefabOffsetSpawn);

        spawnPosition = new(laneX + randomOffset, car.transform.position.y,
            car.transform.position.z + frontSpawnDynamicObstacleDistance);

        if (!IsSpawnLocationClear(spawnPosition))
        {
            Quaternion rotation = randomLaneIndex is 0 or 1 ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
            _ = Instantiate(dynamicObstaclePrefab, spawnPosition, rotation);

            CarAIMoving.instance.SpawnRandomCar(randomLaneIndex);
        }
    }

    public void SpawnBackDynamicCar()
    {
        int randomLaneIndex = Random.Range(2, laneXPositions.Length);
        float laneX = laneXPositions[randomLaneIndex];
        float randomOffset = Random.Range(-playerPrefabOffsetSpawn, playerPrefabOffsetSpawn);

        spawnPosition = new(laneX + randomOffset, car.transform.position.y,
            car.transform.position.z + backSpawnDynamicObstacleDistance);

        if (!IsSpawnLocationClear(spawnPosition))
        {
            _ = Instantiate(dynamicObstaclePrefab, spawnPosition, Quaternion.identity);
            CarAIMoving.instance.SpawnRandomCar(randomLaneIndex);
        }
    }

    private void SpawnPointsPrefab()
    {
        float randomXPosition = Random.Range(-pointsSpawnPositionX, pointsSpawnPositionX);
        spawnPosition = new Vector3(randomXPosition, pointsSpawnHeight,
            car.transform.position.z + pointsSpawnDistance);

        if (!IsSpawnLocationClear(spawnPosition))
        {
            _ = Instantiate(pointsPrefab, spawnPosition, Quaternion.identity);
        }
    }

    public bool IsSpawnLocationClear(Vector3 position)
    {
        Vector3 boxCenter = position + (Vector3.up * raycastHeightOffset);

        Collider[] colliders = Physics.OverlapBox(boxCenter, boxSize / 2, Quaternion.identity, trafficLayer);

        return colliders.Length > 0;
    }

    public void OnPlayerDeath()
    {
        isGameOver = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 boxCenter = spawnPosition;
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
}