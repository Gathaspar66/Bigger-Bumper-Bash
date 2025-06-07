using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float offset = 0.2f;
    public float speed = 2f;
    Vector3 startPosition;
    float randomOffset;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        randomOffset = Random.Range(0f, 2f * Mathf.PI);
    }

    // Update is called once per frame
    void Update()
    {
        MoveUpAndDown();
        Rotate();
    }

    void MoveUpAndDown()
    {
        Vector3 newPos = startPosition;
        newPos.y += Mathf.Sin(Time.time * speed) * offset;
        transform.position = newPos;
    }

    void Rotate()
    {
        transform.Rotate(Vector3.up);
    }
}
