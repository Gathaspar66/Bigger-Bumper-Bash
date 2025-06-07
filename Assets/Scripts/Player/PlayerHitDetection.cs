using UnityEngine;

public class PlayerHitDetection : MonoBehaviour
{
    public Material normal, immune;
    private bool isImmune = false;
    private readonly float immunityDuration = 3;
    private float currentImmunityDuration = 0;

    public ParticleSystem sparksL, sparksR;
    private GameObject playerCarPrefab;
    public TrailRenderer leftTrailRenderer;
    public TrailRenderer rightTrailRenderer;
    public Vector2 input;
    public GameObject leftTireTrail;
    public GameObject rightTireTrail;
    public float minForwardVelocity;
    public float maxForwardVelocity;

    private void Start()
    {
        leftTrailRenderer.emitting = false;
        rightTrailRenderer.emitting = false;
        minForwardVelocity = PlayerSteering.instance.minForwardVelocity;
        maxForwardVelocity = PlayerSteering.instance.maxForwardVelocity;
    }

    private void Update()
    {
        UpdateImmunity();
        UpdateTrailEffects();
    }

    public void UpdateTrailEffects()
    {
        Transform carTransform = PlayerManager.instance.GetPlayerInstance().transform;
        _ = carTransform.position;

        float currentVelocity = PlayerSteering.instance.rb.velocity.z;

        bool accelerating = input.y > 0 && currentVelocity < maxForwardVelocity;
        bool braking = input.y < 0 && currentVelocity > minForwardVelocity;

        bool shouldEmit = accelerating || braking;

        if (leftTrailRenderer != null)
        {
            leftTrailRenderer.emitting = shouldEmit;
        }

        if (rightTrailRenderer != null)
        {
            rightTrailRenderer.emitting = shouldEmit;
        }
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

        EffectManager.instance.SpawnCrashEffect(Effect.CRASH, hitPoint);
        if (other.gameObject.layer == 6)
        {
            SoundManager.instance.PlaySFX("unlock");
        }

        if (other.gameObject.layer == 3)
        {
            CameraShake.Instance.Shake(0.2f, 0.1f);
            SoundManager.instance.PlaySFX("crash");

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

    public void SetInput(Vector2 inputVector)
    {
        inputVector.Normalize();
        input = inputVector;
    }

    private void SetCarMaterial(Material materialToSet)
    {
        playerCarPrefab.transform.Find("body").GetComponent<MeshRenderer>().material = materialToSet;
    }
}