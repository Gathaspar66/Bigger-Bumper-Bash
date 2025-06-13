using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [Header("Sound Configuration")] public List<Sound> sounds = new();

    [Header("Audio Sources")] public AudioSource sfxSource;
    public AudioSource musicSource;

    private readonly Dictionary<string, AudioClip> soundMap = new();

    private void Awake()
    {
        instance = this;
        InitializeSoundMap();
    }

    public void Activate()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        switch (currentScene)
        {
            case "MainMenu":
                PlayMusic("sleep");
                break;

            case "Level1":
                PlayMusic("sleep");
                break;

            default:
                PlayMusic("default");
                break;
        }
    }

    private void InitializeSoundMap()
    {
        foreach (Sound sound in sounds)
        {
            if (!soundMap.ContainsKey(sound.id))
            {
                soundMap[sound.id] = sound.clip;
            }
            else
            {
                Debug.LogWarning($"Duplicate sound ID found: {sound.id}");
            }
        }
    }

    public void PlaySFX(string id)
    {
        return;
        if (soundMap.TryGetValue(id, out AudioClip clip))
        {
            GameObject tempAudio = new("SFX_" + id);
            AudioSource audioSource = tempAudio.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.volume = 0.1f;
            audioSource.Play();

            Destroy(tempAudio, clip.length);
        }
        else
        {
            Debug.LogWarning($"SFX with ID '{id}' not found.");
        }
    }

    public void PlayMusic(string id)
    {
        return;
        if (soundMap.TryGetValue(id, out AudioClip clip))
        {
            if (musicSource.clip != clip)
            {
                musicSource.clip = clip;
                musicSource.loop = true;
                musicSource.Play();
            }
        }
        else
        {
            Debug.LogWarning($"Music with ID '{id}' not found.");
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