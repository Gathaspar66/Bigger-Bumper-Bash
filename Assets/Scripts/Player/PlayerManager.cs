using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using static carConfig;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public GameObject playerPrefab;
    public GameObject unikaczPrefab;
    private GameObject playerInstance;
    private GameObject carInstance;
    public Camera camera;
    private CarConfig config;
    private CarConfigDictionary carConfigs;
    public TextAsset carConfigFile;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SpawnPlayerPrefab();
        SpawnCar();
        LoadConfig();
        SpawnCamera();
    }
    void LoadConfig()
    {

        carConfigs = JsonUtility.FromJson<CarConfigDictionary>(carConfigFile.text);
        config = carConfigs.Unikacz;
        playerInstance.GetComponent<PlayerSteering>().LoadCarSettings(config);
    }
    public void SpawnPlayerPrefab()
    {
        playerInstance = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
    }

    public void SpawnCar()
    {
        carInstance = Instantiate(unikaczPrefab, playerInstance.transform.position, Quaternion.identity);
        carInstance.transform.SetParent(playerInstance.transform);
    }

    public void SpawnCamera()
    {
        Instantiate(camera, new Vector3(0, 0, 0), camera.transform.rotation);
    }

    public GameObject GetPlayerInstance()
    {
        return playerInstance;
    }
}