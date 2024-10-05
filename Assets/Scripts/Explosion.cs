using UnityEngine;

public class Explosion : MonoBehaviour
{

    public GameObject originalObject;
    public GameObject model;
    Rigidbody[] rigidbodies;
    
   private void Awake()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
    }

    void Start()
    {
       // Explode(Vector3.forward);
       
    }

    public void Explode(Vector3 externalForce)
    {
      
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

     
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.transform.parent = null;
            rb.GetComponent<MeshCollider>().enabled = true;
            
            rb.gameObject.SetActive(true);
            rb.isKinematic = false;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.AddForce(Vector3.up*200+externalForce,ForceMode.Force);

           
            rb.AddTorque(Random.insideUnitSphere * 0.5f, ForceMode.Impulse);
        }
    }
}