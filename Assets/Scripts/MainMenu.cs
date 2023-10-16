using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayButton()
    {
        // Load the game whenever Play is pressed. Change the ID later, atm just loads SampleScene.
        SceneManager.LoadScene(0);
    }

    public void ExitButton()
    {
        Debug.Log("Exit button pressed.");
        Application.Quit();
    }

    public void OptionsButton()
    {
        
    }
}
