using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsHandler : MonoBehaviour
{
    public Transform playerCarTransform;
    public PointsManager pointsManager;

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
        PointsManager.instance.AddPoints(1000);

        Destroy(gameObject);
    }
}