using UnityEngine;

public class PlayerHitDetection : MonoBehaviour
{
    public Material normal, immune;
    private bool isImmune = false;
    private readonly float immunityDuration = 3;
    private float currentImmunityDuration = 0;

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
            SoundManager.instance.PlaySFX("unlock");
        }

        if (other.gameObject.layer == 3)
        {
            if (isImmune)
            {
                return;
            }
            SoundManager.instance.PlaySFX("crash");
            EffectManager.instance.SpawnAnEffect(gameObject.transform.position, false);
            PlayerManager.instance.GetDamaged();
        }
    }

    private void UpdateImmunity()
    {
        if (!isImmune)
        {
            return;
        }

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

    private void EndImmunity()
    {
        SetCarMaterial(normal);
        SetCarImmune(false);
    }

    private void SetCarImmune(bool value)
    {
        isImmune = value;
    }

    private void SetCarMaterial(Material materialToSet)
    {
        transform.GetChild(0).transform.Find("body").GetComponent<MeshRenderer>().material = materialToSet;
    }
}