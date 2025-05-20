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
  
    GameObject pointsMultiplierDisplay;
    GameObject pointsDisplay;

    private GameObject speedMeter;
    private GameObject wrongWay;
    private GameObject pickupMultiplier;
    private GameObject healthDisplay;


    private void Awake()
    {
        instance = this;
    }

    public void Activate()
    {
        _ = Instantiate(controlsPrefab);
        _ = Instantiate(ingameMenuPrefab);

        speedMeter = Instantiate(speedMeterPrefab);
        wrongWay = Instantiate(wrongWayPrefab);
        pickupMultiplier = Instantiate(pickupMultiplierPrefab);
        healthDisplay = Instantiate(healthDisplayPrefab);

        pointsMultiplierDisplay = Instantiate(pointsMultiplierDisplayPrefab);
        pointsDisplay = Instantiate(pointsDisplayPrefab);

        UserInterfaceManager.instance.UpdateHealthDisplay(PlayerManager.instance.GetPlayerHealth(), PlayerManager.instance.GetPlayerMaxHealth());

    }

    public void UpdatePickupMultiplier(int value)
    {
        pickupMultiplier.GetComponent<PickupMultiplier>().UpdatePickupMultiplier(value);
    }

    public void UpdateHealthDisplay(int amount, int maxAmount)
    {
        healthDisplay.GetComponent<HealthDisplay>().UpdateHealthVisuals(amount, maxAmount);
    }

    public void OnPlayerDeath()
    {
        PointsManager.instance.SavePlayerScore();
        Time.timeScale = 0;
        _ = Instantiate(endgameMenuPrefab);
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