using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

// Manages the loading, spawning, generation, and killing of bosses.
public class BossController : MonoBehaviour
{
    public static BossController instance;

    private GameObject currentBossPrefab = null;
    private GameObject player;
    private List<GameObject> indicators = new();

    public GameObject currentBoss = null;
    public Dictionary<string, GameObject> bossPrefabs;
    public GameObject portalPrefab;
    public GameObject indicatorPrefab;
    public GameObject CircleIndicatorPrefab;
    
    public GameObject inidcatorSmallPrefab;
    

    public List<string> runBosses; // The list of bosses for the currently generated run, in order.

    // Establishes singleton instance
    private void Awake()
    {
        Debug.Log("Awakened Boss Controller");
        if (instance == null)
        {
            Debug.Log("Generated new instance of Boss Controller");
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Generates boss prefab dictionary and gets player instance
    void Start()
    {
        bossPrefabs = Resources.LoadAll<GameObject>("Prefabs/Boss").ToDictionary(x => x.name, x => x);
        string[] actualBosses = {"Cordelia", "Blagthoroth", "Onyx"};
        foreach (var key in bossPrefabs.Keys.ToList())
        {
            if (!actualBosses.Contains(key))
            {
                bossPrefabs.Remove(key);
            }
        }

        if (player == null)
            player = PlayerController.instance.gameObject;
    }

    // Instantiates the boss and sets up its basic attributes
    public void SummonBoss(Vector3 pos, float health)
    {
        currentBoss = Instantiate(currentBossPrefab, pos, Quaternion.identity);

        health *= GameManager.instance.getDifficultyModifier();
        currentBoss.GetComponent<Damageable>().MaxHealth = health;
        currentBoss.GetComponent<Damageable>().CurrentHealth = health;
        
        BossHPBar.instance.Setup(currentBoss);
        StartCoroutine(currentBoss.GetComponent<Boss>().StartPhase());
    }

    // Sets up the boss internally and plays music
    public void LoadBoss(string bossName)
    {
        Debug.Log("Loading Boss: " + bossName);
        
        currentBossPrefab = bossPrefabs[bossName];
        BossHPBar.instance.SetFrame(bossName);
        
        FindObjectOfType<MusicManager>()?.LoadBossMusic(bossName);
    }
    
    // Kills the current boss
    public void ForceBossDie()
    {
        BossHPBar.instance.SetHPBarHidden(true);
        removeAllIndicators();
        Destroy(currentBoss);
        currentBossPrefab = null;
        currentBoss = null;
    }
    
    // Handles the death of the current boss and spawns the portal to the next area
    public void BossDie(Vector3 deathPos, Quaternion deathAng)
    {
        Debug.Log("Boss Died");
        removeAllIndicators();
        BossHPBar.instance.SetHPBarHidden(true);

        GameObject portal;
        
        if (currentBoss is null)
        {
            portal = Instantiate(portalPrefab, deathPos, deathAng);
        }
        else
        {
            portal = Instantiate(portalPrefab, currentBoss.transform.position, currentBoss.transform.rotation);
        }
        
        Debug.LogWarning($"Current Level is {GameManager.instance.getCurrentLevel()}");
        
        if (GameManager.instance.getCurrentLevel() == 3)
        {
            portal.GetComponent<Portal>().destination = "Start";
            Debug.Log("You won, generating start portal!");
        }
        else
        {
            portal.GetComponent<Portal>().destination = "Loot Room"; 
        }
        

        currentBossPrefab = null;
        currentBoss = null;
    }

    // Wrapper for BossDie(Transform,Rotation) which uses the current boss's transform by default.
    public void BossDie()
    {
        BossDie(currentBoss.transform.position, currentBoss.transform.rotation);
    }

    // Removes all boss indicators/telegraphs.
    public void removeAllIndicators()
    {
        foreach (var i in indicators)
            Destroy(i);
    }

    // Spawns an indicator at the specified position.
    public GameObject Indicate(Vector3 position, Quaternion rotation)
    {
        GameObject indicator = Instantiate(indicatorPrefab, position, rotation);
        indicators.Add(indicator);
        return indicator;
    }
    
    // Spawns a circle indicator at the specified position.
    public GameObject IndicateCircle(Vector3 position, Quaternion rotation)
    {
        GameObject indicator = Instantiate(CircleIndicatorPrefab, position, rotation);
        indicators.Add(indicator);
        return indicator;
    }

    //Returns the predicted position of the player
    //Strength is how strong the prediction is
    public Vector3 GetPredictedPos(float strength)
    {
        Vector3 playerPos = PlayerController.instance.transform.position;
        return (Vector2)playerPos + Vector2.ClampMagnitude(PlayerController.instance.GetComponent<Rigidbody2D>().velocity, strength);
    }

    // Removes the given indicator from the tracking list then destroys it.
    public void RemoveIndicator(GameObject indicator)
    {
        indicators.Remove(indicator);
        Destroy(indicator);
    }

    // Randomly picks 3 bosses, sets the level to 0, and resets the player's perks.
    public void GenerateRun()
    {
        runBosses = bossPrefabs.Keys.OrderBy(x => new Random().Next()).Take(3).ToList();
        GameManager.instance.setLevel(0);
        PerkManager.instance.ResetPerks();
        
        Debug.Log($"Generated new run. Bosses: {string.Join(", ", runBosses)}");
    }

    // Loads and summons the boss at the given level.
    public void StartBoss(int index)
    {
        string boss = runBosses[index];
        LoadBoss(boss);
        SummonBoss(currentBossPrefab.transform.position, GetBossHP(boss));
    }

    // Returns the base, unmodified maximum health for the given boss.
    private float GetBossHP(string name)
    {
        switch (name)
        {
            case "Cordelia":
                return 40;
            case "Blagthoroth":
                return 75;
            case "Onyx":
                return 60;
        }

        return 75;
    }
}
