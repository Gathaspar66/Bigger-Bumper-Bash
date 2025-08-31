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

    [Header("Car Preview")]
    public Transform carChoiceContainer;

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

        CarData car = cars[index];

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

        if (carNameText != null)
        {
            carNameText.text = car.name;
        }

        if (carHpText != null)
        {
            carHpText.text = "HP: " + car.hp.ToString();
        }

        HandleLockedCarState(car);

        PlayerPrefs.SetInt("SelectedCar", (int)car.carType);
        PlayerPrefs.Save();
    }

    private void HandleLockedCarState(CarData car)
    {
        if (car.isUnlocked)
        {
            carStatus.text = "";
            textToUnlock.text = "";
            startButtonImage.color = inactiveColor;
            startButton.interactable = true;

        }
        else
        {
            carStatus.text = "LOCKED";
            textToUnlock.text = car.textToUnlock;
            startButton.interactable = false;
            startButtonImage.color = activeColor;
        }
    }
}