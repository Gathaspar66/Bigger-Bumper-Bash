using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCarSpawner : MonoBehaviour
{
    public GameObject dynamicCarPrefab;
    public Transform[] lanePositions;
    public Transform playerCarTransform;
    public float spawnDistance = 50f;

    public float spawnIntervalDistance = 50f;
    GameObject car;
    private Rigidbody rb;
    public float speed;
    public float lastSpawnPosition;

    void Start()
    {
        car = ControlsCameraChoice.instance.GetCar();
        rb = car.GetComponent<Rigidbody>();
        //StartCoroutine(SpawnStaticCars());
        lastSpawnPosition = car.transform.position.z;
    }

    /* IEnumerator SpawnDynamicCars()
     {
         while (true)
         {
             SpawnDynamicCar();
             yield return new WaitForSeconds(spawnInterval);
         }
     }
    */
    void Update()
    {
        speed = rb.velocity.z;

        float distanceTraveled = car.transform.position.z - lastSpawnPosition;


        if (distanceTraveled >= spawnIntervalDistance)
        {
            SpawnDynamicCar();
            lastSpawnPosition = car.transform.position.z;
        }
    }


    void SpawnDynamicCar()
    {
        int randomLaneIndex = Random.Range(0, lanePositions.Length);
        Transform lane = lanePositions[randomLaneIndex];


        Vector3 spawnPosition =
            new Vector3(lane.position.x, lane.position.y, playerCarTransform.position.z + spawnDistance);


        GameObject car = Instantiate(dynamicCarPrefab, spawnPosition, Quaternion.identity);


        CarAIMoving carAI = car.GetComponent<CarAIMoving>();
        carAI.lanes = lanePositions;
        carAI.SetPlayerCarTransform(playerCarTransform);
    }
}