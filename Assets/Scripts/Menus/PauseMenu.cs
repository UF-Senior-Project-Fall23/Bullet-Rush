using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [HideInInspector] public bool paused = false;
    public GameObject pauseMenu;
    public GameObject options;
    
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            print("Pressed Escape!");
            paused = !paused;

            if (paused) Pause();
            else Resume();

        }
    }

    public void Pause()
    {
        paused = true;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }

    public void Resume()
    {
        paused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    // TODO: Make this not irreparably break whenever you re-enter the game. The button is temporarily disabled due to that.
    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        SceneManager.UnloadSceneAsync("AlphaTest");
        MusicManager.instance.FadeCurrentInto("Title Theme", 0.5f);
    }

    public void OptionsMenu()
    {
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
