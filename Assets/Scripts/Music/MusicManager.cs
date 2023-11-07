using System;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public Sound[] sounds;
    public static MusicManager instance;
    
    [HideInInspector]
    public Sound currentTrack;
    
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
    
    void Start()
    {
        currentTrack = FindSound("Title Theme");
        Play(currentTrack);
    }

    Sound FindSound(string name)
    {
        return Array.Find(sounds, s => s.name == name);
    }
    
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

    void Play(Sound sound)
    {
        Debug.Log("Trying to play via sound " + sound?.name);
        if (sound == null || sound.source == null)
        {
            Debug.LogWarning("Error! Null Sound.");
        }
        sound?.source.Play();
    }

    IEnumerator FadeOut_Then_In_Internal(Sound oldSound, Sound newSound, float fadeDuration)
    {
        AudioSource oldMusic = oldSound.source;
        AudioSource newMusic = newSound.source;
        
        currentTrack = newSound;
        
        float startTime = Time.time;
        float normalizedTime = 0f;
        float elapsedTime = 0f;

        while (normalizedTime < 1f)
        {
            elapsedTime = Time.time - startTime;
            normalizedTime = Mathf.Clamp01(elapsedTime / fadeDuration);
            oldMusic.volume = 1.0f - normalizedTime;
            yield return null;
        }

        oldMusic.Stop();
        newMusic.Play();
        startTime = Time.time;
        normalizedTime = 0;


        while (normalizedTime < 1f)
        {
            elapsedTime = Time.time - startTime;
            normalizedTime = Mathf.Clamp01(elapsedTime / fadeDuration);
            newMusic.volume = normalizedTime;
            yield return null;
        }
        newMusic.volume = 1.0f;

    }

    void Fade_Out_Then_In(string from, string to, float fadeDuration)
    {
        StartCoroutine(FadeOut_Then_In_Internal(FindSound(from), FindSound(to), fadeDuration));
    }

    public void FadeTo(string to, float fadeDuration)
    {
        StartCoroutine(FadeOut_Then_In_Internal(currentTrack, FindSound(to), fadeDuration));
    }
}
