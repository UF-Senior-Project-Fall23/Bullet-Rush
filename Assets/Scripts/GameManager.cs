using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI weaponText;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI difficultyText;
    public List<GameObject> levelCoordinates;

    private float gameTime = 0f;
    private int score = 0;

    private int difficulty = 0; //0 = easy, 1 = medium, 2 = hard

    private int currentLevel = 0;

    public bool inLootRoom = false;

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
    }

    private void Start()
    {
        //levelCoordinates[0] is the lootRoom location
        //levelCoordinates[1] is level 1 location
        //... and so on
        //levelCoordinates[6] is the start room
        foreach (Transform child in transform)
        {
            if (child.gameObject.CompareTag("Teleport"))
            {
                levelCoordinates.Add(child.gameObject);
            }
        }

        weaponText.text = "Weapon: None";
    }

    void FixedUpdate()
    {
        gameTime += Time.deltaTime;
        timeText.text = "Time Elapsed: " + Mathf.Floor(gameTime).ToString();
        scoreText.text = "Score: " + score.ToString();
        healthText.text = "Health: " + PlayerController.instance.CurrentHealth.ToString();
        levelText.text = "Level: " + currentLevel.ToString();
        if (difficulty == 0)
        {
            difficultyText.text = "Difficulty: Easy";
        }
        else if (difficulty == 1)
        {
            difficultyText.text = "Difficulty: Medium";
        }
        else
        {
            difficultyText.text = "Difficulty: Hard";
        }
    }

    public void AddScore(int type)
    {
        switch (type)
        {
            case 1:
                score += 10;
                break;
        }
    }

    public void incrementLevel()
    {
        currentLevel += 1;
    }
    public int getCurrentLevel()
    {
        return currentLevel;
    }
    public Vector3 getNextLevelLocation()
    {
        Vector3 nextLevelLocation = levelCoordinates[currentLevel + 1].transform.position;
        return nextLevelLocation;
    }
    public Vector3 getLootRoomLocation()
    {
        Vector3 lootRoomVector = levelCoordinates[0].transform.position;
        return lootRoomVector;
    }
    public void setDifficulty(int newDifficulty)
    {
        difficulty = newDifficulty;
    }
    public int getCurrentDifficulty()
    {
        return difficulty;
    }
}
