using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;
    public GameObject aboutPanel;
    public GameObject mainMenuIngameMenu;
    public AudioMixer am;

    public GameObject menuButton;
    public GameObject aboutMenu;
    public Button startButton;
    public GameObject isCarChooseViewOpen;
    private Image startButtonImage;
    public TMP_Text carStatus;
    public Color activeColor = Color.green;
    public Color inactiveColor = Color.white;
    public TMP_Text textToUnlock;
    public TMP_Text statusToUnlock;
    public Light sceneDirectionalLight;

    [Header("Car Preview")]
    public Transform carChoiceContainer;

    private int currentCarIndex = 0;
    private GameObject spawnedCar;

    [Header("Stat Bars")]
    public ColorBar hpBar;

    public ColorBar speedBar;
    public ColorBar handlingBar;
    public ColorBar accelerationBar;
    public ColorBar torqueBar;
    private CarData car;

    private void Awake()
    {
        instance = this;
    }

    private List<CarData> CarObjects => CarDatabaseManager.instance != null ? CarDatabaseManager.instance.carObjects : new List<CarData>();

    private void Start()
    {
        Application.targetFrameRate = 120;
        ActivateManagers();
        UpdateHighScoreText();
        LoadSoundVolumeSettings();

        LoadSelectedCar();

        startButtonImage = startButton.GetComponent<Image>();

        ShowCar(currentCarIndex);
    }

    private void LoadSelectedCar()
    {
        int savedCarType = PlayerPrefs.GetInt("SelectedCar", -1);

        if (savedCarType != -1)
        {
            for (int i = 0; i < CarObjects.Count; i++)
            {
                if ((int)CarObjects[i].carType == savedCarType)
                {
                    currentCarIndex = i;
                    return;
                }
            }
        }

        currentCarIndex = 0;
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
        PlayerPrefs.Save();
        mainMenuIngameMenu.SetActive(false);
        menuButton.SetActive(true);
    }

    public void OnNextCar()
    {
        currentCarIndex = (currentCarIndex + 1) % CarObjects.Count;
        ShowCar(currentCarIndex);
    }

    public void OnPreviousCar()
    {
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

        car = cars[index];

        if (car.carPrefab != null)
        {
            spawnedCar = Instantiate(
                car.carPrefab,
                carChoiceContainer.position,
                carChoiceContainer.rotation,
                carChoiceContainer
            );
            spawnedCar.name = car.name;
        }

        HandleLockedCarState();
        SetCarStats();

        PlayerPrefs.SetInt("SelectedCar", (int)car.carType);
        PlayerPrefs.Save();
    }

    private void HandleLockedCarState()
    {
        int collectedBarrels = PlayerPrefs.GetInt("CollectedBarrels", 0);
        int hitBarriers = PlayerPrefs.GetInt("HitBarriers", 0);
        int highScore = PlayerPrefs.GetInt("highScore", 0);
        if (car.isUnlocked)
        {
            carStatus.text = "";
            textToUnlock.text = car.name;

            startButtonImage.color = inactiveColor;
            startButton.interactable = true;
            sceneDirectionalLight.intensity = 2f;
            statusToUnlock.text = "";
        }
        else
        {
            carStatus.text = "LOCKED";
            textToUnlock.text = car.textToUnlock;
            startButton.interactable = false;
            startButtonImage.color = activeColor;
            sceneDirectionalLight.intensity = 0f;
            statusToUnlock.text = car.carType switch
            {
                CarType.OGIER => $"{collectedBarrels}/100 barrels",
                CarType.MOTORCAR => $"{hitBarriers}/17 barriers",
                CarType.PUDZIAN => "",
                CarType.PICKUP => $"{highScore}/100000",
                _ => "",
            };
        }
    }

    public void SetCarStats()
    {
        UpdateHpBar(car.hp);
        UpdateAccelerationBar((int)car.acceleationMultiplier);
        UpdateSpeedBar((int)car.maxForwardVelocity);
        UpdateHandlingBar((int)car.brakeMultiplier);
        UpdateTorqueBar(car.steeringMultiplier);
    }

    private void UpdateHpBar(int hp)
    {
        int min = 0;
        int step = 1;

        int activeSegments = Mathf.Clamp((hp - min) / step, 0, hpBar.maxSegments);
        hpBar.SetValue(activeSegments);
    }

    private void UpdateAccelerationBar(int handling)
    {
        int min = 10;
        int step = 10;

        int activeSegments = Mathf.Clamp((handling - min) / step, 0, accelerationBar.maxSegments);
        accelerationBar.SetValue(activeSegments);
    }

    private void UpdateHandlingBar(int handling)
    {
        int min = 10;
        int step = 10;

        int activeSegments = Mathf.Clamp((handling - min) / step, 0, handlingBar.maxSegments);
        handlingBar.SetValue(activeSegments);
    }

    private void UpdateTorqueBar(float handling)
    {
        float min = 0.9f;
        float step = 0.1f;

        int activeSegments = Mathf.Clamp(Mathf.FloorToInt((handling - min) / step), 0, torqueBar.maxSegments);
        torqueBar.SetValue(activeSegments);
    }

    private void UpdateSpeedBar(int speed)
    {
        int min = 10;
        int step = 3;

        int activeSegments = Mathf.Clamp((speed - min) / step, 0, speedBar.maxSegments);
        speedBar.SetValue(activeSegments);
    }

    public void PlayMenuClickSound()
    {
        SoundManager.instance.PlayMenuClickSound();
    }
}