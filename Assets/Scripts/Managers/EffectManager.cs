using UnityEngine;

public enum Effect
{
    CRASH,
}

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    private GameObject currentEffect;

    //reference to an effect prefab, add to enum class and call in the switch
    public GameObject crashEffect;

    private void Awake()
    {
        instance = this;
    }

    public void SpawnCrashEffect(Effect effect, Vector3 location)
    {
        switch (effect)
        {
            case Effect.CRASH:
                currentEffect = Instantiate(crashEffect, location, Quaternion.identity);
                break;
        }

        //effectively the same as play on awake, redundant?
        ParticleSystem particleSystem = currentEffect.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            particleSystem.Play();
        }
    }
}