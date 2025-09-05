using UnityEngine;

public enum Effect
{
    CRASH,
    BARREL,
    CRASH_AND_FIRE,
    FLOATING_TEXT,
}

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    private GameObject currentEffect;

    //reference to an effect prefab, add to enum class and call in the switch
    public GameObject crashEffect;
    public GameObject barrelEffect;
    public GameObject crashAndFireEffect;
    public GameObject floatingTextEffect;

    private void Awake()
    {
        instance = this;
    }

    public GameObject SpawnAnEffect(Effect effect, Vector3 location)
    {
        switch (effect)
        {
            case Effect.CRASH:
                currentEffect = Instantiate(crashEffect, location, Quaternion.identity);
                break;

            case Effect.BARREL:
                currentEffect = Instantiate(barrelEffect, location, Quaternion.identity);
                break;
            case Effect.CRASH_AND_FIRE:
                currentEffect = Instantiate(crashAndFireEffect, location, Quaternion.identity);
                break;
            case Effect.FLOATING_TEXT:
                currentEffect = Instantiate(floatingTextEffect, location, Quaternion.identity);
                break;
        }

        //effectively the same as play on awake, redundant?
        ParticleSystem particleSystem = currentEffect.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            particleSystem.Play();
        }
        return currentEffect;
    }
}