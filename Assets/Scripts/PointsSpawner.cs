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

    void Start()
    {
        StartCoroutine(SpawnStaticCars());
    }

    IEnumerator SpawnStaticCars()
    {
        while (true)
        {
            SpawnPointsPrefab();
            yield return new WaitForSeconds(spawnInterval);
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