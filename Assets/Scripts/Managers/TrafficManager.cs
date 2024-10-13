using UnityEngine;

public class TrafficManager : MonoBehaviour
{
    [Header("Static Obstacle Spawn Settings")]
    public float spawnStaticObstacleDistance = 50f;

    public float staticObstacleSpawnIntervalDistance = 50f;

    [Header("Dynamic Obstacle Spawn Settings")]
    public float frontSpawnDynamicObstacleDistance = 50f;

    public float frontDynamicObstacleSpawnIntervalDistance = 50f;

    public float backSpawnDynamicObstacleDistance = -10f;
    public float backDynamicObstacleSpawnIntervalDistance = 50f;

    [Header("Static And Dynamic Prefabs")] public GameObject staticObstaclePrefab;
    public GameObject dynamicObstaclePrefab;

    [Header("Other")] public float playerPrefabOffset = 0.5f;
    public Transform[] lanePositions;

    public GameObject car;
    public float raycastHeightOffset = 0.5f;
    public LayerMask trafficLayer;
    public Vector3 boxSize = new Vector3(1.4f, 1.0f, 4.18f);
    public Vector3 spawnPosition;
    public Rigidbody rb;
    public float speed;
    public float lastSpawnPosition;

    private Vector3 spawnPosition2; // do usuniecia
    public static TrafficManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        car = PlayerManager.instance.GetPlayerInstance();
        rb = car.GetComponent<Rigidbody>();
        lastSpawnPosition = car.transform.position.z;
    }

    void Update()
    {
        speed = rb.velocity.z;

        float distanceTraveled = car.transform.position.z - lastSpawnPosition;

        if (distanceTraveled >= staticObstacleSpawnIntervalDistance)
        {
            SpawnObstacleCar();
            lastSpawnPosition = car.transform.position.z;
        }

        if (distanceTraveled >= frontDynamicObstacleSpawnIntervalDistance)
        {
            SpawnFrontDynamicCar();
            lastSpawnPosition = car.transform.position.z;
        }

        if (distanceTraveled >= backDynamicObstacleSpawnIntervalDistance)
        {
            SpawnBackDynamicCar();
            lastSpawnPosition = car.transform.position.z;
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
        float randomOffset = Random.Range(-playerPrefabOffset, playerPrefabOffset);

        spawnPosition = new Vector3(lane.position.x + randomOffset, lane.position.y,
            car.transform.position.z + frontSpawnDynamicObstacleDistance);


        if (!IsSpawnLocationClear(spawnPosition))
        {
            GameObject newFrontDynamicCar = Instantiate(dynamicObstaclePrefab, spawnPosition, Quaternion.identity);
          
            CarAIMoving.instance.SpawnRandomCar(randomLaneIndex);
        }
    }

    public void SpawnBackDynamicCar()
    {
        int randomLaneIndex = Random.Range(2, lanePositions.Length);

        Transform lane = lanePositions[randomLaneIndex];
        float randomOffset = Random.Range(-playerPrefabOffset, playerPrefabOffset);

        spawnPosition2 = new Vector3(lane.position.x + randomOffset, lane.position.y,
            car.transform.position.z + backSpawnDynamicObstacleDistance);


        if (!IsSpawnLocationClear(spawnPosition2))
        {
            GameObject newBackDynamicCar = Instantiate(dynamicObstaclePrefab, spawnPosition2, Quaternion.identity);
            CarAIMoving.instance.SpawnRandomCar(randomLaneIndex);
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