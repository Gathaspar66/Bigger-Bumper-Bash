using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterfaceManager : MonoBehaviour
{
    public static UserInterfaceManager instance;

    public GameObject controlsPrefab;
    public GameObject speedMeterPrefab;
    public GameObject wrongWayPrefab;
    public GameObject pickupMultiplierPrefab;

    GameObject speedMeter;
    GameObject wrongWay;
    GameObject pickupMultiplier;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Instantiate(controlsPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        speedMeter = Instantiate(speedMeterPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        wrongWay = Instantiate(wrongWayPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        pickupMultiplier = Instantiate(pickupMultiplierPrefab, new Vector3(0, 0, 0), Quaternion.identity);
    }

    public void UpdatePickupMultiplier(int value)
    {
        pickupMultiplier.GetComponent<PickupMultiplier>().UpdatePickupMultiplier(value);
    }
}