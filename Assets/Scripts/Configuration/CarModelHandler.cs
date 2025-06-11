using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarModelHandler : MonoBehaviour
{


    [Header("Car Elements")] //
    public GameObject carBody;

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

    public void Activate()
    {
        ChangeCarBodyColor();
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

        Renderer renderer = carBody.GetComponent<Renderer>();

        Material baseMaterial = carPaintMaterials[Random.Range(0, carPaintMaterials.Count)];
        Material matInstance = new(baseMaterial);
        renderer.material = matInstance;
    }

    public void SetImmuneCarMaterial(Material materialToSet)
    {
        MeshRenderer carRenderer = carBody.GetComponent<MeshRenderer>();

        carRenderer.material = materialToSet;
    }
}