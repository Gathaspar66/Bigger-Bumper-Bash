using UnityEngine;

public class PointsCollisionDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PointsManager.instance.CrateHit();
        _ = EffectManager.instance.SpawnAnEffect(Effect.BARREL, transform.position);
        Destroy(gameObject);
    }
}
