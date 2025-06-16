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

    [Header("Car Materials")] //
    public List<Material> carPaintMaterials;

    public Material glowingFrontLight;

    public Material glowingRearLight;

    private Renderer frontLightRenderer;
    private Renderer rearLightRenderer;

    public Material originalMaterial;
    public Material originalRearMaterial;

    private bool isBlinking = false;

    public Animator unikaczAnimator;

    PlayerPrefabHandler pph;

    public void SetupAICarModel()
    {
        ChangeCarBodyColor();
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
            EffectManager.instance.SpawnAnEffect(Effect.CRASH_AND_FIRE,
                pph.gameObject.transform.position + new Vector3(0, 0.750999987f, 1.20599997f));
        }
        
        if (pph != null) pph.SetSmokeParticle(health == 1);
    }

    void SetCarDamagedState(List<GameObject> list, bool enable)
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
        _ = StartCoroutine(BlinkCoroutine());
    }

    private IEnumerator BlinkCoroutine()
    {
        isBlinking = true;

        for (int i = 0; i < 3; i++)
        {
            frontLightRenderer.material = glowingFrontLight;
            yield return new WaitForSeconds(0.1f);

            frontLightRenderer.material = originalMaterial;
            yield return new WaitForSeconds(0.1f);
        }

        frontLightRenderer.material = originalMaterial;
        isBlinking = false;
    }

    public void SetRearBrakeLight(bool state)
    {
        rearLightRenderer = rearLight.GetComponent<Renderer>();

        rearLightRenderer.material = state ? glowingRearLight : originalRearMaterial;
    }

    public void ChangeCarBodyColor()
    {
        frontLightRenderer = frontLight.GetComponent<Renderer>();

        Material baseMaterial = carPaintMaterials[Random.Range(0, carPaintMaterials.Count)];
        foreach (GameObject currentCarBodyElement in carBody)
        {
            Renderer renderer = currentCarBodyElement.GetComponent<Renderer>();
            Material matInstance = new(baseMaterial);
            renderer.material = matInstance;
        }
    }

    public void SetImmuneCarMaterial(Material materialToSet)
    {
        foreach (GameObject currentCarBodyElement in carBody)
        {
            MeshRenderer carRenderer = currentCarBodyElement.GetComponent<MeshRenderer>();
            carRenderer.material = materialToSet;
        }
    }
}