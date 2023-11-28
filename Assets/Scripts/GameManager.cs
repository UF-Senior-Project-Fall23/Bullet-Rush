using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public enum Difficulty
{
    Easy,
    Medium,
    Hard
}

public enum RoomType
{
    Start,
    LootRoom,
    Boss,
    Error
}

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public List<GameObject> levelCoordinates;
    public Dictionary<string, GameObject> bulletPrefabs;

    public static float gameTime = 0f;

    [HideInInspector]
    public UnityEvent ScoreChanged;
    public int score = 0;

    [HideInInspector]
    public UnityEvent DifficultyChanged;
    public Difficulty difficulty = Difficulty.Easy;

    [HideInInspector]
    public UnityEvent LevelChanged;
    public int currentLevel = 0;

    public RoomType roomType;
    
    public bool inLootRoom => roomType == RoomType.LootRoom;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        roomType = RoomType.Start;
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
    public void setLevel(int value)
    {
        currentLevel = value;
        LevelChanged.Invoke();
    }
    public int getCurrentLevel()
    {
        return currentLevel;
    }
    public Vector3 getNextLevelLocation()
    {
        // Vector3 nextLevelLocation = levelCoordinates[currentLevel + 1].transform.position;
        // return nextLevelLocation;
        return levelCoordinates[1].transform.position;
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

    public Vector3 getStartAreaLocation()
    {
        return new Vector3(30, 25, 0);
    }

    public Vector3 getLootRoomExitLocation()
    {
        return new Vector3(-54.25f, -53.5f, 0);
    }

    public void GoToStart()
    {
        PlayerController.instance.transform.position = getStartAreaLocation();
        roomType = RoomType.Start;
        setLevel(0);
        MusicManager.instance.FadeCurrentInto("Start Area Theme", 0.5f);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Loaded scene {scene.name} with mode {mode}");
        if (scene.name == "AlphaTest")
        {
            GoToStart();
        }
    }
    
    
}
