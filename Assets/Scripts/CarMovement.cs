using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public float speed = 10f; // jakies gówno

    void Update()
    {
        
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

       
        if (transform.position.z < -100f)
        {
            Destroy(gameObject);
        }
    }
}