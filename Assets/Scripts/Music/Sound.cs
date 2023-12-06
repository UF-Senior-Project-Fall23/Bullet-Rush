using System;
using UnityEngine;

// Represents a sound that can be played by the Audio or Music managers.
[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
        
    [Range(0f,1f)]
    public float volume;

    public bool loop;

    [HideInInspector] public AudioSource source;
}