using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    public GameObject[] PointsMeshes;

    void Start()
    {
        SpawnRandomPointsMesh();
    }

    void Update()
    {
        DestroyPointsIfTooFar();
    }

    void SpawnRandomPointsMesh()
    {
        int randomIndex = Random.Range(0, PointsMeshes.Length);


        GameObject spawnedPoints = Instantiate(PointsMeshes[randomIndex], transform.position, transform.rotation);

        spawnedPoints.transform.SetParent(transform);
    }

    void DestroyPointsIfTooFar()
    {
        GameObject player = PlayerManager.instance.GetPlayerInstance();
        if (transform.position.z < player.transform.position.z - 100f)
        {
            Destroy(gameObject);
        }
    }
}