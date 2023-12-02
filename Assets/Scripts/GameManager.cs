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
    public Dictionary<string, Vector3> warpCoordinates;
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
            warpCoordinates = new();
            warpCoordinates["Default"] = new Vector3(30, 24, 0); // Set to Start Area
            foreach (Transform child in transform)
            {
                if (child.gameObject.CompareTag("Teleport"))
                {
                    var warpname = child.gameObject.name;
                    warpCoordinates[warpname.Remove(warpname.LastIndexOf("Teleport"))] = child.position;
                }
            }
            Debug.LogWarning($"Created Warp Locations {string.Join(", ", warpCoordinates)}");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        roomType = RoomType.Start;

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
        return getWarpLocation(BossController.instance.currentBoss.name);
    }

    public Vector3 getWarpLocation(string destination)
    {
        if (destination.EndsWith("(Clone)"))
        {
            destination = destination.Remove(destination.LastIndexOf("(Clone)"));
        }
        
        if (warpCoordinates.ContainsKey(destination))
        {
            return warpCoordinates[destination];
        }

        Debug.LogWarning($"Error! Attempting to warp to unknown destination \"{destination}\"");
        return warpCoordinates["Default"];
    }
    
    public Vector3 getLootRoomLocation()
    {
        return getWarpLocation("LootRoom");
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

    public int getCurrentDifficultyInt()
    {
        return ((int)difficulty);
    }

    public Vector3 getStartAreaLocation()
    {
        return getWarpLocation("StartArea");
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
        PerkManager.instance.ResetHeldPerks();
        PerkManager.instance.ResetPerks();
        MusicManager.instance?.FadeCurrentInto("Start Area Theme", 0.5f);
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
