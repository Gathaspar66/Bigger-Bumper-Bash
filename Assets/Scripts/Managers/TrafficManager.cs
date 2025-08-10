using System.Linq;
using UnityEngine;

public class TrafficManager : MonoBehaviour
{
    [Header("Distance how far elements are spawned")] //
    public float spawnStaticObstacleDistance = 50f;

    public float frontSpawnDynamicObstacleDistance = 100f;
    public float backSpawnDynamicObstacleDistance = -10f;
    public float pointsSpawnDistance = 50f;
    public float healthSpawnDistance = 50f;

    [Space(25)] //
    [Header("How often objects are spawned")]
    public float staticObstacleSpawnIntervalDistance = 50f;

    public float frontDynamicObstacleSpawnIntervalDistance = 50f;
    public float backDynamicObstacleSpawnIntervalDistance = 50f;
    public float pointsSpawnIntervalDistance = 30;
    public float healthSpawnIntervalDistance = 100;
    [Space(25)] //
    [Header("Prefabs")]
    public GameObject staticObstaclePrefab;

    public GameObject dynamicObstaclePrefab;
    public GameObject pointsPrefab;
    public GameObject healthPrefab;
    [Space(25)] //
    [Header("Other")]
    public float playerPrefabOffsetSpawn = 0.3f;
    public float staticPrefabOffsetSpawn = 0.5f;
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
    private float lastSpawnPositionHealth;
    public static TrafficManager instance;
    private float timerFront;
    private float timerBack;
    public float frontSpawnIntervalGameOver = 0.5f;
    public float backSpawnIntervalGameOver = 0.5f;
    private bool isGameOver = false;
    private float[] frontLanesX;
    private float[] backLanesX;
    private float[] staticObstacleLanesX;

    private Lane[] frontLanes;
    private Lane[] backLanes;
    private StaticObstacle[] staticObstaclesData;

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
        ConfigureLevelSpawners();
    }

    public void ConfigureLevelSpawners()
    {
        if (LevelConfigLoader.instance != null && LevelConfigLoader.instance.IsConfigLoaded())
        {
            Level currentLevel = LevelConfigLoader.instance.gameConfig.levels[0];
            LevelConfig config = currentLevel.levelConfig;

            Lane[] front = config.lanes
                .Where(lane => lane.spawner == "Front")
                .ToArray();

            Lane[] back = config.lanes
                .Where(lane => lane.spawner == "Back")
                .ToArray();

            StaticObstacle[] staticObstacles = config.staticObstacles;

            SetSpawnLanes(front, back, staticObstacles);
        }
        else
        {
            Debug.LogError("LevelConfigLoader not loaded or missing!");
        }
    }

    public void SetSpawnLanes(Lane[] front, Lane[] back, StaticObstacle[] staticObstacles)
    {
        frontLanes = front;
        backLanes = back;
        staticObstaclesData = staticObstacles;

        frontLanesX = front.Select(l => l.positionX).ToArray();
        backLanesX = back.Select(l => l.positionX).ToArray();
        staticObstacleLanesX = staticObstacles.Select(s => s.positionX).ToArray();
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
        float distanceTraveledPoints = car.transform.position.z - lastSpawnPositionPoints;
        float distanceTraveledHealth = car.transform.position.z - lastSpawnPositionHealth;
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

        if (distanceTraveledPoints >= pointsSpawnIntervalDistance)
        {
            SpawnPointsPrefab(pointsPrefab);
            lastSpawnPositionPoints = car.transform.position.z;
            return;
        }
        if (distanceTraveledHealth >= healthSpawnIntervalDistance)
        {
            SpawnPointsPrefab(healthPrefab);
            lastSpawnPositionHealth = car.transform.position.z;
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
        if (staticObstacleLanesX == null || staticObstacleLanesX.Length == 0)
        {
            Debug.LogWarning("No static obstacle lanes available to spawn.");
            return;
        }
        float randomOffset = Random.Range(-staticPrefabOffsetSpawn, staticPrefabOffsetSpawn);
        int randomLaneIndex = Random.Range(0, staticObstacleLanesX.Length);
        float laneX = staticObstacleLanesX[randomLaneIndex] + randomOffset;

        spawnPosition = new Vector3(laneX, car.transform.position.y,
            car.transform.position.z + spawnStaticObstacleDistance);

        if (!IsSpawnLocationClear(spawnPosition))
        {
            _ = Instantiate(staticObstaclePrefab, spawnPosition, Quaternion.identity);
        }
    }

    public void SpawnFrontDynamicCar()
    {
        if (frontLanes == null || frontLanes.Length == 0)
        {
            Debug.LogWarning("No front lanes available to spawn.");
            return;
        }

        int randomLaneIndex = Random.Range(0, frontLanes.Length);
        Lane selectedLane = frontLanes[randomLaneIndex];

        float randomOffset = Random.Range(-playerPrefabOffsetSpawn, playerPrefabOffsetSpawn);
        spawnPosition = new Vector3(selectedLane.positionX + randomOffset, car.transform.position.y,
            car.transform.position.z + frontSpawnDynamicObstacleDistance);

        if (!IsSpawnLocationClear(spawnPosition))
        {
            Quaternion rotation = selectedLane.direction == "Backward"
                ? Quaternion.Euler(0, 180, 0)
                : Quaternion.identity;

            _ = Instantiate(dynamicObstaclePrefab, spawnPosition, rotation);
            CarAIDynamicObstacle.instance.SpawnRandomCarModel(randomLaneIndex);
        }
    }

    public void SpawnBackDynamicCar()
    {
        if (backLanes == null || backLanes.Length == 0)
        {
            Debug.LogWarning("No back lanes available to spawn.");
            return;
        }

        int randomLaneIndex = Random.Range(0, backLanes.Length);
        Lane selectedLane = backLanes[randomLaneIndex];

        float randomOffset = Random.Range(-playerPrefabOffsetSpawn, playerPrefabOffsetSpawn);
        spawnPosition = new Vector3(selectedLane.positionX + randomOffset, car.transform.position.y,
            car.transform.position.z + backSpawnDynamicObstacleDistance);

        if (!IsSpawnLocationClear(spawnPosition))
        {
            Quaternion rotation = selectedLane.direction == "Backward"
                ? Quaternion.Euler(0, 180, 0)
                : Quaternion.identity;

            _ = Instantiate(dynamicObstaclePrefab, spawnPosition, rotation);
            CarAIDynamicObstacle.instance.SpawnRandomCarModel(randomLaneIndex);
        }
    }

    private void SpawnPointsPrefab(GameObject spawnBonus)
    {
        float randomXPosition = Random.Range(-pointsSpawnPositionX, pointsSpawnPositionX);
        spawnPosition = new Vector3(randomXPosition, pointsSpawnHeight,
            car.transform.position.z + pointsSpawnDistance);

        if (!IsSpawnLocationClear(spawnPosition))
        {
            _ = Instantiate(spawnBonus, spawnPosition, Quaternion.identity);
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