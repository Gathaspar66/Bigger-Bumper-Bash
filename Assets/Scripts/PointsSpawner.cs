using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsSpawner : MonoBehaviour
{
    public GameObject pointsPrefab;
    public Transform[] lanePositions;
    public Transform playerCarTransform;
    public float spawnDistance = 50f;
    public float spawnInterval = 1f;
    public float spawnIntervalDistance = 50f;
    GameObject car;
    private Rigidbody rb;
    public float speed;
    public float lastSpawnPosition;

    /*
    IEnumerator SpawnStaticCars()
    {
        while (true)
        {
            SpawnPointsPrefab();
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    */

    void Start()
    {
        car = ControlsCameraChoice.instance.GetCar();
        rb = car.GetComponent<Rigidbody>();
        //StartCoroutine(SpawnStaticCars());
        lastSpawnPosition = car.transform.position.z;
    }

    void Update()
    {
        speed = rb.velocity.z;

        float distanceTraveled = car.transform.position.z - lastSpawnPosition;


        if (distanceTraveled >= spawnIntervalDistance)
        {
            SpawnPointsPrefab();
            lastSpawnPosition = car.transform.position.z;
        }
    }


    void SpawnPointsPrefab()
    {
        int randomLaneIndex = Random.Range(0, lanePositions.Length);
        Transform lane = lanePositions[randomLaneIndex];


        Vector3 spawnPosition =
            new Vector3(lane.position.x, lane.position.y, playerCarTransform.position.z + spawnDistance);


        GameObject newPointsObject = Instantiate(pointsPrefab, spawnPosition, Quaternion.identity);


        PointsHandler pointsHandler = newPointsObject.GetComponent<PointsHandler>();
        if (pointsHandler != null)
        {
            pointsHandler.playerCarTransform = playerCarTransform;
        }
    }
}