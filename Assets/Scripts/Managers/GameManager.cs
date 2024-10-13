using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private GameObject playerInstance;
    public GameObject roadManager;
    public GameObject userInterfaceManager;
    public GameObject playerManager;
    public GameObject trafficMaganer;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        spawnBasicElementsOfGame();


        activeManagers();
    }

    public void spawnBasicElementsOfGame()
    {
    }

    public void activeManagers()
    {
        playerManager.SetActive(true);
        roadManager.SetActive(true);
        userInterfaceManager.SetActive(true);
        trafficMaganer.SetActive(true);
    }

    public GameObject GetPlayer()
    {
        return playerInstance;
    }
}