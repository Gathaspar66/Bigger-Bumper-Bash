using UnityEngine;

public class UserInterfaceManager : MonoBehaviour
{
    public static UserInterfaceManager instance;

    public GameObject controlsPrefab;
    public GameObject speedMeterPrefab;
    public GameObject wrongWayPrefab;
    public GameObject pickupMultiplierPrefab;
    public GameObject healthDisplayPrefab;
    public GameObject inGameMenuPrefab;
    public GameObject endGameMenuPrefab;

    public GameObject pointsMultiplierDisplayPrefab;
    public GameObject pointsDisplayPrefab;
    private GameObject pointsMultiplierDisplay;
    private GameObject pointsDisplay;

    private GameObject speedMeter;
    private GameObject wrongWay;
    private GameObject pickupMultiplier;
    private GameObject healthDisplay;
    private GameObject inGameMenu;
    private GameObject controls;

    private void Awake()
    {
        instance = this;
    }

    public void Activate()
    {
        controls = Instantiate(controlsPrefab);
        inGameMenu = Instantiate(inGameMenuPrefab);

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
        //Time.timeScale = 0;
        DisableUI();
        GameObject tmp = Instantiate(endGameMenuPrefab);
        tmp.GetComponent<EndgameMenu>().UpdateScore();
        PointsManager.instance.SavePlayerScore();
    }

    public void DisableUI()
    {
        controls.SetActive(false);
        speedMeter.SetActive(false);
        wrongWay.SetActive(false);
        pickupMultiplier.SetActive(false);
        healthDisplay.SetActive(false);
        inGameMenu.SetActive(false);

        pointsDisplay.SetActive(false);

        healthDisplay.SetActive(false);
        pointsMultiplierDisplay.SetActive(false);
    }

    public void OnIngameMenuToggle(bool ifOpen)
    {

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