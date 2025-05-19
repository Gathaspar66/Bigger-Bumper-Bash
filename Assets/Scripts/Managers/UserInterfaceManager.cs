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
    public GameObject pointsMultiplierDisplayPrefab;
    public GameObject pointsDisplayPrefab;

    GameObject speedMeter;
    GameObject wrongWay;
    GameObject pickupMultiplier;
    GameObject healthDisplay;
    GameObject pointsMultiplierDisplay;
    GameObject pointsDisplay;

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
        pointsMultiplierDisplay = Instantiate(pointsMultiplierDisplayPrefab);
        pointsDisplay = Instantiate(pointsDisplayPrefab);
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

    public void UpdatePointsMultiplayerDisplay(float amount)
    {
        pointsMultiplierDisplay.GetComponent<PointsMultiplierDisplay>().UpdatePointsMultiplayerDisplay(amount);
    }

    public void UpdatePointsDisplay(float amount)
    {
        pointsDisplay.GetComponent<PointsDisplay>().UpdatePointsDisplay(amount);
    }
}