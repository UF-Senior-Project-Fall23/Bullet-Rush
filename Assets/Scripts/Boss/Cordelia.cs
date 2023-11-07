using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cordelia : MonoBehaviour, Boss
{
    public GameObject bulletPreFab;
    public GameObject puppetPreFab;
    public GameObject dimPreFab;
    GameObject dim;
    public int puppetCount = 4;
    public bool isPuppet = false;

    double attackSpeedModifier = 1;
    float globalTime = 0;

    bool phaseShifted = false;
    bool attackReady = false;
    bool lights = false;
    bool rushMode = false;
    bool spotlightActive = false;
    
    GameObject cb;
    
    Vector3 playerPos;

    int attackNum = 4;

    private float bulletTime = 0f;
    private float bulletWait = .2f;
    private Vector3 rushDirection = Vector3.up;
    
    private List<GameObject> puppets = new List<GameObject>();

    void Start()
    {
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
        Debug.Log(collision.gameObject.tag);
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
        // runs on the global timer, so every 3 seconds Cordelia will rush the player while spining.
        if (MathF.Floor(globalTime) % 3 == 0)
        {
            // rush once halfway in the rotation.
            transform.position = Vector3.MoveTowards(transform.position, playerPos, step * 15f);
        }
        //could add a delay in the update so Cordelia is not exactly circuling the player and has a chance to touch them.
        else
        {
            playerPos = PlayerController.instance.gameObject.transform.position;
            int rotationSpeed = 3;
            int rotationSize = 7;
            float x = Mathf.Cos(Time.time * rotationSpeed) * rotationSize;
            float y = Mathf.Sin(Time.time * rotationSpeed) * rotationSize;
            transform.position = new Vector3(x, y);
            transform.position = transform.position + playerPos;
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
            float health = 20f;
            for (int i = 0; i < puppetCount; i++)
            {
                GameObject puppet = Instantiate(puppetPreFab, bossPos, Quaternion.identity);
                puppets.Add(puppet);
                puppet.GetComponent<IHealth>().MaxHealth = health;
                StartCoroutine(puppet.GetComponent<Boss>().StartPhase());
                bossPos = new Vector3(-74 + i, 11 + i, 0);
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
                    int damage = (int)MathF.Floor(puppets[i].GetComponent<IHealth>().MaxHealth);
                    puppets[i].GetComponent<IHealth>().takeDamage(damage);
                }
            }

            BossController.instance.BossDie(transform.position, transform.rotation);
        }
        else
        {
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
        Debug.Log("about to spotlight");
        if (!isPuppet && !spotlightActive)
        {
            dim = Instantiate(dimPreFab, transform);
            spotlightActive = true;
            DimLights.instance.Appear();
        }
        yield return null;
    }

    IEnumerator BladeFlourish()
    {
        playerPos = PlayerController.instance.transform.position - transform.position;
        //Get the angle from the position
        float playerAngle = Mathf.Atan2(playerPos.y, playerPos.x);
        //Fire a bullet at the player based on its position
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
        StartCoroutine(BladeFlourish());
        yield return null;
    }

    void Update()
    {
        
        globalTime += Time.deltaTime;

        //can move outside of update and adjust per attack.
        int interval = 6;

        // six second timer
        float second = MathF.Floor(globalTime) % interval;

        // sets Cordelia's attack if the time has counted down from 6
        // lasts a second
        if (second == 0 && !isPuppet)
        {
            // put an action/idle here for the 1 second break
            attackNum = UnityEngine.Random.Range(1, 10);
        }

        // sets the puppets attack
        if (!isPuppet)
        {
            for (int i = 0; i < puppets.Count; i++)
            {
                // set up the method for interface- create new minionBoss interface?
                //puppets[i].GetComponent<Boss>().setPuppetAttack(attackNum);
            }
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
            attackNum = 6;

            switch (attackNum)
            {
                case 1:
                    StartCoroutine(SpinDance());
                    break;
                case 2:
                    StartCoroutine(KickDance()); 
                    break;
                case 3:
                    StartCoroutine(StringDance()); 
                    break;
                case 4:
                    StartCoroutine(SummonPuppets()); 
                    break;
                case 5:
                    StartCoroutine(DetonatePuppets()); 
                    break;
                case 6:
                    StartCoroutine(Rush()); 
                    break;
                case 7:
                    StartCoroutine(Spotlight()); 
                    break;
                case 8:
                    StartCoroutine(BladeFlourish()); 
                    break;
                case 9:
                    StartCoroutine(PuppeteersGrasp()); 
                    break;
                default:
                    print("Incorrect Number.");
                    break;
            }
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
                    int damage = (int)MathF.Floor(puppets[i].GetComponent<IHealth>().MaxHealth);
                    puppets[i].GetComponent<IHealth>().takeDamage(damage);
                }
            }
            
            BossController.instance.BossDie(transform.position, transform.rotation);
        }
        else
        {
            BossController.instance.MinionDie();
        }
        
    }

}