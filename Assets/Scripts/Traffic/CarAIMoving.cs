using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAIMoving : MonoBehaviour
{
    public float speed = 12f;
    public float brakeDistance = 10f;
    public float laneChangeSpeed = 3f;
    public float laneChangeInterval = 2f;
    public Transform[] lanes;
    public LayerMask carLayer;

    private int currentLaneIndex;
    private float maxSpeed;
    public bool isChangingLane = false;
    private Transform playerCarTransform;

    void Start()
    {
        currentLaneIndex = Random.Range(0, lanes.Length);
        transform.position = new Vector3(lanes[currentLaneIndex].position.x, transform.position.y, transform.position.z);
        maxSpeed = speed;
        StartCoroutine(ChangeLane());
    }

    public void SetPlayerCarTransform(Transform playerTransform)
    {
        playerCarTransform = playerTransform;
    }

    void Update()
    {
        MoveCar();
        DetectOtherCarsAndBrake();
        DestroyCarIfTooFar();
    }

    void DestroyCarIfTooFar()
    {
        if (playerCarTransform != null && transform.position.z < playerCarTransform.position.z - 100f)
        {
            Destroy(gameObject);
        }
    }

    void MoveCar()
    {
        // Vector3 newPosition = transform.position + Vector3.forward * speed * Time.deltaTime;
        //transform.position = newPosition;
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void DetectOtherCarsAndBrake()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, brakeDistance, carLayer))
        {
            speed = Mathf.Lerp(speed, 0, Time.deltaTime * 3f);
        }
        else
        {
            speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime * 2f);
        }
    }

    IEnumerator ChangeLane()
    {
        while (true)
        {
            yield return new WaitForSeconds(laneChangeInterval);
            if (!isChangingLane)
            {
                int newLaneIndex = Random.Range(0, lanes.Length);
                if (newLaneIndex != currentLaneIndex)
                {
                    isChangingLane = true;
                    StartCoroutine(MoveToNewLane(newLaneIndex));
                }
            }
        }
    }

    IEnumerator MoveToNewLane(int newLaneIndex)
    {
        Vector3 targetPosition = new Vector3(lanes[newLaneIndex].position.x, transform.position.y, transform.position.z);

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            // Ruch tylko w osi X
            transform.position = new Vector3(
                Mathf.Lerp(transform.position.x, targetPosition.x, Time.deltaTime * laneChangeSpeed),
                transform.position.y,
                transform.position.z
            );
            yield return null;
        }

        currentLaneIndex = newLaneIndex;
        isChangingLane = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * brakeDistance);
    }
}
