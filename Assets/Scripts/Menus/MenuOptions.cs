using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuOptions : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Toggle fullscreenToggle;
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
        
        fullscreenToggle.onValueChanged.AddListener(fullscreen =>
        {
            PlayerPrefs.SetInt("Fullscreen", Convert.ToInt32(fullscreen));
            Screen.fullScreenMode = fullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        });
        
    }
    
    public void UpdateSettings()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.8f);
        fullscreenToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("Fullscreen", 1));
    }
    
}
