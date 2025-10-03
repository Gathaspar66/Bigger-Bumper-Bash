using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarModelHandler : MonoBehaviour
{
    [Header("Car Elements")] //
    public List<GameObject> carBody = new();

    public List<GameObject> damagedBase = new();
    public List<GameObject> damagedSmall = new();
    public List<GameObject> damagedBig = new();
    public List<GameObject> damagedDestroyed = new();
    public List<List<GameObject>> listOfListsCarDamageParts = new();

    public GameObject frontLight;
    public GameObject rearLight;

    public GameObject leftSlideSource;
    public GameObject rightSlideSource;
    public GameObject fireSmokeSource;

    [Header("Car Materials")] //
    public List<Material> carPaintMaterials;

    public Material glowingFrontLight;

    public Material glowingRearLight;

    private readonly Renderer frontLightRenderer;
    private Renderer rearLightRenderer;

    public Material originalMaterial;
    public Material originalRearMaterial;

    private bool isBlinking = false;

    public Animator unikaczAnimator;

    private PlayerPrefabHandler pph;

    [HideInInspector]
    public Material normalMaterial;

    [HideInInspector]
    public Material immuneMaterial;

    public void SetupAICarModel()
    {
        SetCarDamagedLists();
        UpdatePlayerDamagedState(3);
    }

    public void SetCarDamagedLists()
    {
        listOfListsCarDamageParts.Add(damagedDestroyed);
        listOfListsCarDamageParts.Add(damagedBig);
        listOfListsCarDamageParts.Add(damagedSmall);
        listOfListsCarDamageParts.Add(damagedBase);
    }

    internal void SetPlayerPrefabHandler(PlayerPrefabHandler playerPrefabHandler)
    {
        pph = playerPrefabHandler;
    }

    public void UpdatePlayerDamagedState(int health)
    {
        foreach (List<GameObject> i in listOfListsCarDamageParts)
        {
            SetCarDamagedState(i, false);
        }
        health = Mathf.Clamp(health, 0, 3); //multihit on death protection
        SetCarDamagedState(listOfListsCarDamageParts[health], true);
        if (health <= 0)
        {
            gameObject.GetComponent<Animator>().speed = 0.1f;
            _ = EffectManager.instance.SpawnAnEffect(Effect.CRASH_AND_FIRE,
                fireSmokeSource.transform.position);
        }

        if (pph != null)
        {
            pph.SetSmokeParticle(health == 1);
        }
    }

    private void SetCarDamagedState(List<GameObject> list, bool enable)
    {
        foreach (GameObject i in list)
        {
            i.SetActive(enable);
        }
    }

    public void BlinkFrontLights()
    {
        if (isBlinking)
        {
            return;
        }

        if (frontLight == null)
        {
            return;
        }

        Renderer flRenderer = frontLight.GetComponent<Renderer>();
        if (flRenderer == null)
        {
            return;
        }

        _ = StartCoroutine(BlinkCoroutine(flRenderer));
    }

    private IEnumerator BlinkCoroutine(Renderer flRenderer)
    {
        isBlinking = true;

        for (int i = 0; i < 3; i++)
        {
            if (flRenderer != null)
            {
                flRenderer.material = glowingFrontLight;
            }

            yield return new WaitForSeconds(0.1f);

            if (flRenderer != null)
            {
                flRenderer.material = originalMaterial;
            }

            yield return new WaitForSeconds(0.1f);
        }

        if (flRenderer != null)
        {
            flRenderer.material = originalMaterial;
        }

        isBlinking = false;
    }

    public void SetRearBrakeLight(bool state)
    {
        rearLightRenderer = rearLight.GetComponent<Renderer>();

        rearLightRenderer.material = state ? glowingRearLight : originalRearMaterial;
    }
}