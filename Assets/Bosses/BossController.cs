using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{

    //public const Boss CORDELIA = new Cordelia();
    
    private Boss currentBoss = null;

    public GameObject cordeliaPrefab;
    
    public GameObject bulletPrefab;
    public Transform firepoint;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentBoss?.BossLogic();
    }

    // Calls a function for the given boss with the name provided.
    // Useful for storing attacks in a list and properly naming the attack functions something other than "Attack1, Attack2", etc.
    public void CallAttack(Boss boss, string attack)
    {
            var bossType = boss.GetType();
            var attackFunction = bossType.GetMethod(attack);
            attackFunction?.Invoke(boss, null);
    }

    public void SummonBoss(Boss boss)
    {
        
    }

    public void LoadBoss(string name)
    {
        if (name == "Cordelia")
        {
            SummonBoss(Instantiate(cordeliaPrefab,  transform));
        }
    }
}
