using UnityEngine;

public class NearMissCollider : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset = Vector3.zero;
    public float scaleMultiplier = 1.5f;
    private int nearMissCount = 0;
    private BoxCollider nearMiss;

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
            playerCollider.size.x * 2.0f,
            playerCollider.size.y * 1.2f,
            playerCollider.size.z * 1.2f
        );

        nearMiss.size = newSize;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            return;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Traffic"))
        {
            nearMissCount++;
            Debug.Log("NEAR MISS with: " + other.name + " | Count: " + nearMissCount);
        }
    }
}
