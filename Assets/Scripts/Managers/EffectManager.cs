using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    private GameObject currentEffect;
    public GameObject crashEffect;

    private void Awake()
    {
        instance = this;
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