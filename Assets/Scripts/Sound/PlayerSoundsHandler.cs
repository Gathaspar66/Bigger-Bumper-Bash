using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundsHandler : MonoBehaviour
{
    public static PlayerSoundsHandler instance;

    private void Awake()
    {
        instance = this;
    }

    public AudioSource engineAudioSource;

    public void AdjustEngineSound(float value)
    {
        engineAudioSource.pitch = value;
    }
}
