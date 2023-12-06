using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

// Represents the game difficulty.
public enum Difficulty
{
    Easy,
    Medium,
    Hard
}

// Represents a possible room the player can be in.
public enum RoomType
{
    Start,
    LootRoom,
    Boss,
    Error // Unknown or invalid room type
}

// Handles the basic game loop and provides utility information.
public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    
    // Dictionary of possible warp locations to their destination.
    public Dictionary<string, Vector3> warpCoordinates;
    
    // Dictionary of bullet and projectile types by name.
    public Dictionary<string, GameObject> bulletPrefabs;

    public static float gameTime = 0f;

    // Event that detects when the player's score changes
    // TODO: Deprecate?
    [HideInInspector]
    public UnityEvent ScoreChanged;
    public int score = 0;

    // Event that detects when the player changes their difficulty.
    [HideInInspector]
    public UnityEvent DifficultyChanged;
    public Difficulty difficulty = Difficulty.Easy;

    // Event that detects when the player increases their current level (usually after beating a boss).
    [HideInInspector]
    public UnityEvent LevelChanged;
    public int currentLevel = 0;

    public RoomType roomType;
    
    public bool inLootRoom => roomType == RoomType.LootRoom;

    // Set up singleton instance and configure
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            
            SceneManager.sceneLoaded += OnSceneLoaded;
            
            // Set up Warp Location dictionary
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

        // Generate bullet dictionary from files
        bulletPrefabs = Resources.LoadAll<GameObject>("Prefabs/Bullets").ToDictionary(x => x.name, x => x);
    }

    // Update game time.
    // TODO: Remove? We can just use Time.time.
    private void FixedUpdate()
    {
        gameTime += Time.deltaTime;
    }

    // Adds score to the player
    // TODO: Deprecate
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
    
    // Returns the arena location of the current boss, if any. 
    public Vector3 getNextLevelLocation()
    {
        return getWarpLocation(BossController.instance.currentBoss.name);
    }

    // Returns the location associated with the given warp destination.
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
        return (int)difficulty;
    }

    // Returns a multiplier based on difficulty, for boss scaling.
    // Values are 1 for easy, 1.5 for medium, 2 for hard.
    public float getDifficultyModifier()
    {
        return getCurrentDifficultyInt() * 0.5f + 1;
    }

    // Returns a modifier based on how far into the game you are, for scaling.
    // Values start at 1 and increase by 0.5 per level beaten.
    public float getLevelModifier()
    {
        return (getCurrentLevel() - 1) * 0.5f + 1;
    }

    public Vector3 getStartAreaLocation()
    {
        return getWarpLocation("StartArea");
    }

    public Vector3 getLootRoomExitLocation()
    {
        return new Vector3(-54.25f, -53.5f, 0);
    }

    // Sends the player to the start area and runs a bunch of reset functionality.
    public void GoToStart()
    {
        PlayerController.instance.transform.position = getStartAreaLocation();
        roomType = RoomType.Start;
        setLevel(0);
        PerkManager.instance.ResetHeldPerks();
        PerkManager.instance.ResetPerks();
        PlayerController.instance.health.CurrentHealth = PlayerController.instance.health.baseMaxHP;
        MusicManager.instance?.FadeCurrentInto("Start Area Theme", 0.5f);
    }

    // Sends the player to the start whenever the main scene is loaded.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Loaded scene {scene.name} with mode {mode}");
        if (scene.name == "AlphaTest")
        {
            GoToStart();
        }
    }
    
    
}
