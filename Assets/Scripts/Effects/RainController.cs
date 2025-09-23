using System.Collections;
using UnityEngine;

public class RainController : MonoBehaviour
{
    [Header("Particle System")]
    [SerializeField] private ParticleSystem rainSystem;

    [Header("Timing (seconds)")]
    [SerializeField] private float minDryTime = 120f;

    [SerializeField] private float maxDryTime = 300f;
    [SerializeField] private float rainDuration = 20f;

    [Header("Chance for rain on start")]
    [Range(0f, 1f)]
    [SerializeField] private float startRainChance = 0.1f;

    [Header("Force over Lifetime (Y)")]
    [SerializeField] private float minForceY = 0f;

    [SerializeField] private float maxForceY = -12f;
    [SerializeField] private float maxCarSpeed = 30f;

    [Header("Smooth Appearance")]
    [SerializeField] private float fadeDuration = 1f;

    private ParticleSystem.EmissionModule emissionModule;
    private ParticleSystem.ForceOverLifetimeModule forceModule;

    private void Start()
    {
        emissionModule = rainSystem.emission;
        forceModule = rainSystem.forceOverLifetime;
        forceModule.enabled = true;

        rainSystem.Stop();

        float lastRainTime = PlayerPrefs.GetFloat("LastRainTime", -1f);
        float currentTime = Time.time;

        bool startRainImmediately = Random.value < startRainChance;
        if (startRainImmediately)
        {
            _ = StartCoroutine(StartRainCycle(initial: true));

            PlayerPrefs.SetFloat("LastRainTime", currentTime);
            PlayerPrefs.Save();
        }
        else if (lastRainTime < 0f)
        {
            _ = StartCoroutine(RainCycle());
        }
        else
        {
            float elapsed = currentTime - lastRainTime;
            _ = StartCoroutine(RainCycle(elapsed));
        }
    }

    private IEnumerator RainCycle(float elapsedSinceLast = 0f)
    {
        while (true)
        {
            float waitTime = Random.Range(minDryTime, maxDryTime) - elapsedSinceLast;
            if (waitTime < 0f)
            {
                waitTime = 0f;
            }

            yield return new WaitForSeconds(waitTime);

            yield return StartCoroutine(StartRainCycle());

            elapsedSinceLast = 0f;
        }
    }

    private IEnumerator StartRainCycle(bool initial = false)
    {
        EffectManager.instance?.DimLight();

        rainSystem.Play();

        float targetRate = 500f;
        float t = 0f;
        while (t < fadeDuration)
        {
            emissionModule.rateOverTime = Mathf.Lerp(0, targetRate, t / fadeDuration);
            t += Time.deltaTime;
            yield return null;
        }
        emissionModule.rateOverTime = targetRate;

        PlayerPrefs.SetFloat("LastRainTime", Time.time);
        PlayerPrefs.Save();

        float timer = 0f;
        while (timer < rainDuration)
        {
            float currentSpeed = PlayerSteering.instance.rb.velocity.z;
            float speedFactor = Mathf.Clamp01(currentSpeed / maxCarSpeed);
            float currentForceY = Mathf.Lerp(minForceY, maxForceY, speedFactor);
            forceModule.y = new ParticleSystem.MinMaxCurve(currentForceY);

            timer += Time.deltaTime;
            yield return null;
        }

        t = 0f;
        while (t < fadeDuration)
        {
            emissionModule.rateOverTime = Mathf.Lerp(targetRate, 0, t / fadeDuration);
            t += Time.deltaTime;
            yield return null;
        }
        emissionModule.rateOverTime = 0f;

        rainSystem.Stop();

        EffectManager.instance?.RestoreLight();
    }
}