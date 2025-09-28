using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public GameObject playerPrefab;

    private GameObject playerInstance;

    private GameObject carInstance;
    public Camera cameraPrefab;
    public Camera currentCameraPrefab;

    public int playerMaxHealth;

    private int playerHealth;
    public CarData selectedCarData;

    bool isPlayerImmune = false;

    private void Awake()
    {
        instance = this;
    }

    public void Activate()
    {
        playerHealth = playerMaxHealth;
        SpawnPlayerPrefab();
        SpawnCar();
        LoadConfig();
        UpdatePlayerDamagedState();
        SpawnCamera();
    }

    private void LoadConfig()
    {
        playerInstance.GetComponent<PlayerSteering>().LoadCarSettings(selectedCarData);
        playerInstance.GetComponent<PlayerPrefabHandler>().SetParameters();
    }

    public void SpawnPlayerPrefab()
    {
        playerInstance = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
    }

    public void SpawnCar()
    {
        CarType selectedCar = (CarType)PlayerPrefs.GetInt("SelectedCar", (int)CarType.UNIKACZ);
        selectedCarData = null;

        foreach (CarData car in CarDatabaseManager.instance.carObjects)
        {
            if (car.carType == selectedCar)
            {
                selectedCarData = car;
                break;
            }
        }

        carInstance = Instantiate(selectedCarData.carPrefab, playerInstance.transform.position, Quaternion.identity);
        carInstance.transform.SetParent(playerInstance.transform);

        playerInstance.GetComponent<PlayerPrefabHandler>().SetCarPrefab(carInstance);

        playerMaxHealth = selectedCarData.hp;
        playerHealth = playerMaxHealth;

        carInstance.GetComponent<CarModelHandler>()
            .SetCarMaterial(CarMaterialManager.instance.materials[PlayerPrefs.GetInt("CarColorChoice")].matObject);
    }

    public void SpawnCamera()
    {
        currentCameraPrefab = Instantiate(cameraPrefab, new Vector3(0, 0, 0), cameraPrefab.transform.rotation);
    }

    public Camera GetCameraInstance()
    {
        return currentCameraPrefab;
    }

    public GameObject GetPlayerInstance()
    {
        return playerInstance;
    }

    public GameObject GetCarInstance()
    {
        return carInstance;
    }

    public void GetDamaged()
    {
        playerHealth -= 1;
        UpdatePlayerDamagedState();
        UserInterfaceManager.instance.UpdateHealthDisplay(playerHealth, playerMaxHealth);
        if (playerHealth <= 0)
        {
            GameManager.instance.OnPlayerDeath();
            return;
        }
        StartImmunity();
    }

    private void UpdatePlayerDamagedState()
    {
        playerInstance.GetComponent<PlayerPrefabHandler>().UpdatePlayerDamagedState(playerHealth);
    }

    public int GetPlayerHealth()
    {
        return playerHealth;
    }

    public int GetPlayerMaxHealth()
    {
        return playerMaxHealth;
    }

    private void StartImmunity()
    {
        playerInstance.GetComponent<PlayerPrefabHandler>().StartImmunity();
        SetPlayerImmune(true);
    }

    public void OnPlayerDeath()
    {
        playerInstance.GetComponent<PlayerPrefabHandler>().SetPlayerDead(true);
        playerInstance.GetComponent<PlayerSteering>().SetPlayerDead(true);

        Rigidbody rb = playerInstance.GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    public void SetPlayerImmune(bool value)
    {
        isPlayerImmune = value;
    }

    public bool GetIsPlayerImmune()
    {
        return isPlayerImmune;
    }
}