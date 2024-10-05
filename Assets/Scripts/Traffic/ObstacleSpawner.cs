using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject staticCarPrefab;
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
        lastSpawnPosition=car.transform.position.z;
    }

    /* IEnumerator SpawnStaticCars()
     {
         while (true)
         {
             SpawnStaticCar();
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
                SpawnStaticCar();
                lastSpawnPosition = car.transform.position.z;
            }


        
    }


    void SpawnStaticCar()
    {
        int randomLaneIndex = Random.Range(0, lanePositions.Length);
        Transform lane = lanePositions[randomLaneIndex];


        Vector3 spawnPosition =
            new Vector3(lane.position.x, lane.position.y, playerCarTransform.position.z + spawnDistance);


        GameObject newCar = Instantiate(staticCarPrefab, spawnPosition, Quaternion.identity);


        BlueCarDestroyer blueCarDestroyer = newCar.GetComponent<BlueCarDestroyer>();
        if (blueCarDestroyer != null)
        {
            blueCarDestroyer.playerCarTransform = playerCarTransform;
        }
    }
}