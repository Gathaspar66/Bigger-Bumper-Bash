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
    Default
}

public class SoundManager : MonoBehaviour
{
    [Header("Audio Clips")]//
    public AudioClip pointsClip;

    public AudioClip crashClip;
    public List<AudioClip> levelClipsList;

    [Header("Audio Sources")]//
    public AudioSource sfxSource;

    public AudioSource musicSource;

    [Header("Audio Mixer Groups")]//
    public AudioMixerGroup sfxMixerGroup;

    public AudioMixerGroup musicMixerGroup;

    [Header("Audio Mixer")]//
    public AudioMixer audioMixer;

    public static SoundManager instance;
    public Toggle myToggle;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;
    public Image soundIcon;
    private void Awake()
    {
        instance = this;
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
    }

    public void PlaySFX(SoundEffect effect)
    {
        AudioClip clip = GetClip(effect);
        if (clip != null)
        {
            GameObject tempAudio = new("SFX_" + effect);
            AudioSource source = tempAudio.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = sfxMixerGroup;
            source.clip = clip;
            source.volume = 01.01f;
            source.Play();
            Destroy(tempAudio, clip.length);
        }
        else
        {
            Debug.LogWarning($"Clip for {effect} not assigned.");
        }
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
            SoundEffect.POINTS_SOUND => pointsClip,
            SoundEffect.CRASH_SOUND => crashClip,
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
}