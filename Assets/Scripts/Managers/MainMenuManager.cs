using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    public GameObject aboutPanel;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ActivateManagers();
        UpdateHighScoreText();
    }


    public void ActivateManagers()
    {
        SoundManager.instance.Activate();
    }
    public DigitRowHandler digitRowHandler;

    public void OnGameStartButtonPressed()
    {
        SceneManager.LoadScene("Level1");
    }

    private void UpdateHighScoreText()
    {
        if (PlayerPrefs.GetInt("highScore") == 0)
        {
            return;
        }

        int points = PlayerPrefs.GetInt("highScore");
        digitRowHandler.UpdatePointsDisplay(points);
    }

    public void ToggleAboutPanel(bool showPanel)
    {
        aboutPanel.SetActive(showPanel);
    }

    public void ToggleSound(bool enableSound)
    {
        print("toggle sound button pressed: " + enableSound);
    }

    public void OnQuitGamePressed()
    {
        print("quit button pressed: ");
        Application.Quit();
    }
}