using UnityEngine;

public class SpawnHealth : MonoBehaviour
{

    public GameObject[] HealthMeshes;

    private void Start()
    {
        SpawnRandomPointsMesh();
    }

    private void Update()
    {
        DestroyPointsIfTooFar();
    }

    private void SpawnRandomPointsMesh()
    {
        int randomIndex = Random.Range(0, HealthMeshes.Length);


        GameObject spawnedPoints = Instantiate(HealthMeshes[randomIndex], transform.position, transform.rotation);

        spawnedPoints.transform.SetParent(transform);
    }

    private void DestroyPointsIfTooFar()
    {
        GameObject player = PlayerManager.instance.GetPlayerInstance();
        if (transform.position.z < player.transform.position.z - 100f)
        {
            Destroy(gameObject);
        }
    }

}
