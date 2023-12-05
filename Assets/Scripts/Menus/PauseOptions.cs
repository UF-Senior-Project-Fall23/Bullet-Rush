using System;
using UnityEngine;
using UnityEngine.UI;


// TODO: Figure out a way to merge this with the MenuOptions.cs script so it's not just redundant code
// This version is needed because the MusicManager and AudioManager object don't exist in the AlphaTest scene.
public class PauseOptions : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Toggle fullscreenToggle;
    private MusicManager musicManager;
    private AudioManager audioManager;
    public static PauseOptions instance;

    void Awake()
    {
        if (instance is null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        musicManager = MusicManager.instance;
        audioManager = AudioManager.instance;
        
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
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        fullscreenToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("Fullscreen"));
    }
}