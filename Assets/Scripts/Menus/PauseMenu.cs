using UnityEngine;
using UnityEngine.SceneManagement;

// Handles the pause menu while ingame.
public class PauseMenu : MonoBehaviour
{

    [HideInInspector] public bool paused = false;
    public GameObject pauseMenu;
    public GameObject options;
    
    // Detects whether the user presses Esc. to open/close the menu.
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

    // Pauses the game and shows the pause menu.
    public void Pause()
    {
        paused = true;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }

    // Unpauses the game and hides the pause menu.
    public void Resume()
    {
        paused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    // TODO: Make this not irreparably break whenever you re-enter the game. The button is temporarily disabled due to that.
    // Takes the player back to the main menu.
    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        SceneManager.UnloadSceneAsync("AlphaTest");
        MusicManager.instance.FadeCurrentInto("Title Theme", 0.5f);
    }

    // Unused due to behavior being implemented in the editor.
    public void OptionsMenu()
    {
    }

    // Closes the game
    public void ExitGame()
    {
        Application.Quit();
    }
}
