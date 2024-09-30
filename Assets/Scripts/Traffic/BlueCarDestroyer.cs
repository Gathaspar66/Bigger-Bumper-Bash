using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueCarDestroyer : MonoBehaviour
{
    public Transform playerCarTransform;  

    void Update()
    {
        DestroyCarIfTooFar();
    }

    void DestroyCarIfTooFar()
    {
        
        if (playerCarTransform != null && transform.position.z < playerCarTransform.position.z - 100f)
        {
            Destroy(gameObject);  
        }
    }
}
