using UnityEngine;
using UnityEngine.UI;

public class VibrateController : MonoBehaviour
{
    public Toggle vibrationToggle;

    private void Awake()
    {
        bool vibrationEnabled = PlayerPrefs.GetInt("VibrationEnabled") == 1;
        vibrationToggle.isOn = vibrationEnabled;

        //vibrationToggle.onValueChanged.AddListener(delegate { OnVibrationToggleChanged(); });
    }

    public void OnVibrationToggleChanged()
    {
        PlayerPrefs.SetInt("VibrationEnabled", vibrationToggle.isOn ? 1 : 0);

        PlayerPrefs.Save();
    }

    public void awezsprawdz()
    {
        print("on vaue changed");
    }
}