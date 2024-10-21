using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterfaceManager : MonoBehaviour
{
    public GameObject controlsPrefab;
    public GameObject speedMeterPrefab;
    public GameObject wrongWayPrefab;

    void Start()
    {
        Instantiate(controlsPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        Instantiate(speedMeterPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        Instantiate(wrongWayPrefab, new Vector3(0, 0, 0), Quaternion.identity);
    }

}