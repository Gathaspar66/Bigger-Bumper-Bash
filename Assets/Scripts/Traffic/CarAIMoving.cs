using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAIMoving : MonoBehaviour
{
    public float speed;
    public float brakeRaycastDistance;


    public LayerMask carLayer;

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

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        brakeRaycastDistance = Random.Range(minBrakeRaycastDistance, maxBrakeRaycastDistance);
        SetInitialSpeed();
    }

    void FixedUpdate()
    {
        MoveCar();
        DetectOtherCarsAndBrake();
        DestroyCarIfTooFar();
    }

    void SetInitialSpeed()
    {
        maxSpeed = PlayerSteering.instance.maxForwardVelocity;
        minSpeed = PlayerSteering.instance.minForwardVelocity;
        spawnMinSpeed = minSpeed * 0.9f;
        spawnMaxSpeed = maxSpeed * 1.1f;

        speed = GenerateSpeed();

        maxSpeed = speed;
    }

    float GenerateSpeed()
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

        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);


        Quaternion spawnRotation;

        if (currentLaneIndex == 0 || currentLaneIndex == 1)
        {
            spawnRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            spawnRotation = transform.rotation;
        }


        GameObject spawnedCar = Instantiate(carPrefabs[randomIndex], spawnPosition, spawnRotation);
        spawnedCar.transform.SetParent(transform);
    }


    void DestroyCarIfTooFar()
    {
        GameObject player = PlayerManager.instance.GetPlayerInstance();
        if (transform.position.z < player.transform.position.z - 100f)
        {
            Destroy(gameObject);
        }
    }

    void MoveCar()
    {
        if (currentLaneIndex == 0 || currentLaneIndex == 1)
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    void DetectOtherCarsAndBrake()
    {
        RaycastHit hit;


        Vector3 rayOrigin = transform.position + Vector3.up * raycastOffsetY;


        Vector3 rayDirection =
            (currentLaneIndex == 0 || currentLaneIndex == 1) ? Vector3.back : Vector3.forward;


        if (Physics.Raycast(rayOrigin, rayDirection, out hit, brakeRaycastDistance, carLayer))
        {
            speed = Mathf.Lerp(speed, 0, Time.deltaTime * 3f);
        }
        else
        {
            speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime * 2f);
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 rayOrigin = transform.position + Vector3.up * raycastOffsetY;


        Vector3 rayDirection =
            (currentLaneIndex == 0 || currentLaneIndex == 1) ? Vector3.back : Vector3.forward;


        Gizmos.DrawRay(rayOrigin, rayDirection * brakeRaycastDistance);
    }
}