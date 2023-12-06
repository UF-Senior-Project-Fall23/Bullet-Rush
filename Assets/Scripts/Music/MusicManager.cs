using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

// Handles the music playing.
public class MusicManager : MonoBehaviour
{
    public Sound[] sounds;
    public static MusicManager instance;

    [HideInInspector] public Sound currentTrack;

    // Set up singleton instance.
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        // Load each track from the inspector into an AudioSource component that can be played.
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.loop = sound.loop;
            sound.source.spatialBlend = 0;
            sound.source.pitch = 1;
        }
    }

    // Play title theme on game startup.
    void Start()
    {
        currentTrack = FindSound("Title Theme");
        currentTrack.source.volume = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
        Play(currentTrack);
    }

    // Find a sound by name.
    Sound FindSound(string name)
    {
        return Array.Find(sounds, s => s.name == name);
    }

    // Play a sound instantly by name.
    void Play(string name)
    {
        Debug.Log("Trying to play via name " + name);
        Sound s = FindSound(name);
        if (s == null)
        {
            Debug.LogWarning("Error! Unknown Sound. (" + name + ")");
            return;
        }

        s.source.Play();
    }

    // Play a sound instantly by value.
    void Play(Sound sound)
    {
        Debug.Log("Trying to play via sound " + sound?.name);
        if (sound == null || sound.source == null)
        {
            Debug.LogWarning("Error! Null Sound.");
        }

        sound?.source.Play();
    }

    // INTERNAL: Smoothly fades out the given sound then smoothly fades in the new sound.
    private IEnumerator FadeOutThenIn_Internal([CanBeNull] Sound oldSound, Sound newSound, float fadeDuration)
    {
        if (oldSound != null)
        {
            yield return StartCoroutine(FadeOut_Internal(oldSound, fadeDuration));
        }

        currentTrack = newSound;
        
        yield return StartCoroutine(FadeIn_Internal(newSound, fadeDuration));
    }

    // INTERNAL: Smoothly fades out the given sound
    private IEnumerator FadeOut_Internal(Sound sound, float fadeDuration)
    {
        AudioSource src = sound.source;
        var startTime = Time.time;
        var normalizedTime = 0f;
        
        while (normalizedTime < 1f)
        {
            var elapsedTime = Time.time - startTime;
            normalizedTime = Mathf.Clamp01(elapsedTime / fadeDuration);
            src.volume = (1.0f - normalizedTime) * PlayerPrefs.GetFloat("MusicVolume", 0.8f);
            yield return null;
        }

        src.Stop();
    }

    // INTERNAL: Smoothly fades in the given sound
    private IEnumerator FadeIn_Internal(Sound sound, float fadeDuration)
    {
        AudioSource src = sound.source;
        var startTime = Time.time;
        var normalizedTime = 0f;

        src.volume = 0f;
        src.Play();

        while (normalizedTime < 1f)
        {
            var elapsedTime = Time.time - startTime;
            normalizedTime = Mathf.Clamp01(elapsedTime / fadeDuration);
            src.volume = normalizedTime * PlayerPrefs.GetFloat("MusicVolume", 0.8f);
            yield return null;
        }

        src.volume = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
    }

    // INTERNAL: Smoothly fades out the first sound then fades in the second sound
    private IEnumerator FadeOutThenPlay_Internal(Sound oldSound, Sound newSound, float fadeDuration)
    {
        yield return StartCoroutine(FadeOut_Internal(oldSound, fadeDuration));
        
        newSound.source.volume = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
        currentTrack = newSound;
        Play(newSound);
    }
    
    // Smoothly fades out the current sound then instantly starts the given sound.
    public void FadeOutThenPlay(string to, float fadeDuration)
    {
        StartCoroutine(FadeOutThenPlay_Internal(currentTrack, FindSound(to), fadeDuration));
    }

    // Smoothly fades out the current sound then fades into the given sound.
    public void FadeCurrentInto(string to, float fadeDuration)
    {
        StartCoroutine(FadeOutThenIn_Internal(currentTrack, FindSound(to), fadeDuration));
    }

    // Fades out the current sound
    public void FadeOut(float fadeDuration)
    {
        StartCoroutine(FadeOut_Internal(currentTrack, fadeDuration));
    }

    // Fades in the current music track
    public void FadeIn(float fadeDuration)
    {
        StartCoroutine(FadeIn_Internal(currentTrack, fadeDuration));
    }

    // Plays the music for a given boss.
    public void LoadBossMusic(string bossName)
    {
        string toPlay;
        
        switch (bossName)
        {
            case "Cordelia":
                toPlay = "Cordelia Theme";
                break;
            case "Blagthoroth":
                toPlay = "Blagthoroth Theme";
                break;
            case "Onyx":
                toPlay = "Onyx Theme";
                break;
            default:
                toPlay = null;
                break;
        }

        if (toPlay != null)
        {
            FadeOutThenPlay(toPlay, 0.5f);
        }
        else
        {
            Debug.LogWarning($"No music found for boss {bossName}");
        }
    }

}
