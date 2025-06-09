using System.Collections.Generic;
using UnityEngine;

public class CarModelHandler : MonoBehaviour
{
    public List<Material> carPaintMaterials;
    public Vector2 smoothnessRange = new(0.5f, 0.8f);
    public Vector2 metallicRange = new(0.6f, 1f);
    public GameObject carBody;

    public void Activate()
    {
        ChangeCarBodyColor();
    }

    public void ChangeCarBodyColor()
    {
        Transform bodyTransform = carBody.transform;
        if (bodyTransform != null)
        {
            Renderer renderer = bodyTransform.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material baseMaterial = carPaintMaterials[Random.Range(0, carPaintMaterials.Count)];
                Material matInstance = new(baseMaterial);
                matInstance.SetFloat("_Glossiness", Random.Range(smoothnessRange.x, smoothnessRange.y));
                matInstance.SetFloat("_Metallic", Random.Range(metallicRange.x, metallicRange.y));
                renderer.material = matInstance;
            }
        }
    }
}