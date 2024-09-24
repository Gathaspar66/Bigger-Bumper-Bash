using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCarSpawner : MonoBehaviour
{
    public GameObject dynamicCarPrefab;    
    public Transform[] lanePositions;      
    public Transform playerCarTransform;    
    public float spawnDistance = 50f;        
    public float spawnInterval = 3f;       

    void Start()
    {
        StartCoroutine(SpawnDynamicCars());
    }
  
    IEnumerator SpawnDynamicCars()
    {
        while (true)
        {
            SpawnDynamicCar();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnDynamicCar()
    {
      

     
        int randomLaneIndex = Random.Range(0, lanePositions.Length);
        Transform lane = lanePositions[randomLaneIndex];

     
        Vector3 spawnPosition = new Vector3(lane.position.x, lane.position.y, playerCarTransform.position.z + spawnDistance);

      
        GameObject car = Instantiate(dynamicCarPrefab, spawnPosition, Quaternion.identity);

     
        CarAIMoving carAI = car.GetComponent<CarAIMoving>();
        carAI.lanes = lanePositions;
        carAI.SetPlayerCarTransform(playerCarTransform);
    }
}
