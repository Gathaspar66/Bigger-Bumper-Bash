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

    private Renderer frontLightRenderer;
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

    public void SetupImmuneMaterial()
    {
        Material baseMat = carBody[0].GetComponent<Renderer>().material;

        normalMaterial = baseMat;

        immuneMaterial = new Material(baseMat);

        immuneMaterial.SetFloat("_Mode", 3);
        immuneMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        immuneMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        immuneMaterial.SetInt("_ZWrite", 0);
        immuneMaterial.DisableKeyword("_ALPHATEST_ON");
        immuneMaterial.EnableKeyword("_ALPHABLEND_ON");
        immuneMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        immuneMaterial.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

        Color c = baseMat.color;
        c.a = 128f / 255f;
        immuneMaterial.color = c;

        if (immuneMaterial.HasProperty("_Metallic"))
        {
            immuneMaterial.SetFloat("_Metallic", 0f);
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