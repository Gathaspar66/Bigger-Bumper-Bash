using UnityEngine;

public class PointsCollisionDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        //explosionScript = GetComponent<Explosion>();
        PointsManager.instance.AddPoints(10);
        //explosionScript.Explode(Vector3.forward);
        EffectManager.instance.SpawnAnEffect(Effect.BARREL, transform.position);
        GameObject tmp = EffectManager.instance.SpawnAnEffect(Effect.FLOATING_TEXT, transform.position);
        tmp.GetComponent<FloatingText>().SetFloatingText("combo bonus! +10", PlayerManager.instance.GetPlayerInstance(),
                 PlayerManager.instance.selectedCarData.carPrefab
                 .GetComponent<CarModelHandler>().fireSmokeSource.transform.position);
        Destroy(gameObject);
    }
}
