using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cordelia : MonoBehaviour, Boss, IHealth
{
    public GameObject bulletPreFab;
    public GameObject puppetPreFab;
    public GameObject dimPreFab;
    GameObject dim;
    public int puppetCount = 4;
    public bool isPuppet = false;
    public int attackNum = 4;
    private Animator m_Animator;

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


    //IHealth Stuff
    public float MaxHP = 5.0f;
    private float m_CurrHP;
    bool m_invulnerable = false;
    public float MaxHealth { get => MaxHP; set => MaxHP = value; }
    public float CurrentHealth { get => m_CurrHP; set => m_CurrHP = value; }
    public bool Invulnerable { get => m_invulnerable; set => m_invulnerable = value; }

    void Start()
    {
        m_CurrHP = MaxHP;
        //StartCoroutine(SummonPuppets());
        cb = BossController.instance.currentBoss;
        playerPos = PlayerController.instance.gameObject.transform.position;
        m_Animator = GetComponent<Animator>();

        dim = Instantiate(dimPreFab, transform);
        spotlightActive = true;
        if (puppets.Count != 0)
        {
            for (int i = 0; i < puppetCount; i++)
            {
                puppets[i].GetComponent<puppetAttack>().spotlight = true;
                StartCoroutine(puppets[i].GetComponent<puppetAttack>().Spotlight());
            }
        }

    }

    public string[] Attacks { get; } =
    {
        "SpinDance", "KickDance", "StringDance", "SummonPuppets", "DetonatePuppets", "Rush", "Spotlight", "BladeFlourish", "PuppeteersGrasp"
    };

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag != "Clone")
        {
            if (GetComponent<BoxCollider2D>().IsTouching(collision.collider))
            {
                transform.position = new Vector3(-75, 25);
            }
            if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Wall" && rushMode)
            {
                rushDirection = Vector3.Reflect(rushDirection.normalized, collision.contacts[0].normal);
            }
        }
        

    }

    IEnumerator SpinDance()
    {
        float length = 0f;
        float endTime = 3f;
        float moveSpeed = .1f;
        bool rush = false;
        //playerPos = PlayerController.instance.gameObject.transform.position;
        while (length < endTime)
        {
            if (length > 2.3f)
            {
                rush = true;
            }
            var movementVector = (playerPos - transform.position).normalized * moveSpeed;
            transform.Translate(movementVector);
            if (puppets.Count != 0)
            {
                for (int i = 0; i < puppetCount; i++)
                {
                    if (puppets[i] != null)
                    {
                        StartCoroutine(puppets[i].GetComponent<puppetAttack>().SpinDance(rush));
                    }
                }
            }
            length += Time.deltaTime;
            yield return null;
        }


        PhaseChange();
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
        bool alive = false;
        if (puppets.Count == 0) 
        {
            Vector2 bossPos = new Vector2(transform.position.x, transform.position.y);
            float health = 7f;
            float radius = 5f;
            float playerAngle = Mathf.Atan2(playerPos.y, playerPos.x);
            for (int i = 0; i < puppetCount; i++)
            {
                GameObject puppet = Instantiate(puppetPreFab, bossPos + (UnityEngine.Random.insideUnitCircle.normalized* radius), Quaternion.identity);
                puppet.transform.Translate(new Vector3(0, puppet.transform.localScale.y / 2, 0));

                puppets.Add(puppet);
                puppet.GetComponent<IHealth>().MaxHealth = health;
                puppet.GetComponent<puppetAttack>().attackNum = attackNum;
            }
        }
        else
        {
            for (int i = 0; i < puppetCount; i++)
            {
                if (puppets[i] != null)
                {
                    alive = true;
                }

            }

            if (!alive)
            {
                puppets.Clear();
                Vector2 bossPos = new Vector2(transform.position.x, transform.position.y);
                float health = 7f;
                float radius = 5f;
                float playerAngle = Mathf.Atan2(playerPos.y, playerPos.x);
                for (int i = 0; i < puppetCount; i++)
                {
                    GameObject puppet = Instantiate(puppetPreFab, bossPos + (UnityEngine.Random.insideUnitCircle.normalized * radius), Quaternion.identity);
                    puppet.transform.Translate(new Vector3(0, puppet.transform.localScale.y / 2, 0));

                    puppets.Add(puppet);
                    puppet.GetComponent<IHealth>().MaxHealth = health;
                    puppet.GetComponent<puppetAttack>().attackNum = attackNum;
                }
            }
        }

        
        yield return null;
        PhaseChange();
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
        float length = 0f;
        float endTime = 6f;
        while (length < endTime)
        {
            rushMode = true;
            float speed = .1f;

            transform.Translate(rushDirection * speed);
            length += Time.deltaTime;
            if (puppets.Count != 0)
            {
                for (int i = 0; i < puppetCount; i++)
                {
                    if (puppets[i] != null)
                    {
                        StartCoroutine(puppets[i].GetComponent<puppetAttack>().Rush());
                    }
                }
            }
            yield return null;
        }
        rushMode = false;
        PhaseChange();
    }

    IEnumerator Spotlight()
    {
        if (!spotlightActive)
        {
            dim = Instantiate(dimPreFab, transform);
            spotlightActive = true;
            if (puppets.Count != 0)
            {
                for (int i = 0; i < puppetCount; i++)
                {
                    if(puppets[i] != null)
                    {
                        puppets[i].GetComponent<puppetAttack>().spotlight = true;
                        StartCoroutine(puppets[i].GetComponent<puppetAttack>().Spotlight());
                    }
                    
                }
            }
            DimLights.instance.Appear();
        }
        else if(spotlightActive)
        {
            spotlightActive = false;
            if (puppets.Count != 0)
            {
                for (int i = 0; i < puppetCount; i++)
                {
                    if (puppets[i] != null)
                    {
                        puppets[i].GetComponent<puppetAttack>().spotlight = false;
                    }
                }
            }
            DimLights.instance.TurnOff();
        }
        yield return null;
    }
    
    IEnumerator BladeFlourish()
    {
        float length = 0f;
        float endTime = 6f;
        int followPattern = 1;
        if (MathF.Floor(length) % 2 == 0)
        {
            //followPattern = UnityEngine.Random.Range(1, 4);
        }
        while (length < endTime)
        {
            if (puppets.Count != 0)
            {
                for (int i = 0; i < puppetCount; i++)
                {
                    
                    if (puppets[i] != null)
                    {
                        followPattern = UnityEngine.Random.Range(1, 4);
                        StartCoroutine(puppets[i].GetComponent<puppetAttack>().BladeFlourish(followPattern));
                    }
                }
            }

            playerPos = PlayerController.instance.transform.position - transform.position;
            //Get the angle from the position
            float playerAngle = Mathf.Atan2(playerPos.y, playerPos.x);
            float speed = 2f;
            var step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, PlayerController.instance.transform.position, step * 2f);


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
                yield return null;
            }
            length += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        
        PhaseChange();
    }

    IEnumerator PuppeteersGrasp()
    {
        yield return null;
    }
    
    private void setPuppetAttack()
    {
        if (puppets.Count != 0)
        {
            for (int i = 0; i < puppetCount; i++)
            {
                if (puppets[i] != null)
                {
                    puppets[i].GetComponent<puppetAttack>().attackNum = attackNum;
                    puppets[i].GetComponent<puppetAttack>().reset();
                }
               
            }
        }
    }

    public void PhaseChange()
    {
        
        {
            attackNum = UnityEngine.Random.Range(1, 5);
            //attackNum = 4;
            setPuppetAttack();
            switch (attackNum)
            {
                case 1:
                    playerPos = PlayerController.instance.gameObject.transform.position;
                    StartCoroutine(SpinDance());
                    break;
                case 2:
                    StartCoroutine(Rush());
                    break;
                case 3:
                    StartCoroutine(BladeFlourish());
                    break;
                case 4:
                    StartCoroutine(SummonPuppets()); 
                    break;
                case 5:
                    StartCoroutine(DetonatePuppets()); 
                    break;
                case 6:
                    StartCoroutine(KickDance());
                    break;
                case 7:
                    StartCoroutine(StringDance());
                    break;
                case 8:
                    StartCoroutine(PuppeteersGrasp()); 
                    break;
                default:
                    print("Incorrect Number.");
                    break;
            }
        }
    }

    public IEnumerator StartPhase()
    {
        Invulnerable = true;
        yield return new WaitForSeconds(1f);
        Invulnerable = false;

        PhaseChange();
    }

    void Update()
    {
        
        globalTime += Time.deltaTime;
        if (spotlightActive)
        {
            dim.transform.position = transform.position;
        }
        float second = MathF.Floor(globalTime) % interval;
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

    public void Die()
    {
        if (!isPuppet)
        {
            // kills any remaining puppets once Cordelia dies
            for (int i = 0; i < puppets.Count; i++)
            {
                if (puppets[i] != null)
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
        DimLights.instance.TurnOff();
        Destroy(gameObject);
    }

}