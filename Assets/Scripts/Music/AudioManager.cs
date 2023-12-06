using System;
using UnityEngine;

// Manages sound effects being played.
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public Sound[] currentlyPlaying;
    public static AudioManager instance;
    
    // Set up singleton instance
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
        
        // Load each sound from the inspector into an AudioSource component that can be played.
        foreach (Sound sound in sounds)
        {
            var s = gameObject.AddComponent<AudioSource>();
            s.clip = sound.clip;
            s.volume = sound.volume;
            s.loop = sound.loop;
            
            sound.source = s;
        }
        
    }

    // Get a sound by name.
    Sound FindSound(string name)
    {
        return Array.Find(sounds, s => s.name == name);
    }
    
    // Plays a given sound.
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
