using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject playerInstance;
    public GameObject roadManager;
    public GameObject userInterfaceManager;
    public GameObject playerManager;
    public GameObject trafficManager;
    public GameObject pointsManager;
    public GameObject soundManager;
    private readonly bool isGameOver = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Application.targetFrameRate = 120;
        //QualitySettings.vSyncCount = 0;
        ActivateManagers();
    }

    public void ActivateManagers()
    {
        CarDatabaseManager.instance.Activate();
        PlayerManager.instance.Activate();
        RoadManager.instance.Activate();
        UserInterfaceManager.instance.Activate();
        TrafficManager.instance.Activate();

        PointsManager.instance.Activate();
        SoundManager.instance.Activate();

    }

    public void PauseGame(bool ifPause)
    {
        if (isGameOver)
        {
            return;
        }

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

    public void OnPlayerDeath()
    {
        RoadManager.instance.OnPlayerDeath();
        UserInterfaceManager.instance.OnPlayerDeath();
        TrafficManager.instance.OnPlayerDeath();
        PlayerManager.instance.OnPlayerDeath();
        //PointsManager.instance.OnPlayerDeath();
        SoundManager.instance.OnPlayerDeath();
    }
}