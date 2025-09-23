using System.Collections.Generic;
using UnityEngine;

public class NearMissCollider : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset = Vector3.zero;
    public float scaleMultiplier = 1.5f;
    private int nearMissCount = 0;
    private BoxCollider nearMiss;
    public Queue<GameObject> ignoredObjects = new();

    private void Start()
    {

        player = PlayerManager.instance.GetPlayerInstance();
        if (player == null)
        {
            Debug.LogError("Nie znaleziono gracza!");
            return;
        }

        transform.position = player.transform.position + offset;


        CreateNearMissCollider();
    }

    private void LateUpdate()
    {
        if (player == null)
        {
            return;
        }

        transform.position = player.transform.position + offset;

    }

    private void CreateNearMissCollider()
    {
        nearMiss = GetComponent<BoxCollider>();
        if (nearMiss == null)
        {
            nearMiss = gameObject.AddComponent<BoxCollider>();
        }

        nearMiss.isTrigger = true;

        BoxCollider playerCollider = player.GetComponent<BoxCollider>();
        if (playerCollider == null)
        {

            playerCollider = new BoxCollider
            {
                size = new Vector3(2f, 1f, 4f)
            };
        }

        nearMiss.center = playerCollider.center;


        Vector3 newSize = new(
            playerCollider.size.x * 1.35f,
            playerCollider.size.y * 1.25f,
            playerCollider.size.z * 1.25f
        );

        nearMiss.size = newSize;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.CompareTag("Player")) return;
        if (PlayerManager.instance.GetIsPlayerImmune()) return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Traffic"))
        {
            if (ignoredObjects.Contains(other.gameObject))
            {
                return;
            }
            ignoredObjects.Enqueue(other.gameObject);
            if (ignoredObjects.Count > 10)
            {
                _ = ignoredObjects.Dequeue();
            }
            nearMissCount++;
            GameObject tmp = EffectManager.instance.SpawnAnEffect(Effect.FLOATING_TEXT, transform.position);
            PointsManager.instance.AddPoints(100);
        }
    }
}
