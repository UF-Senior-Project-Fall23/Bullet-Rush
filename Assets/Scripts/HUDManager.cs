using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Manages the HUD UI while in game.
public class HUDManager : MonoBehaviour
{
    public static HUDManager instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI weaponText;
    public TextMeshProUGUI heatText;
    public FillableBar heatBar;
    public GameObject tooltipPrefab;
    public GameObject tooltipCanvas;
    public TextMeshProUGUI winstreakText;

    GameObject DeathScreen;
    List<GameObject> tooltips;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI difficultyText;

    // Set up singleton instance
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DeathScreen = transform.Find("DeathScreen").gameObject;
    }

    // Set up change listeners and fill in basic values for HUD displays.
    void Start()
    {
        weaponText.text = "Weapon: None";
        UpdateScoreText();
        UpdateHealthText(PlayerController.instance.health.CurrentHealth, PlayerController.instance.health.MaxHealth);
        UpdateDifficultyText();
        UpdateLevelText();

        PlayerController.instance.health.HPChange.AddListener(UpdateHealthText);
        GameManager.instance.ScoreChanged.AddListener(UpdateScoreText);
        GameManager.instance.DifficultyChanged.AddListener(UpdateDifficultyText);
        GameManager.instance.LevelChanged.AddListener(UpdateLevelText);
    }

    // Called a fixed number of times per second, currently updates the time elapsed.
    void FixedUpdate()
    {
        timeText.text = "Time Elapsed: " + Mathf.Floor(GameManager.gameTime).ToString() + " s";
    }

    void UpdateHealthText(float current, float max)
    {
        healthText.text = $"HP: {current}/{max}";
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + GameManager.instance.score.ToString();
    }

    void UpdateDifficultyText()
    {
        difficultyText.text = "Difficulty: " + GameManager.instance.difficulty.ToString();
    }

    void UpdateLevelText()
    {
        levelText.text = "Level: " + GameManager.instance.currentLevel.ToString();
    }

    // Utility to display a tooltip with a title and description somewhere. Used for weapons and perks.
    public GameObject CreateTooltip(string title, string description, Vector3 pos)
    {
        GameObject tooltip = Instantiate(tooltipPrefab, tooltipCanvas.transform);
        tooltip.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = title;
        tooltip.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = description;
        tooltip.transform.position = pos;
        return tooltip;
    }

    public void HideTooltip(GameObject tooltip)
    {
        Destroy(tooltip);
    }

    public void ShowDeathScreen()
    {
        DeathScreen.SetActive(true);
        winstreakText.text = $"Win Streak: {GameManager.instance.winstreak}";
    }

    public void HideDeathScreen()
    {
        DeathScreen.SetActive(false);
    }
}
