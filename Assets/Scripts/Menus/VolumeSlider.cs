using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private AudioManager audioManager;
    
    void Start()
    {
        musicSlider.onValueChanged.AddListener(value =>
        {
            PlayerPrefs.SetFloat("MusicVolume", value);
            musicManager.currentTrack.source.volume = value;
        });
        
        sfxSlider.onValueChanged.AddListener(value =>
        {
            PlayerPrefs.SetFloat("SFXVolume", value);
            foreach (var sfx in audioManager.currentlyPlaying)
            {
                sfx.source.volume = value;
            }
        });
    }
    
}
