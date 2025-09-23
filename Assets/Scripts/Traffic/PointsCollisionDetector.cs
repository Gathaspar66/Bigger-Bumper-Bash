using UnityEngine;

public class PointsCollisionDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        //explosionScript = GetComponent<Explosion>();
        PointsManager.instance.CrateHit();
        //explosionScript.Explode(Vector3.forward);
        _ = EffectManager.instance.SpawnAnEffect(Effect.BARREL, transform.position);
        GameObject tmp = EffectManager.instance.SpawnAnEffect(Effect.FLOATING_TEXT, transform.position);
        tmp.GetComponent<FloatingText>().SetFloatingText("+10", PlayerManager.instance.GetPlayerInstance(),
                 PlayerManager.instance.selectedCarData.carPrefab
                 .GetComponent<CarModelHandler>().fireSmokeSource.transform.position);
        Destroy(gameObject);
    }
}
