using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCar : MonoBehaviour
{
    Vector3 camPosition = Vector3.zero;
    public Vector3 offset;

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
        transform.position = camPosition;
    }
}
