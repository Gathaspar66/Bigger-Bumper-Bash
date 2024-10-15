using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;
    public GameObject effect;
    GameObject currentEffect;

    private void Awake()
    {
        instance = this;
    }


    public void SpawnAnEffect(Vector3 location, bool isPositive = true)
    {
        if (currentEffect != null)
        {
            Destroy(currentEffect);
        }

        currentEffect = Instantiate(effect, location, Quaternion.identity);
        currentEffect.transform.parent = PlayerManager.instance.GetCameraInstance().transform;
        currentEffect.GetComponent<Effect>().Activate(isPositive);
    }
}