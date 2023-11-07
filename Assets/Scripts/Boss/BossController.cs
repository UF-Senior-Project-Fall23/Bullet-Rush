using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public static BossController instance;

    private GameObject currentBossPrefab = null;
    private GameObject player;

    public GameObject currentBoss = null;
    public Dictionary<string, GameObject> bossPrefabs;
    public String BossName;
    public GameObject portalPrefab;
    
    private void Awake()
    {
        Debug.Log("Awakened");
        if (instance == null)
        {
            Debug.Log("Generated new instance.");
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        bossPrefabs = Resources.LoadAll<GameObject>("Prefabs/Boss").ToDictionary(x => x.name, x => x);

        if (player == null)
            player = PlayerController.instance.gameObject;

        Debug.Log("Calling LoadBoss");
        LoadBoss(BossName);
    }

    public void SummonBoss(Vector3 pos, float health)
    {
        Debug.Log("Is this running?");

        currentBoss = Instantiate(currentBossPrefab, pos, Quaternion.identity);
        currentBoss.GetComponent<IHealth>().MaxHealth = health;
        StartCoroutine(currentBoss.GetComponent<Boss>().StartPhase());
    }

    public void LoadBoss(string bossName)
    {
        Debug.Log("Loading Boss: " + bossName);

        Vector3 bossPos = new Vector3(-75, 10, 0);
        
        currentBossPrefab = bossPrefabs[bossName];
        
        if (currentBossPrefab != null) SummonBoss(bossPos, 20f);
    }
    
    public void BossDie(Vector3 deathPos, Quaternion deathAng)
    {
        Debug.Log("Boss Died");
        if (currentBoss == null)
        {
            Instantiate(portalPrefab, deathPos, deathAng);
        }
        else
        {
            Instantiate(portalPrefab, currentBoss.transform.position, currentBoss.transform.rotation);
        }
        
        currentBossPrefab = null;
        currentBoss = null;
    }

    public void BossDie()
    {
        Debug.Log("Boss Died");
 
        Instantiate(portalPrefab, currentBoss.transform.position, currentBoss.transform.rotation);

        currentBossPrefab = null;
        currentBoss = null;
    }

    public void MinionDie()
    {
        Debug.Log("Minion Died");
        currentBossPrefab = null;
        currentBoss = null;
    }
}