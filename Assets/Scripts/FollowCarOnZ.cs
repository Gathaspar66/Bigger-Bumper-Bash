using UnityEngine;

public class FollowCarOnZ : MonoBehaviour
{
    GameObject car;

    public Transform carTransform;
    public Vector3 offset;
    public float cameraOffsetZ = -10f;  

    private Vector3 initialCameraPosition; 

    void Start()
    {
        car = ControlsCameraChoice.instance.GetCar();
        transform.position = car.transform.position + new Vector3(0, 0, cameraOffsetZ) + offset;
        initialCameraPosition = transform.position;
    }

    void LateUpdate()
    {
        
        transform.position = new Vector3(initialCameraPosition.x, initialCameraPosition.y, car.transform.position.z + cameraOffsetZ) + offset;
    }
}