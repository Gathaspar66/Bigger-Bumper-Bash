using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitDetection : MonoBehaviour
{
    public Material normal, immune;
    bool isImmune = false;
    float immunityDuration = 3;
    float currentImmunityDuration = 0;

    private void Start()
    {
    }

    private void Update()
    {
        UpdateImmunity();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Wykryto kolizjê z: " + other.gameObject.name, other.gameObject);

        if (other.gameObject.layer == 6)
        {
            EffectManager.instance.SpawnAnEffect(gameObject.transform.position, true);
        }

        if (other.gameObject.layer == 3)
        {
            if (isImmune) return;
            EffectManager.instance.SpawnAnEffect(gameObject.transform.position, false);
            PlayerManager.instance.GetDamaged();
        }
    }


    void UpdateImmunity()
    {
        if (!isImmune) return;

        currentImmunityDuration -= Time.deltaTime;
        if (currentImmunityDuration <= 0)
        {
            EndImmunity();
        }
        else
        {
            bool flash = Mathf.PingPong(currentImmunityDuration * 5, 1) > 0.5f;
            SetCarMaterial(flash ? immune : normal);

            /*
             if(0.75f < currentImmunityDuration && currentImmunityDuration <= 1)
             {
                 SetCarMaterial(normal);
             }
             else if (0.5f < currentImmunityDuration && currentImmunityDuration < 0.75f)
             {
                 SetCarMaterial(immune);
             }
             else if (0.25f < currentImmunityDuration && currentImmunityDuration < 0.5f)
             {
                 SetCarMaterial(normal);
             }
             else if (currentImmunityDuration < 0.25f)
             {
                 SetCarMaterial(immune);
             }
             */
        }
    }

    public void StartImmunity()
    {
        currentImmunityDuration = immunityDuration;
        SetCarMaterial(immune);
        SetCarImmune(true);
    }

    void EndImmunity()
    {
        SetCarMaterial(normal);
        SetCarImmune(false);
    }

    void SetCarImmune(bool value)
    {
        isImmune = value;
    }

    void SetCarMaterial(Material materialToSet)
    {
        transform.GetChild(0).transform.Find("Body").GetComponent<MeshRenderer>().material = materialToSet;
    }
}