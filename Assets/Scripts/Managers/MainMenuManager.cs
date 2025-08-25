using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;
    public GameObject aboutPanel;
    public GameObject mainMenuIngameMenu;
    public AudioMixer am;

    public GameObject menuButton;
    public GameObject aboutMenu;
    [Header("Car Preview")]
    public Transform carChoiceContainer;  // miejsce, gdzie spawnuj¹ siê auta
    private int currentCarIndex = 0;
    private GameObject spawnedCar;

    [Header("Car Info UI")]
    public TMP_Text carNameText;
    public TMP_Text carHpText;
    private void Awake()
    {
        instance = this;
    }
    private List<CarData> CarObjects => CarDatabaseManager.instance != null ? CarDatabaseManager.instance.carObjects : new List<CarData>();

    private void Start()
    {
        ActivateManagers();
        UpdateHighScoreText();
        LoadSoundVolumeSettings();
        if (CarObjects.Count > 0)
        {
            currentCarIndex = 0;
            ShowCar(currentCarIndex);
        }
    }

    private void LoadSoundVolumeSettings()
    {
        _ = am.SetFloat("Music", Mathf.Log10(Mathf.Clamp(PlayerPrefs.GetFloat("musicVolume"), 0.0001f, 1f)) * 20f);
        _ = am.SetFloat("SFX", Mathf.Log10(Mathf.Clamp(PlayerPrefs.GetFloat("soundVolume"), 0.0001f, 1f)) * 20f);
    }

    public void ActivateManagers()
    {
        CarDatabaseManager.instance.Activate();
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

    public void OnNextCar()
    {
        if (CarObjects.Count == 0)
        {
            return;
        }

        currentCarIndex = (currentCarIndex + 1) % CarObjects.Count;
        ShowCar(currentCarIndex);
    }

    public void OnPreviousCar()
    {
        if (CarObjects.Count == 0)
        {
            return;
        }

        currentCarIndex--;
        if (currentCarIndex < 0)
        {
            currentCarIndex = CarObjects.Count - 1;
        }

        ShowCar(currentCarIndex);
    }

    private void ShowCar(int index)
    {
        if (spawnedCar != null)
        {
            Destroy(spawnedCar);
        }

        List<CarData> cars = CarObjects;
        if (cars.Count == 0 || carChoiceContainer == null)
        {
            return;
        }

        CarData car = cars[index];


        if (car.prefabAuta != null)
        {
            spawnedCar = Instantiate(
                car.prefabAuta,
                carChoiceContainer.position,
                carChoiceContainer.rotation,
                carChoiceContainer
            );
            spawnedCar.name = car.nazwa;
        }


        if (carNameText != null)
        {
            carNameText.text = car.nazwa;
        }

        if (carHpText != null)
        {
            carHpText.text = "HP: " + car.hp.ToString();
        }
    }


}