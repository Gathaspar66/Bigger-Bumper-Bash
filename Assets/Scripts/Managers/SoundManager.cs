using UnityEngine;
using UnityEngine.SceneManagement;

public enum SoundEffect
{
    POINTS_SOUND,
    CRASH_SOUND,
    LEVEL1_MUSIC,
    Default
}

public class SoundManager : MonoBehaviour
{
    [Header("Audio Clips")] //
    public AudioClip pointsClip;

    public AudioClip crashClip;
    public AudioClip level1Clip;

    [Header("Other")] //
    public AudioSource sfxSource;

    public AudioSource musicSource;
    public static SoundManager instance;

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
            source.clip = clip;
            source.volume = 0.01f;
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
        return effect switch
        {
            SoundEffect.POINTS_SOUND => pointsClip,
            SoundEffect.CRASH_SOUND => crashClip,
            SoundEffect.LEVEL1_MUSIC => level1Clip,

            _ => null,
        };
    }

    public void StopMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }
}