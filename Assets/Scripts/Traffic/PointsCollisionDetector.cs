using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsCollisionDetector: MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
       
        //explosionScript = GetComponent<Explosion>();
        PointsManager.instance.AddPoints(1000);
        //explosionScript.Explode(Vector3.forward);
        Destroy(gameObject);
    }
}
