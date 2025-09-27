using UnityEngine;
using UnityEngine.UI;

public class VibrateController : MonoBehaviour
{
    public Toggle vibrationToggle;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("VibrationEnabled"))
        {
            PlayerPrefs.SetInt("VibrationEnabled", 0);
            PlayerPrefs.Save();
        }
        bool vibrationEnabled = PlayerPrefs.GetInt("VibrationEnabled", 0) == 1;
        vibrationToggle.isOn = vibrationEnabled;

        vibrationToggle.onValueChanged.AddListener(delegate { OnVibrationToggleChanged(); });
    }

    public void OnVibrationToggleChanged()
    {
        PlayerPrefs.SetInt("VibrationEnabled", vibrationToggle.isOn ? 1 : 0);

        PlayerPrefs.Save();
    }
}