using UnityEngine;

public class PlayerHitDetection : MonoBehaviour
{
    public Material normal, immune;
    private bool isImmune = false;
    private readonly float immunityDuration = 3;
    private float currentImmunityDuration = 0;
    private float lastBarrierEffectTime = -10f;
    private readonly float barrierCooldown = 1f;

    private void Start()
    {
    }

    private void Update()
    {
        UpdateImmunity();
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 hitPoint = transform.position;

        if (other is BoxCollider || other is SphereCollider || other is CapsuleCollider ||
            (other is MeshCollider mc && mc.convex))
        {
            hitPoint = other.ClosestPoint(transform.position);
        }

        EffectManager.instance.SpawnCrashEffect(hitPoint, true);
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
            CameraShake.Instance.Shake(0.2f, 0.1f);
            SoundManager.instance.PlaySFX("crash");
            EffectManager.instance.SpawnAnEffect(gameObject.transform.position, false);
            PlayerManager.instance.GetDamaged();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Barrier"))
        {
            if (Time.time - lastBarrierEffectTime >= barrierCooldown)
            {
                lastBarrierEffectTime = Time.time;

                Vector3 hitPoint = transform.position;
                if (other is BoxCollider || other is SphereCollider || other is CapsuleCollider ||
                    (other is MeshCollider mc && mc.convex))
                {
                    hitPoint = other.ClosestPoint(transform.position);
                }

                EffectManager.instance.SpawnCrashEffect(hitPoint, true);
            }
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