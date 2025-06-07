using UnityEngine;

public class PointsCollisionDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        //explosionScript = GetComponent<Explosion>();
        PointsManager.instance.AddPoints(1000);
        //explosionScript.Explode(Vector3.forward);
        EffectManager.instance.SpawnAnEffect(Effect.BARREL, transform.position);
        Destroy(gameObject);
    }
}
