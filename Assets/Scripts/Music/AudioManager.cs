using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public Sound[] currentlyPlaying;
    public static AudioManager instance;
    
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
            var s = gameObject.AddComponent<AudioSource>();
            s.clip = sound.clip;
            s.volume = sound.volume;
            s.loop = sound.loop;
            
            sound.source = s;
        }
        
    }

    Sound FindSound(string name)
    {
        return Array.Find(sounds, s => s.name == name);
    }
    
    void Play(string name)
    {
        Sound s = FindSound(name);
        if (s == null)
        {
            Debug.LogWarning("Error! Unknown Sound. (" + name + ")");
            return;
        }
        
        s.source.Play();
        
    }
    
}
