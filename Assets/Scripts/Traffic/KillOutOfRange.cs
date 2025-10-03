using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOutOfRange : MonoBehaviour
{
    Vector3 startLocation;

    // Start is called before the first frame update
    void Start()
    {
        startLocation = transform.position;    
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(PlayerManager.instance.GetPlayerInstance().transform.position, transform.position) > 100){
            Destroy(gameObject);
        }
    }
}
