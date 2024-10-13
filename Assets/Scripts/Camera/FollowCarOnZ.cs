using UnityEngine;

public class FollowCarOnZ : MonoBehaviour
{
    public GameObject car;
    public Vector3 offset;


    void Start()
    {
        SetStartPosition();
    }

    public void SetStartPosition()
    {
        car = PlayerManager.instance.GetPlayerInstance();

        transform.position = car.transform.position + offset;
    }

    void LateUpdate()
    {
        transform.position = new Vector3(0, car.transform.position.y, car.transform.position.z) + offset;
    }
}