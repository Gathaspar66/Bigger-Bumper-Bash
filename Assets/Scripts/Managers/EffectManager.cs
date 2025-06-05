using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;
    public GameObject effect;
    private GameObject currentEffect;
    public GameObject crashEffect;

    private void Awake()
    {
        instance = this;
    }


    public void SpawnAnEffect(Vector3 location, bool isPositive = true)
    {
        // if (currentEffect != null)
        // {
        //     Destroy(currentEffect);
        // }

        //  currentEffect = Instantiate(effect, location, Quaternion.identity);
        //   currentEffect.transform.parent = PlayerManager.instance.GetCameraInstance().transform;
        //   currentEffect.GetComponent<Effect>().Activate(isPositive);
    }

    public void SpawnCrashEffect(Vector3 location, bool isPositive = false)
    {
        currentEffect = Instantiate(crashEffect, location, Quaternion.identity);

        ParticleSystem particleSystem = currentEffect.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            particleSystem.Play();
        }
    }
}