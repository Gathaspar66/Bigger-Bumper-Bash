using UnityEngine;

public class FollowCarOnZ : MonoBehaviour
{
    public Transform carTransform; 
    public float cameraOffsetZ = -10f;  

    private Vector3 initialCameraPosition; 

    void Start()
    {
      
        initialCameraPosition = transform.position;
    }

    void LateUpdate()
    {
        
        transform.position = new Vector3(initialCameraPosition.x, initialCameraPosition.y, carTransform.position.z + cameraOffsetZ);
    }
}