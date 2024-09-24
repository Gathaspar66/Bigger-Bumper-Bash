using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject staticCarPrefab;     
    public Transform[] lanePositions;        
    public Transform playerCarTransform;    
    public float spawnDistance = 50f;       
    public float spawnInterval = 3f;        

    void Start()
    {
        StartCoroutine(SpawnStaticCars());
    }

    IEnumerator SpawnStaticCars()
    {
        while (true)
        {
            SpawnStaticCar();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnStaticCar()
    {
        
        int randomLaneIndex = Random.Range(0, lanePositions.Length);
        Transform lane = lanePositions[randomLaneIndex];

      
        Vector3 spawnPosition = new Vector3(lane.position.x, lane.position.y, playerCarTransform.position.z + spawnDistance);

      
        Instantiate(staticCarPrefab, spawnPosition, Quaternion.identity);
    }
}
