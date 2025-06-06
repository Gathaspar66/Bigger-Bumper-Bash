using UnityEngine;

public class PlayerHitDetection : MonoBehaviour
{
    public Material normal, immune;
    private bool isImmune = false;
    private readonly float immunityDuration = 3;
    private float currentImmunityDuration = 0;
    private readonly float lastBarrierEffectTime = -10f;
    private readonly float barrierCooldown = 1f;

    public ParticleSystem sparksL, sparksR;
    private GameObject playerCarPrefab;

    private void Start()
    {
    }

    private void Update()
    {
        UpdateImmunity();
    }

    public void SetCarPrefab(GameObject playerCarPrefab)
    {
        this.playerCarPrefab = playerCarPrefab;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isImmune)
        {
            return;
        }

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

            CameraShake.Instance.Shake(0.2f, 0.1f);
            SoundManager.instance.PlaySFX("crash");
            EffectManager.instance.SpawnAnEffect(gameObject.transform.position, false);
            PlayerManager.instance.GetDamaged();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("LeftBarrier"))
        {
            sparksL.Play();
        }
        if (other.CompareTag("RightBarrier"))
        {
            sparksR.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LeftBarrier"))
        {
            sparksL.Stop();
        }
        if (other.CompareTag("RightBarrier"))
        {
            sparksR.Stop();
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
        playerCarPrefab.transform.Find("body").GetComponent<MeshRenderer>().material = materialToSet;
    }
}