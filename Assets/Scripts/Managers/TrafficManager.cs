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
    //
    public float staticObstacleSpawnIntervalDistance = 50f;

    public float frontDynamicObstacleSpawnIntervalDistance = 50f;
    public float backDynamicObstacleSpawnIntervalDistance = 50f;
    public float pointsSpawnIntervalDistance = 30;

    [Space(25)] //
    [Header("Prefabs")]
    //
    public GameObject staticObstaclePrefab;

    public GameObject dynamicObstaclePrefab;
    public GameObject pointsPrefab;

    [Space(25)] //
    [Header("Other")]
    //
    public float playerPrefabOffsetSpawn = 0.5f;

    public float pointsSpawnPositionX = 5f;
    public float pointsSpawnHeight = 0.5f;
    public Transform[] lanePositions;
    public LayerMask trafficLayer;
    private GameObject car;
    private float raycastHeightOffset = 0.5f;
    private Vector3 boxSize = new Vector3(1.4f, 1.0f, 4.18f);
    private Vector3 spawnPosition;
    private Rigidbody rb;
    private Vector3 spawnPosition2; //to be removed in the future for the time being for the reason of debugging
    private float lastSpawnPositionStatic;
    private float lastSpawnPositionFrontDynamic;
    private float lastSpawnPositionBackDynamic;
    private float lastSpawnPositionPoints;
    public static TrafficManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        car = PlayerManager.instance.GetPlayerInstance();
        rb = car.GetComponent<Rigidbody>();
        lastSpawnPositionStatic = car.transform.position.z;
        lastSpawnPositionFrontDynamic = car.transform.position.z;
        lastSpawnPositionBackDynamic = car.transform.position.z;

        lastSpawnPositionPoints = car.transform.position.z;
    }

    void Update()
    {
        CheckSpawnTimers();
    }

    public void CheckSpawnTimers()
    {
        float distanceTraveledStatic = car.transform.position.z - lastSpawnPositionStatic;
        float distanceTraveledFrontDynamic = car.transform.position.z - lastSpawnPositionFrontDynamic;
        float distanceTraveledBackDynamic = car.transform.position.z - lastSpawnPositionBackDynamic;
        float distanceTraveledPints = car.transform.position.z - lastSpawnPositionPoints;

        if (distanceTraveledStatic >= staticObstacleSpawnIntervalDistance)
        {
            SpawnObstacleCar();
            lastSpawnPositionStatic = car.transform.position.z;
        }

        if (distanceTraveledFrontDynamic >= frontDynamicObstacleSpawnIntervalDistance)
        {
            SpawnFrontDynamicCar();
            lastSpawnPositionFrontDynamic = car.transform.position.z;
        }

        if (distanceTraveledBackDynamic >= backDynamicObstacleSpawnIntervalDistance)
        {
            SpawnBackDynamicCar();
            lastSpawnPositionBackDynamic = car.transform.position.z;
        }

        if (distanceTraveledPints >= pointsSpawnIntervalDistance)
        {
            SpawnPointsPrefab();
            lastSpawnPositionPoints = car.transform.position.z;
        }
    }

    public void SpawnObstacleCar()
    {
        int randomLaneIndex = Random.Range(0, lanePositions.Length);
        Transform lane = lanePositions[randomLaneIndex];

        Vector3 spawnPosition = new Vector3(lane.position.x, lane.position.y,
            car.transform.position.z + spawnStaticObstacleDistance);


        if (!IsSpawnLocationClear(spawnPosition))
        {
            GameObject newStaticCar = Instantiate(staticObstaclePrefab, spawnPosition, Quaternion.identity);
        }
    }

    public void SpawnFrontDynamicCar()
    {
        int randomLaneIndex = Random.Range(0, lanePositions.Length);
        Transform lane = lanePositions[randomLaneIndex];
        float randomOffset = Random.Range(-playerPrefabOffsetSpawn, playerPrefabOffsetSpawn);

        spawnPosition = new Vector3(lane.position.x + randomOffset, lane.position.y,
            car.transform.position.z + frontSpawnDynamicObstacleDistance);


        if (!IsSpawnLocationClear(spawnPosition))
        {
            Quaternion rotation;
            if (randomLaneIndex == 0 || randomLaneIndex == 1)
            {
                rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                rotation = Quaternion.identity;
            }

            GameObject newFrontDynamicCar = Instantiate(dynamicObstaclePrefab, spawnPosition, rotation);

            CarAIMoving.instance.SpawnRandomCar(randomLaneIndex);
        }
    }

    public void SpawnBackDynamicCar()
    {
        int randomLaneIndex = Random.Range(2, lanePositions.Length);

        Transform lane = lanePositions[randomLaneIndex];
        float randomOffset = Random.Range(-playerPrefabOffsetSpawn, playerPrefabOffsetSpawn);

        spawnPosition2 = new Vector3(lane.position.x + randomOffset, lane.position.y,
            car.transform.position.z + backSpawnDynamicObstacleDistance);


        if (!IsSpawnLocationClear(spawnPosition2))
        {
            GameObject newBackDynamicCar = Instantiate(dynamicObstaclePrefab, spawnPosition2, Quaternion.identity);
            CarAIMoving.instance.SpawnRandomCar(randomLaneIndex);
        }
    }

    void SpawnPointsPrefab()
    {
        float randomXPosition = Random.Range(-pointsSpawnPositionX, pointsSpawnPositionX);
        spawnPosition = new Vector3(randomXPosition, pointsSpawnHeight,
            car.transform.position.z + pointsSpawnDistance);

        if (!IsSpawnLocationClear(spawnPosition))
        {
            GameObject newPointsObject = Instantiate(pointsPrefab, spawnPosition, Quaternion.identity);
        }
    }

    public bool IsSpawnLocationClear(Vector3 position)
    {
        Vector3 boxCenter = position + Vector3.up * raycastHeightOffset;


        Collider[] colliders = Physics.OverlapBox(boxCenter, boxSize / 2, Quaternion.identity, trafficLayer);


        return colliders.Length > 0;
    }

    void OnDrawGizmos()
    {
        Vector3 boxCenter = spawnPosition + Vector3.up * raycastHeightOffset;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCenter, boxSize);
        boxCenter = spawnPosition2 + Vector3.up * raycastHeightOffset;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
}