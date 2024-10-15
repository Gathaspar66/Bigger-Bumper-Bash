using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficManager : MonoBehaviour
{
    [Header("Static Obstacle Spawn Settings")] //
    public float spawnStaticObstacleDistance = 50f;
    public float staticObstacleSpawnIntervalDistance = 50f;

    [Header("Dynamic Obstacle Spawn Settings")] //
    public float spawnDynamicObstacleDistance = 50f;
    public float dynamicObstacleSpawnIntervalDistance = 50f;

    [Header("Other")] //
    public GameObject staticCarPrefab;
    public Transform[] lanePositions;
    GameObject car;
    private Rigidbody rb;
    public float speed;
    public float lastSpawnPosition;

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
        if (distanceTraveled >= dynamicObstacleSpawnIntervalDistance)
        {
            //SpawnDynamicCar();
            lastSpawnPosition = car.transform.position.z;
        }


    }


    void SpawnObstacleCar()
    {
        int randomLaneIndex = Random.Range(0, lanePositions.Length);
        Transform lane = lanePositions[randomLaneIndex];


        Vector3 spawnPosition =
            new Vector3(lane.position.x, lane.position.y, car.transform.position.z + spawnStaticObstacleDistance);


        GameObject newCar = Instantiate(staticCarPrefab, spawnPosition, Quaternion.identity);


        //BlueCarDestroyer blueCarDestroyer = newCar.GetComponent<BlueCarDestroyer>();
        // //if (blueCarDestroyer != null)
        // {
        //     blueCarDestroyer.playerCarTransform = playerCarTransform;
        // }
    }
    /*
    void SpawnDynamicCar()
    {
        int randomLaneIndex = Random.Range(0, lanePositions.Length);
        int randomPrefab = Random.Range(0, carPrefabs.Length);
        Transform lane = lanePositions[randomLaneIndex];


        Vector3 spawnPosition =
            new Vector3(lane.position.x, lane.position.y, playerCarTransform.position.z + spawnDistance);


        GameObject car = Instantiate(carPrefabs[randomPrefab], spawnPosition, Quaternion.identity);


        // CarAIMoving carAI = car.GetComponent<CarAIMoving>();
        //carAI.lanes = lanePositions;
        //carAI.SetPlayerCarTransform(playerCarTransform);
    }

    */

}