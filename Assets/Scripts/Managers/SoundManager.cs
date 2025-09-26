using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SoundEffect
{
    POINTS_SOUND,
    CRASH_SOUND,
    LEVEL1_MUSIC,
    BARRIER_SCRAPE,
    MENU_CLICK,
    Default
}

public class SoundManager : MonoBehaviour
{
    [Header("Audio Clips")]//
    public List<AudioClip> levelClipsList;

    [Header("Audio Sources")]//
    public AudioSource musicSource;

    public AudioSource crashSource;
    public AudioSource pointsSource;
    public AudioSource barrierScrapeSource;
    public AudioSource menuClickSource;

    [Header("Audio Mixer Groups")]//
    public AudioMixerGroup sfxMixerGroup;

    internal void MuteEngineSound(bool muteEngine)
    {
        PlayerSoundsHandler.instance.ToggleEngineSound(muteEngine);
    }

    public AudioMixerGroup musicMixerGroup;

    [Header("Audio Mixer")]//
    public AudioMixer audioMixer;

    public static SoundManager instance;
    public Toggle myToggle;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;
    public Image soundIcon;
    private bool isAlive = true;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
    }

    public void Activate()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        switch (currentScene)
        {
            case "MainMenu":
                PlayMusic(SoundEffect.LEVEL1_MUSIC);
                break;

            case "Level1":
                PlayMusic(SoundEffect.LEVEL1_MUSIC);
                break;

            default:
                PlayMusic(SoundEffect.Default);
                break;
        }

        CheckFirstRun();
    }

    private void CheckFirstRun()
    {
        //default value is 0
        if (PlayerPrefs.GetInt("firstGameRun") == 0)
        {
            Debug.Log("FIRST GAME RUN DETECTED");
            PlayerPrefs.SetInt("firstGameRun", 1);
            //set default values, then adjust mixer
            PlayerPrefs.SetFloat("musicVolume", 0.8f);
            PlayerPrefs.SetFloat("soundVolume", 0.8f);
            PlayerPrefs.Save();
        }
    }

    public void UpdateVolumes()
    {
        _ = audioMixer.SetFloat("Music", Mathf.Log10(Mathf.Clamp(PlayerPrefs.GetFloat("musicVolume"), 0.0001f, 1f)) * 20f);
        _ = audioMixer.SetFloat("SFX", Mathf.Log10(Mathf.Clamp(PlayerPrefs.GetFloat("soundVolume"), 0.0001f, 1f)) * 20f);
    }

    public void PlayCrashSound()
    {
        crashSource.Play();
    }

    public void PlayMenuClickSound()
    {
        menuClickSource.Play();
    }

    public void PlayPointsSound()
    {
        pointsSource.Play();
    }

    public void PlayMusic(SoundEffect effect)
    {
        AudioClip clip = GetClip(effect);
        if (clip != null && musicSource.clip != clip)
        {
            musicSource.outputAudioMixerGroup = musicMixerGroup;
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }
        else if (clip == null)
        {
            Debug.LogWarning($"Music clip for {effect} not assigned.");
        }
    }

    private AudioClip GetClip(SoundEffect effect)
    {
        AudioClip levelClip = levelClipsList[Random.Range(0, levelClipsList.Count)];
        return effect switch
        {
            SoundEffect.LEVEL1_MUSIC => levelClip,

            _ => null,
        };
    }

    public void ToggleSound(bool enableSound)
    {
        if (enableSound)
        {
            _ = audioMixer.SetFloat("MainMusic", 0f);
            soundIcon.sprite = soundOnSprite;
        }
        else
        {
            _ = audioMixer.SetFloat("MainMusic", -80f);
            soundIcon.sprite = soundOffSprite;
        }
    }

    public void StopMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }

    public void StartBarrierScrape()
    {
        if (!isAlive)
        {
            return;
        }

        if (!barrierScrapeSource.isPlaying)
        {
            barrierScrapeSource.Play();
        }
    }

    public void StopBarrierScrape()
    {
        if (barrierScrapeSource.isPlaying)
        {
            barrierScrapeSource.Stop();
        }
    }

    public void AdjustEngineSound(float value)
    {
        PlayerSoundsHandler.instance.AdjustEngineSound(value);
    }

    internal void PlayerCarAccelerateSound(bool accelerating)
    {
        PlayerSoundsHandler.instance.PlayerCarAccelerateSound(accelerating);
    }

    internal void PlayerCarBreakSound(bool braking)
    {
        PlayerSoundsHandler.instance.PlayerCarBreakSound(braking);
    }

    public void Vibrate()
    {
        if (PlayerPrefs.GetInt("VibrationEnabled", 1) == 1)
        {
#if UNITY_ANDROID || UNITY_IOS
            Handheld.Vibrate();
#endif
        }
    }

    public void OnPlayerDeath()
    {
        StopBarrierScrape();
        isAlive = false;
    }
}