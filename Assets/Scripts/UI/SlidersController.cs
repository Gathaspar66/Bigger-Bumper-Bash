using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SlidersController : MonoBehaviour
{
    public Slider musicSlider;
    public Slider soundSlider;

    public AudioMixer am;

    private void Start()
    {
        LoadVolumeValues();
    }

    private void LoadVolumeValues()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        soundSlider.value = PlayerPrefs.GetFloat("soundVolume");
    }

    public void OnMusicSliderChanged()
    {
        am.SetFloat("Music", Mathf.Log10(Mathf.Clamp(musicSlider.value, 0.0001f, 1f)) * 20f);
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
    }

    public void OnSoundSliderChanged()
    {
        am.SetFloat("SFX", Mathf.Log10(Mathf.Clamp(soundSlider.value, 0.0001f, 1f)) * 20f);
        PlayerPrefs.SetFloat("soundVolume", soundSlider.value);
    }
}
