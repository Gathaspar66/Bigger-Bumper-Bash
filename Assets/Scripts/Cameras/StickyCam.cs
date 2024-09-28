using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyCam : MonoBehaviour
{
    public Vector3 offset = new Vector3(0.2f, 4.5f, -2.5f);

    // Start is called before the first frame update
    void Start()
    {
        transform.position = ControlsCameraChoice.instance.GetCar().transform.position + offset;
        transform.parent = ControlsCameraChoice.instance.GetCar().transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
