using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCarBounds : MonoBehaviour
{
    Vector3 camPosition = Vector3.zero;
    public Vector3 offset;
    public float followOffset = 1.5f;

    GameObject car;

    // Start is called before the first frame update
    void Start()
    {
        car = ControlsCameraChoice.instance.GetCar();
    }

    // Update is called once per frame
    void Update()
    {
        camPosition = car.transform.position + offset;
        camPosition.x = transform.position.x;

        if (car.transform.position.x > transform.position.x + followOffset)
        {
            camPosition.x = car.transform.position.x - followOffset;
        }
        if (car.transform.position.x < transform.position.x - followOffset)
        {
            camPosition.x = car.transform.position.x + followOffset;
        }

        transform.position = camPosition;
    }
}
