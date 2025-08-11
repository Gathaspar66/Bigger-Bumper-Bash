using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;
    public GameObject aboutPanel;
    public GameObject mainMenuIngameMenu;
    public AudioMixer am;

    public GameObject menuButton;
    public GameObject aboutMenu;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ActivateManagers();
        UpdateHighScoreText();
        LoadSoundVolumeSettings();
    }

    private void LoadSoundVolumeSettings()
    {
        am.SetFloat("Music", Mathf.Log10(Mathf.Clamp(PlayerPrefs.GetFloat("musicVolume"), 0.0001f, 1f)) * 20f);
        am.SetFloat("SFX", Mathf.Log10(Mathf.Clamp(PlayerPrefs.GetFloat("soundVolume"), 0.0001f, 1f)) * 20f);
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
        SoundManager.instance.ToggleSound(enableSound);
    }

    public void OnQuitGamePressed()
    {
        print("quit button pressed: ");
        Application.Quit();
    }

    public void OnSettingsButtonPressed()
    {
        mainMenuIngameMenu.SetActive(true);
        aboutMenu.SetActive(false);
        menuButton.SetActive(false);
    }

    public void OnSettingsCloseButtonPressed()
    {
        mainMenuIngameMenu.SetActive(false);
        menuButton.SetActive(true);
    }
}