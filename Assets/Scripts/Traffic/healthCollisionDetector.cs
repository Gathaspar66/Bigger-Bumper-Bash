using UnityEngine;

public class healthCollisionDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        //explosionScript = GetComponent<Explosion>();
        // PointsManager.instance.AddPoints(1000);
        //explosionScript.Explode(Vector3.forward);
        //EffectManager.instance.SpawnAnEffect(Effect.BARREL, transform.position);
        PlayerManager.instance.AddHealth();
        GameObject tmp = EffectManager.instance.SpawnAnEffect(Effect.CAR_REPAIR, PlayerManager.instance.GetCarInstance().transform.position);
        tmp.transform.parent = PlayerManager.instance.GetCarInstance().transform;
        Destroy(gameObject);
    }
}
