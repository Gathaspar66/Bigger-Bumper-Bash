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
    private GameObject pointsMultiplierDisplay;
    private GameObject pointsDisplay;

    private GameObject speedMeter;
    private GameObject wrongWay;
    private GameObject pickupMultiplier;
    private GameObject healthDisplay;
    public int maxMultplier = 18;

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

        UserInterfaceManager.instance.UpdateHealthDisplay(PlayerManager.instance.GetPlayerHealth(),
            PlayerManager.instance.GetPlayerMaxHealth());
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
        Time.timeScale = 0;
        GameObject tmp = Instantiate(endgameMenuPrefab);
        tmp.GetComponent<EndgameMenu>().UpdateScore();
        PointsManager.instance.SavePlayerScore();
    }

    public void UpdatePointsMultiplierDisplay(int amount)
    {
        pointsMultiplierDisplay.GetComponent<PointsMultiplierDisplay>().UpdatePointsMultiplierDisplay(amount);
    }

    public void UpdatePointsDisplay(int amount)
    {
        pointsDisplay.GetComponent<PointsDisplay>().SetDigits(amount);
    }
}