using UnityEngine;

public class CarAIDynamicObstacle : MonoBehaviour
{
    private float speed;
    private float brakeRaycastDistance;

    private readonly float raycastOffsetY = 0.5f;
    private int currentLaneIndex;
    private float maxSpeed;
    private float minSpeed;
    private float spawnMinSpeed;
    private float spawnMaxSpeed;
    public GameObject[] carPrefabs;
    public static CarAIDynamicObstacle instance;
    public float minBrakeRaycastDistance;
    public float maxBrakeRaycastDistance;
    private readonly float forwardOffset = 0.5f;
    private Vector3 boxSize;

    public LayerMask trafficLayer;
    private Collider carCollider;

    private Vector2 smoothnessRange = new(0.5f, 0.8f);
    private Vector2 metallicRange = new(0.6f, 1f);
    private bool playerDetected = false;
    private CarModelHandler carModelHandler;
    private bool isStoppedDueToCrash = false;
    public static EffectManager effectManager;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        BrakeRaycastSetup();
        SetInitialSpeed();
        GetComponents();
    }

    private void GetComponents()
    {
        carCollider = GetComponentInChildren<Collider>();
        carModelHandler = GetComponentInChildren<CarModelHandler>();
    }

    private void BrakeRaycastSetup()
    {
        brakeRaycastDistance = Random.Range(minBrakeRaycastDistance, maxBrakeRaycastDistance);
    }

    private void FixedUpdate()
    {
        DetectOtherCarsAndBrake();

        DestroyCarIfTooFar();
    }

    private void Update()
    {
        MoveCar();
        _ = GetComponentInChildren<CarModelHandler>();
    }

    private void SetInitialSpeed()
    {
        maxSpeed = PlayerSteering.instance.maxForwardVelocity;
        minSpeed = PlayerSteering.instance.minForwardVelocity;
        spawnMinSpeed = minSpeed * 0.9f;
        spawnMaxSpeed = maxSpeed * 1.1f;

        speed = GenerateSpeed();

        maxSpeed = speed;
    }

    private float GenerateSpeed()
    {
        do
        {
            speed = Random.Range(spawnMinSpeed, spawnMaxSpeed);
        } while (speed > minSpeed + 1 || speed < minSpeed - 1);

        return speed;
    }

    public void SpawnRandomCarModel(int randomLaneIndex)
    {
        int randomIndex = Random.Range(0, carPrefabs.Length);
        currentLaneIndex = randomLaneIndex;

        Vector3 spawnPosition = new(transform.position.x, transform.position.y, transform.position.z);

        GameObject spawnedCar = Instantiate(carPrefabs[randomIndex], spawnPosition, transform.rotation);
        spawnedCar.transform.SetParent(transform);

        spawnedCar.GetComponent<CarModelHandler>().Activate();
    }

    private void DestroyCarIfTooFar()
    {
        GameObject player = PlayerManager.instance.GetPlayerInstance();
        if (transform.position.z < player.transform.position.z - 100f)
        {
            Destroy(gameObject);
        }
    }

    private void MoveCar()
    {
        if (!isStoppedDueToCrash)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    private void DetectOtherCarsAndBrake()
    {
        Vector3 carSize = carCollider.bounds.size;
        float halfCarLength = carSize.z / 2f;

        Vector3 rayOrigin = transform.position + (Vector3.up * raycastOffsetY);
        Vector3 rayDirection = gameObject.transform.forward;

        Vector3 boxCenter = rayOrigin + (rayDirection.normalized * (halfCarLength + (brakeRaycastDistance / 2f)));

        boxSize = new Vector3(1.2f, 1f, brakeRaycastDistance);

        Collider[] hitColliders = Physics.OverlapBox(boxCenter, boxSize / 2, Quaternion.identity, trafficLayer);

        int detectedCars = 0;
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.transform.IsChildOf(transform) || hitCollider.transform == transform)
            {
                continue;
            }
            if (hitCollider.CompareTag("Player"))
            {
                playerDetected = true;

                carModelHandler?.BlinkFrontLights();
            }
            detectedCars++;
        }

        if (IsOtherCarInsideCollider(carCollider))
        {
            speed = 0f;
            return;
        }

        speed = detectedCars > 0
            ? Mathf.Lerp(speed, 0, Time.deltaTime * 3f)
            : Mathf.Lerp(speed, maxSpeed, Time.deltaTime * 2f);
        bool isBraking = detectedCars > 0;
        carModelHandler.SetRearBrakeLight(isBraking);
    }

    //The method creates a collider that is slightly forwarded by forwardOffset if the collider inside it automatically stops car.
    private bool IsOtherCarInsideCollider(Collider carCollider)
    {
        Vector3 extendedExtents = carCollider.bounds.extents + new Vector3(0f, 0f, 0.5f);

        Vector3 forward = carCollider.transform.forward;
        Vector3 boxCenter = carCollider.bounds.center + (forward * forwardOffset);

        Collider[] hitColliders =
            Physics.OverlapBox(boxCenter, extendedExtents, carCollider.transform.rotation, trafficLayer);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider != carCollider && !hitCollider.transform.IsChildOf(carCollider.transform))
            {
                return true;
            }
        }

        return false;
    }

    public void StopCarDueToCrash()
    {
        speed = 0f;
        isStoppedDueToCrash = true;

        EffectManager.instance.SpawnAnEffect(Effect.CRASH_AND_FIRE, transform.position);
    }

    private void OnDrawGizmos()
    {
        //DrawCubeCheckingInsideCollider();

        //DrawCubeCheckingCarInFrontOF();
    }

    private void DrawCubeCheckingInsideCollider()
    {
        Vector3 extendedExtents = carCollider.bounds.extents + new Vector3(0f, 0f, 0.5f);

        Vector3 boxCenter = carCollider.bounds.center + (carCollider.transform.forward * forwardOffset);
        Quaternion boxRotation = carCollider.transform.rotation;

        Gizmos.color = Color.green;
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, boxRotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, extendedExtents * 2f);
    }

    private void DrawCubeCheckingCarInFrontOF()
    {
        Gizmos.color = Color.red;
        Vector3 rayOrigin = transform.position + (Vector3.up * raycastOffsetY);
        Collider carCollider = GetComponentInChildren<Collider>();
        Vector3 carSize = carCollider.bounds.size;
        float halfCarLength = carSize.z / 2f;
        Vector3 boxSize = new(1.2f, 1f, brakeRaycastDistance);
        Vector3 boxCenter = rayOrigin + (transform.forward * (halfCarLength + (brakeRaycastDistance / 2f)));
        Gizmos.DrawRay(rayOrigin, transform.forward * (halfCarLength + brakeRaycastDistance));
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
}