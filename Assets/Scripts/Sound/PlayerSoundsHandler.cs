using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundsHandler : MonoBehaviour
{
    public static PlayerSoundsHandler instance;

    public AudioSource engineAudioSource;
    public AudioSource accelerateEngineAudioSource;
    public AudioSource accelerateSlideAudioSource;
    public AudioSource breakEngineAudioSource;
    public AudioSource breakSlideAudioSource;

    bool previouslyAccelerating = false;
    bool previouslyBraking = false;

    private void Awake()
    {
        instance = this;
    }


    public void AdjustEngineSound(float value)
    {
        engineAudioSource.pitch = value;
    }

    internal void PlayerCarAccelerateSound(bool accelerating)
    {
        if (!accelerating)
        {
            previouslyAccelerating = false;
            return;
        }

        if (previouslyAccelerating) return;
        previouslyAccelerating = true;

        accelerateEngineAudioSource.Play();
        if (!accelerateEngineAudioSource.isPlaying)
        {
        }

        accelerateSlideAudioSource.Play();
        if (!accelerateSlideAudioSource.isPlaying)
        {
        }
    }

    internal void PlayerCarBreakSound(bool braking)
    {
        if (!braking)
        {
            previouslyBraking = false;
            return;
        }

        if (previouslyBraking) return;
        previouslyBraking = true;

        breakEngineAudioSource.Play();
        breakSlideAudioSource.Play();
        if (!breakEngineAudioSource.isPlaying)
        {
        }

        if (!breakSlideAudioSource.isPlaying)
        {
        }
    }
}
