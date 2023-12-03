using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms.Impl;

public class HUDManager : MonoBehaviour
{
    public static HUDManager instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI weaponText;
    public TextMeshProUGUI heatText;
    public FillableBar heatBar;
    public GameObject tooltip;

    GameObject DeathScreen;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI difficultyText;

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

    // Start is called before the first frame update
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

    void FixedUpdate()
    {
        timeText.text = "Time Elapsed: " + Mathf.Floor(GameManager.gameTime).ToString() + " s";
    }

    void UpdateHealthText(float current, float max)
    {
        Debug.LogWarning($"Setting HP Text to {current}/{max}");
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

    public void ShowTooltip(string name, string description, Vector3 pos)
    {
        tooltip.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = name;
        tooltip.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = description;
        tooltip.transform.position = pos;
        tooltip.SetActive(true);
    }

    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }

    public void ShowDeathScreen()
    {
        DeathScreen.SetActive(true);
    }

    public void HideDeathScreen()
    {
        DeathScreen.SetActive(false);
    }
}
