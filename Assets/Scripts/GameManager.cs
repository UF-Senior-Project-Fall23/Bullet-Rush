using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using Unity.VisualScripting;

public enum Difficulty
{
    Easy,
    Medium,
    Hard
}

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public List<GameObject> levelCoordinates;
    public Dictionary<string, GameObject> bulletPrefabs;

    public float gameTime = 0f;

    [HideInInspector]
    public UnityEvent ScoreChanged;
    public int score = 0;

    [HideInInspector]
    public UnityEvent DifficultyChanged;
    public Difficulty difficulty = Difficulty.Easy;

    [HideInInspector]
    public UnityEvent LevelChanged;
    public int currentLevel = 0;
    public bool inLootRoom = true;

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
        inLootRoom = true;
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

        bulletPrefabs = Resources.LoadAll<GameObject>("Prefabs/Bullets").ToDictionary(x => x.name, x => x);
    }

    private void FixedUpdate()
    {
        gameTime += Time.deltaTime;
    }

    public void AddScore(int type)
    {
        switch (type)
        {
            case 1:
                score += 10;
                break;
        }
        ScoreChanged.Invoke();
    }

    public void incrementLevel()
    {
        currentLevel += 1;
        LevelChanged.Invoke();
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

    public GameObject getBulletPrefab(string name)
    {
        return bulletPrefabs[name];
    }
    public void setDifficulty(Difficulty newDifficulty)
    {
        difficulty = newDifficulty;
        DifficultyChanged.Invoke();
    }
    public Difficulty getCurrentDifficulty()
    {
        return difficulty;
    }
}
