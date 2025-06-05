using System.Collections.Generic;
using UnityEngine;

public class CarAIMoving : MonoBehaviour
{
    public float speed;
    public float brakeRaycastDistance;

    public float raycastOffsetY = 0f;
    private int currentLaneIndex;
    private float maxSpeed;
    private float minSpeed;
    private float spawnMinSpeed;
    private float spawnMaxSpeed;
    public GameObject[] carPrefabs;
    public static CarAIMoving instance;
    public float minBrakeRaycastDistance;
    public float maxBrakeRaycastDistance;
    //private readonly float collisionOffsetZ = 2.0f;
    private Vector3 boxSize;
    public LayerMask trafficLayer;
    public Collider carCollider;
    public List<Material> carPaintMaterials;
    public Vector2 smoothnessRange = new(0.5f, 0.8f);
    public Vector2 metallicRange = new(0.6f, 1f);
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        brakeRaycastDistance = Random.Range(minBrakeRaycastDistance, maxBrakeRaycastDistance);
        SetInitialSpeed();
    }

    private void FixedUpdate()
    {
        DetectOtherCarsAndBrake();

        DestroyCarIfTooFar();
    }

    private void Update()
    {
        MoveCar();
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

    public void SpawnRandomCar(int randomLaneIndex)
    {
        int randomIndex = Random.Range(0, carPrefabs.Length);
        currentLaneIndex = randomLaneIndex;

        Vector3 spawnPosition = new(transform.position.x, transform.position.y, transform.position.z);


        GameObject spawnedCar = Instantiate(carPrefabs[randomIndex], spawnPosition, transform.rotation);
        spawnedCar.transform.SetParent(transform);
        ChangeCarBodyColor(spawnedCar);
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
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void DetectOtherCarsAndBrake()
    {
        carCollider = GetComponentInChildren<Collider>();

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
    }

    private bool IsOtherCarInsideCollider(Collider carCollider)
    {
        Vector3 extendedExtents = carCollider.bounds.extents + new Vector3(0f, 0f, 1f);
        Collider[] hitColliders =
            Physics.OverlapBox(carCollider.bounds.center, extendedExtents, Quaternion.identity, trafficLayer);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider != carCollider && !hitCollider.transform.IsChildOf(carCollider.transform))
            {
                return true;
            }
        }

        return false;
    }

    private void ChangeCarBodyColor(GameObject car)
    {
        Transform bodyTransform = car.transform.Find("body");
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

    private void OnDrawGizmos()
    {
        Vector3 center = carCollider.bounds.center;
        Vector3 halfExtents = carCollider.bounds.extents + new Vector3(0f, 0f, 1f);
        _ = Quaternion.identity;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, halfExtents * 2f);

        //Gizmos.color = Color.red;
        // Vector3 rayOrigin = transform.position + (Vector3.up * raycastOffsetY);
        //  Collider carCollider = GetComponentInChildren<Collider>();
        // Vector3 carSize = carCollider.bounds.size;
        //  float halfCarLength = carSize.z / 2f;
        // Vector3 boxSize = new(1.2f, 1f, brakeRaycastDistance);
        // Vector3 boxCenter = rayOrigin + (transform.forward * (halfCarLength + (brakeRaycastDistance / 2f)));
        // Gizmos.DrawRay(rayOrigin, transform.forward * (halfCarLength + brakeRaycastDistance));
        // Gizmos.DrawWireCube(boxCenter, boxSize);
    }
}