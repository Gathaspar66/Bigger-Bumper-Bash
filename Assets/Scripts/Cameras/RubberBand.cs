using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubberBand : MonoBehaviour
{
    Vector3 camTargetPosition = Vector3.zero;
    public Vector3 offset;
    public float followOffset = 1.5f;

    GameObject car;
    public float followSpeed = 1f;
    public float lerp = 0.1f;
    public float lerpSpeed = 10f;
    int direction = 1;
    float speedScale = 1;
    float distance;
    float step;

    // Start is called before the first frame update
    void Start()
    {
        car = ControlsCameraChoice.instance.GetCar();
        transform.position = car.transform.position + offset;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    private void LateUpdate()
    {
        LerpFollow();
    }

    void LerpFollow()
    {
        //write y and z
        camTargetPosition = car.transform.position + offset;
        //follow only on x
        camTargetPosition.x = Vector3.Lerp(transform.position, car.transform.position + offset, lerp * lerpSpeed * Time.deltaTime).x;
        transform.position = camTargetPosition;
    }

    void SpeedScaleFollow()
    {

        camTargetPosition = car.transform.position + offset;

        if (car.transform.position.x >= transform.position.x)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }

        distance = Mathf.Abs(transform.position.x - camTargetPosition.x);
        //speedScale = Mathf.Clamp(distance, 0f, 1f);
        step = followSpeed * speedScale * Time.deltaTime;


        if (Mathf.Abs(transform.position.x - car.transform.position.x) > 0.1f)
        {
            camTargetPosition.x = transform.position.x + direction * step;
        }

        transform.position = camTargetPosition;
    }
}
