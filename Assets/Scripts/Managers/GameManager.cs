using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private GameObject playerInstance;
    public GameObject roadManager;
    public GameObject userInterfaceManager;
    public GameObject playerManager;
    public GameObject trafficManager;
    public GameObject pointsManager;

    bool isGameOver = false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SpawnBasicElementsOfGame();


        ActivateManagers();
    }

    public void SpawnBasicElementsOfGame()
    {
    }

    public void ActivateManagers()
    {
        PlayerManager.instance.Activate();
        RoadManager.instance.Activate();
        UserInterfaceManager.instance.Activate();
        TrafficManager.instance.Activate();
        PointsManager.instance.Activate();
    }

    public GameObject GetPlayer()
    {
        return playerInstance;
    }

    public void PauseGame(bool ifPause)
    {
        if (isGameOver) return;
        Time.timeScale = (ifPause == true) ? 0 : 1;
    }

    public void RestartLevel()

    {
        PauseGame(false);
        SceneManager.LoadScene("Level1");
    }

    public void QuitToMenu()
    {
        PauseGame(false);
        SceneManager.LoadScene("MainMenu");
    }
}