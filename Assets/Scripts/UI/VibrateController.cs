using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VibrateController : MonoBehaviour
{
    public Toggle vibrationToggle;
    private void Start()
    {
        bool vibrationEnabled = PlayerPrefs.GetInt("VibrationEnabled", 1) == 1;
        vibrationToggle.isOn = vibrationEnabled;
    }
    public void OnVibrationToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt("VibrationEnabled", isOn ? 1 : 0);
        PlayerPrefs.Save();
    }
}