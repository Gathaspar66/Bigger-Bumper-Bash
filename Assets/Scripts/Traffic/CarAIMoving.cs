using UnityEngine;

public class CarAIMoving : MonoBehaviour
{
    public float speed;
    public float brakeRaycastDistance;

    //public LayerMask carLayer;

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
    private readonly float collisionOffsetZ = 2.0f;
    private Vector3 boxSize;
    public LayerMask trafficLayer;


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
        MoveCar();
        DetectOtherCarsAndBrake();
        DestroyCarIfTooFar();
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
        Collider carCollider = GetComponentInChildren<Collider>();

        Vector3 carSize = carCollider.bounds.size;
        float halfCarLength = carSize.z / 2f;


        Vector3 rayOrigin = transform.position + (Vector3.up * raycastOffsetY);
        Vector3 rayDirection = gameObject.transform.forward;

        Vector3 boxCenter = rayOrigin + (rayDirection.normalized * (halfCarLength + (brakeRaycastDistance / 2f)));

        boxSize = new Vector3(1.2f, 1f, brakeRaycastDistance);
        Collider[] hitColliders = Physics.OverlapBox(boxCenter, boxSize / 2, Quaternion.identity, trafficLayer);
        if (IsOtherCarInsideCollider(carCollider))
        {
            speed = 0f;
            return;
        }


        speed = hitColliders.Length > 0 ? Mathf.Lerp(speed, 0, Time.deltaTime * 3f) : Mathf.Lerp(speed, maxSpeed, Time.deltaTime * 2f);
    }
    // check another collider and set the speed to 0 so that the objects are not in another object
    private bool IsOtherCarInsideCollider(Collider carCollider)
    {
        Vector3 offsetPosition = carCollider.bounds.center + (transform.forward * collisionOffsetZ);
        Collider[] immediateHits = Physics.OverlapSphere(offsetPosition, 0.1f, trafficLayer);

        foreach (Collider c in immediateHits)
        {
            if (c != carCollider)
            {

                return true;
            }
        }
        return false;

    }
    private void OnDrawGizmos()
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