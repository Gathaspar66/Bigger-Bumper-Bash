using UnityEngine;

public class FollowCarOnZ : MonoBehaviour
{
    GameObject car;

    public Transform carTransform;
    public Vector3 offset;
    

    

    void Start()
    {
        car = ControlsCameraChoice.instance.GetCar();
        transform.position = car.transform.position  + offset;
        // initialCameraPosition = transform.position;
        //initialCameraPosition = car.transform.position;
    }

    void LateUpdate()
    {
        
        transform.position = new Vector3(0, car.transform.position.y, car.transform.position.z) + offset;
    }
}