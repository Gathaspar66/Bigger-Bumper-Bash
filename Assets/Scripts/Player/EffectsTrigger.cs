using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Wykryto kolizjê z: " + other.gameObject.name, other.gameObject);

        if (other.gameObject.layer == 6)
        {
            EffectManager.instance.SpawnAnEffect(gameObject.transform.position, true);
        }

        if (other.gameObject.layer == 3)
        {
            EffectManager.instance.SpawnAnEffect(gameObject.transform.position, false);
        }


        //EditorApplication.isPaused = true;
        // Time.timeScale = 0;
    }
}