using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void OptionsButton()
    {
        
    }
}
