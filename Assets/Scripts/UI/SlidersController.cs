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
        SoundManager.instance.UpdateVolumes();
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
    }

    public void OnSoundSliderChanged()
    {
        SoundManager.instance.UpdateVolumes();
        PlayerPrefs.SetFloat("soundVolume", soundSlider.value);
    }
}
