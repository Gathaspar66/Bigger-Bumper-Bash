using UnityEngine;

public class FollowCarOnZ : MonoBehaviour
{
    public GameObject car;
    public Vector3 offset;

    private void Start()
    {
        SetStartPosition();
    }

    public void SetStartPosition()
    {
        car = PlayerManager.instance.GetPlayerInstance();
        transform.position = car.transform.position + offset;
    }

    private void LateUpdate()
    {
        Vector3 basePosition = new Vector3(0, car.transform.position.y, car.transform.position.z) + offset;


        if (CameraShake.Instance != null)
        {
            basePosition += CameraShake.Instance.GetShakeOffset();
        }

        transform.position = basePosition;
    }
}