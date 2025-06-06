using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float speed = 1;
    float rotationspeed = 0.5f;
    Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveUpAndDown();
        Rotate();
    }

    void MoveUpAndDown()
    {
        transform.position = new Vector3(transform.position.x, Mathf.PingPong(Time.time, 0.5f), transform.position.z);
    }

    void Rotate()
    {
        transform.Rotate(Vector3.up * rotationspeed);
    }
}
