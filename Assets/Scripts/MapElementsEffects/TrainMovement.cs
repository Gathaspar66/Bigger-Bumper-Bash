using UnityEngine;

public class TrainMovement : MonoBehaviour
{
    public float distance = 10f;
    public float speed = 5f;
    public float waitTime = 2f;

    private Vector3 startPosition;
    private float waitTimer = 0f;
    private bool isWaiting = false;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        Vector3 targetPosition = startPosition + new Vector3(-distance, 0, 0);

        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                transform.position = startPosition;
                isWaiting = false;
                waitTimer = 0f;
            }
            return;
        }

        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x <= targetPosition.x)
        {
            transform.position = targetPosition;
            isWaiting = true;
        }
    }
}