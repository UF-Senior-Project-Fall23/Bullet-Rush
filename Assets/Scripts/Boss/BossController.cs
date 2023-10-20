using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public static BossController instance;

    private Boss currentBossLogic = null;
    private GameObject currentBoss = null;
    private GameObject cb = null;
    public GameObject player;
    public GameObject cordeliaPrefab;
    
    

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
        Debug.Log("Calling LoadBoss");
        LoadBoss("Cordelia");
    }

    // Update is called once per frame
    void Update()
    {
        currentBossLogic?.BossLogic(Time.deltaTime, cb, player.transform.position);
    }

    // Calls a function for the given boss with the name provided.
    // Useful for storing attacks in a list and properly naming the attack functions something other than "Attack1, Attack2", etc.
    public void CallAttack(Boss boss, string attack)
    {
        var bossType = boss.GetType();
        var attackFunction = bossType.GetMethod(attack);
        attackFunction?.Invoke(boss, null);
    }

    public void SummonBoss(Vector3 pos)
    {
        Debug.Log("Is this running?");

        currentBoss.transform.position = pos;
        cb = Instantiate(currentBoss,  currentBoss.transform);
    }

    public void LoadBoss(string bossName)
    {
        Debug.Log("Loading Boss: " + bossName);

        Vector3 bossPos = new Vector3(-75, 10, 0);
        
        if (bossName == "Cordelia")
        {
            currentBoss = cordeliaPrefab;
            currentBossLogic = cordeliaPrefab.GetComponent<Cordelia>();
        }
        
        if (currentBoss != null) SummonBoss(bossPos);
    }
}