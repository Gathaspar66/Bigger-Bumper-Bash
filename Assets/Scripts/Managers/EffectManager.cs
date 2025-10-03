using System.Collections;
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

    public GameObject crashEffect;
    public GameObject barrelEffect;
    public GameObject crashAndFireEffect;
    public GameObject floatingTextEffect;
    public Light directionalLight;
    [SerializeField] private float normalIntensity = 1f;
    [SerializeField] private float dimIntensity = 0.5f;
    [SerializeField] private float fadeSpeed = 1f;

    private void Awake()
    {
        instance = this;
    }

    public void DimLight()
    {
        if (directionalLight != null)
        {
            _ = StartCoroutine(FadeLight(dimIntensity));
        }
    }

    public void RestoreLight()
    {
        if (directionalLight != null)
        {
            _ = StartCoroutine(FadeLight(normalIntensity));
        }
    }

    private IEnumerator FadeLight(float targetIntensity)
    {
        float startIntensity = directionalLight.intensity;
        float t = 0f;
        while (t < 1f)
        {
            directionalLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, t);
            t += Time.deltaTime * fadeSpeed;
            yield return null;
        }
        directionalLight.intensity = targetIntensity;
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

        ParticleSystem particleSystem = currentEffect.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            particleSystem.Play();
        }
        return currentEffect;
    }
}