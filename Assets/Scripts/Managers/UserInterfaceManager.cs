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
    public GameObject healthDisplayPrefab;
    public GameObject ingameMenuPrefab;
    public GameObject endgameMenuPrefab;

    GameObject speedMeter;
    GameObject wrongWay;
    GameObject pickupMultiplier;
    GameObject healthDisplay;

    private void Awake()
    {
        instance = this;
    }

    public void Activate()
    {
        Instantiate(controlsPrefab);
        Instantiate(ingameMenuPrefab);

        speedMeter = Instantiate(speedMeterPrefab);
        wrongWay = Instantiate(wrongWayPrefab);
        pickupMultiplier = Instantiate(pickupMultiplierPrefab);
        healthDisplay = Instantiate(healthDisplayPrefab);
    }

    public void UpdatePickupMultiplier(int value)
    {
        pickupMultiplier.GetComponent<PickupMultiplier>().UpdatePickupMultiplier(value);
    }

    public void UpdateHealthDisplay(int amount)
    {
        healthDisplay.GetComponent<HealthDisplay>().UpdateHealthVisuals(amount);
    }

    public void OnPlayerDeath()
    {
        Time.timeScale = 0;
        Instantiate(endgameMenuPrefab);
    }
}