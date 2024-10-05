using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsHandler : MonoBehaviour
{
    public Transform playerCarTransform;
    public PointsManager pointsManager;
    private Explosion explosionScript;
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

    private void OnTriggerEnter(Collider other)
    {
        explosionScript = GetComponent<Explosion>();
        PointsManager.instance.AddPoints(1000);
        explosionScript.Explode(Vector3.forward);
        Destroy(gameObject);
    }
}