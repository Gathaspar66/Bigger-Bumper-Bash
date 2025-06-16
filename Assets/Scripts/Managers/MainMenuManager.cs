using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    public GameObject aboutPanel;
    public TMP_InputField livesInputField;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ActivateManagers();
        UpdateHighScoreText();
        SetupLivesInputField();
    }

    private void SetupLivesInputField()
    {
        livesInputField.text = PlayerPrefs.GetInt("playerLives").ToString();
        if (livesInputField.text == "0")
        {
            PlayerPrefs.SetInt("playerLives", 3);
            livesInputField.text = PlayerPrefs.GetInt("playerLives").ToString();
        }
    }

    public void ActivateManagers()
    {
        SoundManager.instance.Activate();
    }

    public DigitRowHandler digitRowHandler;

    public void OnGameStartButtonPressed()
    {
        PlayerPrefs.SetInt("playerLives", int.Parse(livesInputField.text));
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