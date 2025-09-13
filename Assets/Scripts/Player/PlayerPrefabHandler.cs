using UnityEngine;

public class PlayerPrefabHandler : MonoBehaviour
{
    [Header("Car Effects")] //
    public ParticleSystem sparksL;

    public ParticleSystem sparksR;
    public ParticleSystem smokePrefab;
    public TrailRenderer leftTrailRenderer, rightTrailRenderer;

    [Header("Other")] //
    public Material immune;

    private bool isImmune = false;

    private readonly float immunityDuration = 2;
    private float currentImmunityDuration = 0;

    private GameObject playerCarPrefab;

    private Vector2 input;

    private float minForwardVelocity;
    private float maxForwardVelocity;
    private CarModelHandler carModelHandler;
    public CarAIDynamicObstacle carAIDynamicObstacle;
    private bool isPlayerDead = false;
    public GameObject nearMissPrefab;
    private Renderer[] carRenderers;
    private float blinkTimer = 0f;
    private float blinkInterval = 0.1f;

    private void Update()
    {
        UpdateImmunity();
        UpdateTrailEffects();
        UpdateBrakeLights();
    }

    public void SetParameters()
    {
        leftTrailRenderer.emitting = false;
        rightTrailRenderer.emitting = false;
        minForwardVelocity = PlayerSteering.instance.minForwardVelocity;
        maxForwardVelocity = PlayerSteering.instance.maxForwardVelocity;
        carModelHandler = GetComponentInChildren<CarModelHandler>();
        carModelHandler.SetCarDamagedLists();
        carModelHandler.SetPlayerPrefabHandler(this);

        SetupSmokeAndTrailRenderers();
        SpawnNearMiss();
    }

    public void SetCarPrefab(GameObject playerCarPrefab)
    {
        this.playerCarPrefab = playerCarPrefab;
        carRenderers = playerCarPrefab.GetComponentsInChildren<Renderer>(true);
        SetupCollider();
    }

    private void SetupSmokeAndTrailRenderers()
    {
        leftTrailRenderer.transform.position = carModelHandler.leftSlideSource.transform.position;
        rightTrailRenderer.transform.position = carModelHandler.rightSlideSource.transform.position;
        smokePrefab.transform.position = carModelHandler.fireSmokeSource.transform.position;
    }

    public void UpdateTrailEffects()
    {
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
        SoundManager.instance.PlayerCarAccelerateSound(accelerating);
        SoundManager.instance.PlayerCarBreakSound(braking);
    }

    private void UpdateBrakeLights()
    {
        float currentVelocity = PlayerSteering.instance.rb.velocity.z;
        bool isBraking = input.y < 0 && currentVelocity > minForwardVelocity + 0.01f;

        carModelHandler.SetRearBrakeLight(isBraking);
    }

    private void SetupCollider()
    {
        //player prefab handler collider should be smaller than car model handler collider
        BoxCollider pphCol = gameObject.GetComponent<BoxCollider>();
        BoxCollider cmhCol = playerCarPrefab.GetComponent<BoxCollider>();
        pphCol.center = cmhCol.center;
        pphCol.size = cmhCol.size * 0.9f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPlayerDead)
        {
            return;
        }
        if (other.gameObject.layer == 6)
        {
            SoundManager.instance.PlayPointsSound();
            AddCollectedBarrel();
        }
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

        if (other.gameObject.layer == 3)
        {
            CameraShake.Instance.Shake(0.2f, 0.1f);
            SoundManager.instance.PlayCrashSound();
            _ = EffectManager.instance.SpawnAnEffect(Effect.CRASH, hitPoint);
            PlayerManager.instance.GetDamaged();

            CarAIDynamicObstacle aiCar = other.GetComponentInParent<CarAIDynamicObstacle>();
            if (aiCar != null)
            {
                aiCar.StopCarDueToCrash();
            }
        }
    }

    private void AddCollectedBarrel()
    {
        int barrels = PlayerPrefs.GetInt("CollectedBarrels", 0);
        barrels++;
        // print(barrels);
        PlayerPrefs.SetInt("CollectedBarrels", barrels);
        PlayerPrefs.Save();
    }

    private void AddHitBarrier()
    {
        int hitBarriers = PlayerPrefs.GetInt("HitBarriers", 0);
        hitBarriers++;
        PlayerPrefs.SetInt("HitBarriers", hitBarriers);
        PlayerPrefs.Save();
    }

    internal void SetPlayerDead(bool isDead)
    {
        isPlayerDead = isDead;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("LeftBarrier"))
        {
            sparksL.Play();
            SoundManager.instance.StartBarrierScrape();
        }

        if (other.CompareTag("RightBarrier"))
        {
            sparksR.Play();
            SoundManager.instance.StartBarrierScrape();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LeftBarrier"))
        {
            sparksL.Stop();
            SoundManager.instance.StopBarrierScrape();
            AddHitBarrier();
        }

        if (other.CompareTag("RightBarrier"))
        {
            sparksR.Stop();
            SoundManager.instance.StopBarrierScrape();
            AddHitBarrier();
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
            float t = 1 - (currentImmunityDuration / immunityDuration);

            blinkInterval = Mathf.Lerp(0.2f, 0.05f, t);

            blinkTimer -= Time.deltaTime;
            if (blinkTimer <= 0f)
            {
                bool currentlyVisible = carRenderers[0].enabled;
                SetCarRenderersVisible(!currentlyVisible);

                blinkTimer = blinkInterval;
            }
        }
    }

    private void SetCarRenderersVisible(bool visible)
    {
        if (carRenderers == null)
        {
            return;
        }

        foreach (Renderer r in carRenderers)
        {
            if (r != null)
            {
                r.enabled = visible;
            }
        }
    }

    public void StartImmunity()
    {
        currentImmunityDuration = immunityDuration;
        SetCarImmune(true);
    }

    private void EndImmunity()
    {
        SetCarRenderersVisible(true);
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

    public void UpdatePlayerDamagedState(int health)
    {
        carModelHandler.UpdatePlayerDamagedState(health);
    }

    public void SetSmokeParticle(bool ifActive)
    {
        if (ifActive)
        {
            smokePrefab.Play();
        }
        else
        {
            smokePrefab.Stop();
        }
    }

    public void SpawnNearMiss()
    {
        _ = Instantiate(nearMissPrefab);
    }
}