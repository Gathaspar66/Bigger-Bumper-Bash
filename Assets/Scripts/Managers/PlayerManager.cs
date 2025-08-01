using UnityEngine;
using static CarConfiguration;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public GameObject playerPrefab;
    public GameObject unikaczPrefab;
    private GameObject playerInstance;
    private GameObject carInstance;
    public Camera cameraPrefab;
    public Camera currentCameraPrefab;
    private CarConfig config;
    private CarConfigDictionary carConfigs;
    public TextAsset carConfigFile;
    public int playerMaxHealth;
    private int playerHealth;

    private void Awake()
    {
        instance = this;
    }

    public void Activate()
    {
        //playerMaxHealth = PlayerPrefs.GetInt("playerLives");
        playerHealth = playerMaxHealth;
        SpawnPlayerPrefab();
        SpawnCar();
        LoadConfig();
        UpdatePlayerDamagedState();
        SpawnCamera();
    }

    private void LoadConfig()
    {
        carConfigs = JsonUtility.FromJson<CarConfigDictionary>(carConfigFile.text);
        config = carConfigs.Unikacz;
        playerInstance.GetComponent<PlayerSteering>().LoadCarSettings(config);
        playerInstance.GetComponent<PlayerPrefabHandler>().SetParameters();
    }

    public void SpawnPlayerPrefab()
    {
        playerInstance = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
    }

    public void SpawnCar()
    {
        carInstance = Instantiate(unikaczPrefab, playerInstance.transform.position, Quaternion.identity);
        carInstance.transform.SetParent(playerInstance.transform);
        playerInstance.GetComponent<PlayerPrefabHandler>().SetCarPrefab(carInstance);
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
    }

    public void OnPlayerDeath()
    {
        playerInstance.GetComponent<PlayerPrefabHandler>().SetPlayerDead(true);
        playerInstance.GetComponent<PlayerSteering>().SetPlayerDead(true);

        Rigidbody rb = playerInstance.GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }
}