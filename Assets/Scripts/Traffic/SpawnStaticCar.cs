using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStaticCar : MonoBehaviour
{
    public GameObject[] carPrefabs;

    void Start()
    {
        SpawnRandomCar();
    }

    void Update()
    {
        DestroyCarIfTooFar();
    }

    void SpawnRandomCar()
    {
        int randomIndex = Random.Range(0, carPrefabs.Length);


        GameObject spawnedCar = Instantiate(carPrefabs[randomIndex], transform.position, transform.rotation);

        spawnedCar.transform.SetParent(transform);
    }

    void DestroyCarIfTooFar()
    {
        GameObject player = PlayerManager.instance.GetPlayerInstance();
        if (transform.position.z < player.transform.position.z - 100f)
        {
            Destroy(gameObject);
        }
    }
}