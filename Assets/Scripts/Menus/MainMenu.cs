using UnityEngine;
using UnityEngine.SceneManagement;

// Handles the buttons on the main menu.
public class MainMenu : MonoBehaviour
{
    public void PlayButton()
    {
        GameObject.FindWithTag("MusicManager").GetComponent<MusicManager>().FadeOut(0.5f);

        SceneManager.LoadScene("AlphaTest");
    }

    public void ExitButton()
    {
        Debug.Log("Exit button pressed.");
        Application.Quit();
    }

    // Unused due to being implemented in the object inspector instead.
    public void OptionsButton()
    {
        
    }
}
