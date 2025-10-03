using UnityEngine;

public class BarrelIdleMovement : MonoBehaviour
{
    public float offset = 0.2f;
    public float speed = 2f;
    public float rotationSpeed = 20f;

    private Vector3 startPosition;
    private float randomOffset;

    private void Start()
    {
        startPosition = transform.position;
        randomOffset = Random.Range(0f, 2f * Mathf.PI);
    }

    private void Update()
    {
        MoveUpAndDown();
        Rotate();
    }

    private void MoveUpAndDown()
    {
        Vector3 newPos = startPosition;
        newPos.y += Mathf.Sin((Time.time * speed) + randomOffset) * offset;
        transform.position = newPos;
    }

    private void Rotate()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }
}