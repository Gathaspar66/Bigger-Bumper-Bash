using UnityEngine;

public class CarChoiceContainerRotation : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(0, 50 * Time.deltaTime, 0);
    }
}
