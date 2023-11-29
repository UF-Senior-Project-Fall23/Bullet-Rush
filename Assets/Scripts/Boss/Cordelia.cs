using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cordelia : Damageable, Boss
{
    public GameObject bulletPreFab;
    public GameObject puppetPreFab;
    public GameObject dimPreFab;
    GameObject dim;
    public int puppetCount = 4;
    public bool isPuppet = false;
    public int attackNum = 4;

    double attackSpeedModifier = 1;
    float globalTime = 0;
    int interval = 6;

    bool phaseShifted = false;
    bool attackReady = false;
    bool lights = false;
    bool rushMode = false;
    bool spotlightActive = false;
    bool timeIndicator = true;
    bool spinSet = true;
    GameObject cb;
    
    Vector3 playerPos;

    
    int followPattern = 1;
    int rotationSpeed = 3;
    int rotationSize = 8;

    private float bulletTime = 0f;
    private float bulletWait = .2f;
    private Vector3 rushDirection = Vector3.up;
    
    private List<GameObject> puppets = new List<GameObject>();

    void Start()
    {
        StartCoroutine(SummonPuppets());
        cb = BossController.instance.currentBoss;
        playerPos = PlayerController.instance.gameObject.transform.position;
    }

    public string[] Attacks { get; } =
    {
        "SpinDance", "KickDance", "StringDance", "SummonPuppets", "DetonatePuppets", "Rush", "Spotlight", "BladeFlourish", "PuppeteersGrasp"
    };

    public void setPuppetAttack(int attack)
    {
        attackNum = attack;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision with " + collision.gameObject.name);
        //Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Wall" && rushMode)
        {
            rushDirection = Vector3.Reflect(rushDirection.normalized, collision.contacts[0].normal);
        }
    }

    // Sets the dance to Spin.
    IEnumerator SpinDance()
    {
        float speed = 2f;
        var step = speed * Time.deltaTime;
        bool follow = false;
        

        // runs on the global timer, so every 3 seconds Cordelia will rush the player while spining.

        if (MathF.Floor(globalTime) % 3 == 0)
        {
            // rush once halfway in the rotation.
            transform.position = Vector3.MoveTowards(transform.position, playerPos, step * 15f);
            follow = true;
        }
        //could add a delay in the update so Cordelia is not exactly circuling the player and has a chance to touch them.
        else
        {
            if (isPuppet)
            {
                if (spinSet)
                {
                    rotationSpeed = UnityEngine.Random.Range(1, 5);
                    rotationSize = UnityEngine.Random.Range(6, 13);
                    spinSet = false;
                }
                playerPos = PlayerController.instance.gameObject.transform.position;

                float x = Mathf.Cos(Time.time * rotationSpeed) * rotationSize;
                float y = Mathf.Sin(Time.time * rotationSpeed) * rotationSize;
                transform.position = new Vector3(x, y);
                transform.position = transform.position + playerPos;
                follow = false;
            }
            else
            {
                rotationSpeed = 3;
                rotationSize = 8;
                playerPos = PlayerController.instance.gameObject.transform.position;
                    
                float x = Mathf.Cos(Time.time * rotationSpeed) * rotationSize;
                float y = Mathf.Sin(Time.time * rotationSpeed) * rotationSize;
                transform.position = new Vector3(x, y);
                transform.position = transform.position + playerPos;
                follow = false;
            }
                
            
        }
        
        
        yield return null;
    }

    IEnumerator KickDance()
    {
        yield return null;
    }

    IEnumerator StringDance()
    {
        
        yield return null;
    }

    IEnumerator SummonPuppets()
    {
        if (!isPuppet && puppets.Count == 0) 
        {
            // change to be in specific locations or circuling Cordelia
            Vector3 bossPos = new Vector3(-75, 10, 0);
            float health = 7f;
            float radius = 5f;
            for (int i = 0; i < puppetCount; i++)
            {
                GameObject puppet = Instantiate(puppetPreFab, bossPos, Quaternion.identity);
                puppets.Add(puppet);
                puppet.GetComponent<Damageable>().MaxHealth = health;
                StartCoroutine(puppet.GetComponent<Boss>().StartPhase());
                bossPos = new Vector3(-74 + i, 11 + i, 0);
                puppet.GetComponent<puppetAttack>().attack = attackNum;
            }
        }
        yield return null;
    }

    IEnumerator DetonatePuppets()
    {
        if (!isPuppet)
        {
            for (int i = 0; i < puppets.Count; i++)
            {
                if (puppets[i] != null)
                {
                    // explode animation
                    // check if player is in range and do damage if they are.
                    int damage = (int)MathF.Floor(puppets[i].GetComponent<Damageable>().MaxHealth);
                    puppets[i].GetComponent<Damageable>().takeDamage(damage);
                }
            }

            BossController.instance.MinionDie();
        }
        else
        {
            // or just put on a timer like spotlight
            //cordelia uses other attack or stands back.
        }
        yield return null;
    }

    IEnumerator Rush()
    {
        rushMode = true;
        float speed = .1f;

        transform.Translate(rushDirection * speed);
        yield return null;
    }

    IEnumerator Spotlight()
    {
        if (!isPuppet && !spotlightActive)
        {
            dim = Instantiate(dimPreFab, transform);
            spotlightActive = true;
            DimLights.instance.Appear();
        }
        else if(!isPuppet && spotlightActive)
        {
            spotlightActive = false;
            DimLights.instance.TurnOff();
        }
        yield return null;
    }
    
    IEnumerator BladeFlourish()
    {
        playerPos = PlayerController.instance.transform.position - transform.position;
        //Get the angle from the position
        float playerAngle = Mathf.Atan2(playerPos.y, playerPos.x);
        if (MathF.Floor(globalTime) % 2 == 0)
        {
            followPattern = UnityEngine.Random.Range(1, 4);
        }
        if (!isPuppet)
        {
            float speed = 2f;
            var step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, PlayerController.instance.transform.position, step * 2f);

        }

        else
        {
            
            if(followPattern == 1)
            {
                float speed = UnityEngine.Random.Range(0.0f, 3.0f);
                var step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, PlayerController.instance.transform.position, step * 2f);
            }
            else if(followPattern == 2)
            {
                float speed = UnityEngine.Random.Range(0.0f, 5.0f);
                var step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, PlayerController.instance.transform.position, step * 3f);
            }
            else if(followPattern == 3)
            {
                float speed = UnityEngine.Random.Range(0.0f, 3.0f);
                var step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, PlayerController.instance.transform.position, step * -2f);
            }
        }
        if (bulletTime <= Time.time && MathF.Floor(globalTime) % 2 == 0)
        {

            for (int i = 0; i < 5; i++)
            {
                var r2 = Quaternion.Euler(0, 0, playerAngle * Mathf.Rad2Deg + 90);

                GameObject bullet = Instantiate(bulletPreFab, new Vector3(
                    transform.position.x + (transform.localScale.x / 5f * Mathf.Cos(playerAngle)),
                    transform.position.y + (transform.localScale.y / 5f * Mathf.Sin(playerAngle)), 1), r2);

                bullet.transform.Rotate(0, 0, 290 - (i * 10));
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb.AddForce(bullet.transform.right * 13f, ForceMode2D.Impulse);
            }
            bulletTime = Time.time + bulletWait;
        }



        yield return null;
    }

    IEnumerator PuppeteersGrasp()
    {
        yield return null;
    }
    

    void Boss.PhaseChange()
    {
        phaseShifted = true;
        attackSpeedModifier = 1.5;
    }

    IEnumerator Boss.StartPhase()
    {
        //Debug.Log(this.attackNum);
        //StartCoroutine(BladeFlourish());
        //StartCoroutine(Rush());
        yield return null;
    }

    void Update()
    {
        
        globalTime += Time.deltaTime;

        float second = MathF.Floor(globalTime) % interval;

        // sets Cordelia's attack if the time has counted down from 6
        // lasts a second
        //Debug.Log(second);
        //Debug.Log(attackNum);
        if (second == 0 && !isPuppet)
        {
            if (!isPuppet)
            {
                // put an action/idle here for the 1 second break
                attackNum = UnityEngine.Random.Range(1, 5);
            }
            spinSet = true;
        }

        // sets the puppets attack
        if (!isPuppet)
        {
            for (int i = 0; i < puppets.Count; i++)
            {
                // set up the method for interface- create new minionBoss interface?
                if (puppets[i] != null)
                {
                    puppets[i].GetComponent<puppetAttack>().attack = attackNum;
                }
            }
        }
        if (isPuppet)
        {
            attackNum = GetComponent<puppetAttack>().attack;
        }
        if (spotlightActive)
        {
            dim.transform.position = transform.position;
        }
        //Debug.Log(attackNum);
        //runs the coroutine's while the counter is not 0
        // coruntines run as if in update
        if (second != 0)
        {
            //"SpinDance", "KickDance", "StringDance", "SummonPuppets", "DetonatePuppets", "Rush", "Spotlight", "BladeFlourish", "PuppeteersGrasp"
            //attackNum = 3;

            switch (attackNum)
            {
                case 1:
                    interval = 12;
                    StartCoroutine(SpinDance());
                    break;
                case 9:
                    interval = 6;
                    StartCoroutine(Rush());
                    break;
                case 3:
                    interval = 18;
                    StartCoroutine(BladeFlourish());
                    break;
                case 4:
                    interval = 6;
                    StartCoroutine(SummonPuppets()); 
                    break;
                case 5:
                    interval = 3;
                    StartCoroutine(DetonatePuppets()); 
                    break;
                case 6:
                    interval = 3;
                    StartCoroutine(KickDance());
                    break;
                case 7:
                    interval = 3;
                    StartCoroutine(StringDance());
                    break;
                case 8:
                    interval = 3;
                    StartCoroutine(PuppeteersGrasp()); 
                    break;
                default:
                    print("Incorrect Number.");
                    break;
            }
        }
        if (MathF.Floor(globalTime) % 15 == 0 && timeIndicator)
        {
            StartCoroutine(Spotlight());
            timeIndicator = false;
        }
        else if(MathF.Floor(globalTime) % 15 != 0)
        {
            timeIndicator = true;
        }
        }

    //Run this so the boss controller wont call bosslogic anymore
    private void OnDestroy()
    {
        if (!isPuppet)
        {
            // kills any remaining puppets once Cordelia dies
            for (int i = 0; i < puppets.Count; i++)
            {
                if(puppets[i] != null) 
                {
                    int damage = (int)MathF.Floor(puppets[i].GetComponent<Damageable>().MaxHealth);
                    puppets[i].GetComponent<Damageable>().takeDamage(damage);
                }
            }
            
            BossController.instance.BossDie(transform.position, transform.rotation);
        }
        else
        {
            BossController.instance.MinionDie();
        }
        
    }

    public override void Die()
    {
        
    }
    

}