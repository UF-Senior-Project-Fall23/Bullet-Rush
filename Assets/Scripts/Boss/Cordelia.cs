using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cordelia : Damageable, Boss
{
    public GameObject bulletPreFab;
    public GameObject puppetPreFab;
    public GameObject dimPreFab;
    public GameObject voidPreFab;
    public GameObject stringPreFab;
    GameObject dim;
    int puppetCount = 2;
    public int attackNum = 4;
    private Animator m_Animator;
    private float m_DifficultyModifier;
    private float m_LevelModifier;

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
    bool puppetSpawn = false;
    bool summoningDone = true;
    GameObject cb;

    Vector3 playerPos;


    int followPattern = 1;
    int rotationSpeed = 3;
    int rotationSize = 8;
    int puppetRespawnTime = 4;
    int diffAttack = 5;
    private float bulletTime = 0f;
    private float bulletWait = .3f;
    private Vector3 rushDirection = Vector3.up;

    private List<GameObject> puppets = new();
    private List<GameObject> gloves = new ();
    private static List<Vector3> voidList = new(127);

    static Cordelia()
    {
        // Terrible, horrible, no good, very bad code. TODO: Make this dynamically pick a location in Cordelia's arena mesh
        void addVoid(float x, float y)
            {
                voidList.Add(new Vector3(x,y,0f));
            }
        addVoid(-82.5f, -0.7f); addVoid(-74.7f, -3.5f); addVoid(-67.1f, -3.0f); addVoid(-73.2f, 1.8f); addVoid(-81.9f, 4.9f); 
        addVoid(-88.6f, 5.7f); addVoid(-74.7f, 8.9f); addVoid(-65.8f, 8.4f); addVoid(-67.9f, 4.1f); addVoid(-61.4f, 2.0f); 
        addVoid(-62.2f, -2.0f); addVoid(-65.4f, -6.0f); addVoid(-88.3f, 0.1f); addVoid(-93.0f, 1.4f); addVoid(-98.1f, 4.2f); 
        addVoid(-91.9f, 8.7f); addVoid(-84.5f, 8.5f); addVoid(-79.0f, 1.8f); addVoid(-85.4f, 2.6f); addVoid(-75.8f, 5.3f); 
        addVoid(-79.9f, -3.0f); addVoid(-79.9f, -3.0f); addVoid(-101.5f, 7.8f); addVoid(-96.9f, 9.6f); addVoid(-105.4f, 10.2f); 
        addVoid(-101.3f, 12.3f); addVoid(-109.0f, 13.3f); addVoid(-106.7f, 16.3f); addVoid(-101.3f, 15.7f); addVoid(-103.0f, 18.9f); 
        addVoid(-95.8f, 12.8f); addVoid(-89.8f, 11.4f); addVoid(-79.6f, 7.6f); addVoid(-67.5f, 1.0f); addVoid(-70.7f, -5.4f); 
        addVoid(-60.2f, -5.8f); addVoid(-55.8f, -4.1f); addVoid(-56.6f, 0.5f); addVoid(-71.1f, 6.5f); addVoid(-95.0f, 15.8f); 
        addVoid(-89.5f, 14.8f); addVoid(-84.9f, 11.7f); addVoid(-79.4f, 10.9f); addVoid(-74.0f, 11.9f); addVoid(-70.2f, 9.7f); 
        addVoid(-63.0f, 5.4f); addVoid(-57.1f, 4.2f); addVoid(-52.3f, -1.6f); addVoid(-51.8f, 2.1f); addVoid(-46.7f, 3.9f); 
        addVoid(-52.1f, 5.5f); addVoid(-57.6f, 7.4f); addVoid(-61.5f, 10.3f); addVoid(-66.0f, 12.0f); addVoid(-78.3f, 13.6f); 
        addVoid(-83.6f, 14.3f); addVoid(-90.8f, 18.2f); addVoid(-96.8f, 19.1f); addVoid(-99.0f, 22.4f); addVoid(-84.6f, 17.5f); 
        addVoid(-78.6f, 17.0f); addVoid(-73.4f, 15.6f); addVoid(-69.9f, 13.6f); addVoid(-67.9f, 17.0f); addVoid(-63.7f, 15.2f); 
        addVoid(-92.7f, 21.3f); addVoid(-94.7f, 24.8f); addVoid(-91.5f, 27.3f); addVoid(-88.9f, 24.4f); addVoid(-86.8f, 20.6f); 
        addVoid(-81.1f, 20.1f); addVoid(-83.9f, 23.1f); addVoid(-85.4f, 26.9f); addVoid(-80.1f, 25.3f); addVoid(-78.8f, 28.1f); 
        addVoid(-76.9f, 22.5f); addVoid(-75.0f, 19.2f); addVoid(-69.7f, 20.0f); addVoid(-71.5f, 23.0f); addVoid(-74.4f, 25.8f); 
        addVoid(-70.1f, 27.4f); addVoid(-66.4f, 23.8f); addVoid(-64.4f, 19.0f); addVoid(-58.7f, 13.1f); addVoid(-55.1f, 9.9f); 
        addVoid(-49.1f, 8.2f); addVoid(-44.8f, 6.9f); addVoid(-50.2f, 11.7f); addVoid(-54.2f, 14.3f); addVoid(-59.1f, 16.6f); 
        addVoid(-62.4f, 21.7f); addVoid(-61.3f, 25.1f); addVoid(-58.7f, 19.4f); addVoid(-53.4f, 17.2f); addVoid(-48.9f, 14.9f); 
        addVoid(-45.2f, 10.2f); addVoid(-56.9f, 22.6f); addVoid(-52.7f, 20.0f); addVoid(-48.2f, 17.6f); addVoid(-45.9f, 12.8f); 
        addVoid(-51.2f, 22.4f); addVoid(-56.1f, 25.3f); addVoid(-65.5f, 26.4f); addVoid(-93.3f, 4.3f); addVoid(-96.1f, 6.7f); 
        addVoid(-76.0f, -0.8f); addVoid(-71.5f, -1.5f); 
    }
    
    void Start()
    {
        m_DifficultyModifier = GameManager.instance.getDifficultyModifier();
        m_LevelModifier = GameManager.instance.getLevelModifier();
        m_Animator = GetComponent<Animator>();
        Debug.Log(m_DifficultyModifier);
        Debug.Log(m_LevelModifier);
        if(m_DifficultyModifier == 1.5)
        {
            diffAttack = 7;
            puppetCount = 3;
        }
        else if(m_DifficultyModifier == 2)
        {
            diffAttack = 8;
            puppetCount = 5;
        }
        Debug.Log(m_DifficultyModifier);
        Debug.Log(m_LevelModifier);
        //StartCoroutine(SummonPuppets());

        cb = BossController.instance.currentBoss;
        playerPos = PlayerController.instance.gameObject.transform.position;
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
        if (collision.gameObject.tag != "Clone")
        {
            if (GetComponent<BoxCollider2D>().IsTouching(collision.collider) && collision.gameObject.tag == "Wall")
            {
                transform.position = new Vector3(-75, 25);
            }
            if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Wall" && rushMode)
            {
                rushDirection = Vector3.Reflect(rushDirection.normalized, collision.contacts[0].normal);
            }
            if(collision.gameObject.tag == "Player")
            {
                PlayerController.instance.GetComponent<Damageable>()?.takeDamage(1);
            }
        }


    }

    // Sets the dance to Spin.
    IEnumerator SpinDance()
    {
        Debug.Log("SpinDance");
        yield return new WaitWhile(() => m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        m_Animator.SetTrigger("Rush");
        yield return new WaitForSeconds(1f);
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
        Debug.Log("KickDance");
        yield return null;
    }

    IEnumerator SummonPuppets()
    {
        Debug.Log("SummonPuppets");
        puppetRespawnTime = 4;
        puppetSpawn = true;
        bool alive = false;
        if (puppets.Count == 0)
        {
            Vector2 bossPos = new Vector2(transform.position.x, transform.position.y);
            float health = 7f;
            float radius = 5f;
            float playerAngle = Mathf.Atan2(playerPos.y, playerPos.x);
            for (int i = 0; i < puppetCount; i++)
            {
                GameObject puppet = Instantiate(puppetPreFab, bossPos + (UnityEngine.Random.insideUnitCircle.normalized * radius), Quaternion.identity);
                puppet.transform.Translate(new Vector3(0, puppet.transform.localScale.y / 2, 0));

                puppets.Add(puppet);
                puppet.GetComponent<Damageable>().MaxHealth = health;
                puppet.GetComponent<Damageable>().CurrentHealth = health;
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
                    puppet.GetComponent<Damageable>().MaxHealth = health;
                    puppet.GetComponent<Damageable>().CurrentHealth = health;
                }
            }
        }

        yield return new WaitForSeconds(.8f);
        PhaseChange();
    }

    IEnumerator DetonatePuppets()
    {
        Debug.Log("DetonatePuppets");
        playerPos = PlayerController.instance.gameObject.transform.position;
        for (int i = 0; i < puppets.Count; i++)
        {
            if (puppets[i] != null)
            {

                StartCoroutine(puppets[i].GetComponent<puppetAttack>().DetonatePuppets());

                float distance = Vector3.Distance(puppets[i].transform.position, playerPos);
                //Debug.Log(distance);
                if(distance < 3f)
                {
                    
                    PlayerController.instance.GetComponent<Damageable>()?.takeDamage(4);
                }
            }
        }

        yield return new WaitForSeconds(1.5f);
        PhaseChange();
        
    }

    IEnumerator Rush()
    {
        Debug.Log("Rush");
        float length = 0f;
        float endTime = 6f;
        float bulletLength = 0f;
        float bulletFreq = .5f;
        yield return new WaitWhile(() => m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        m_Animator.SetTrigger("Spin");
        yield return new WaitForSeconds(1f);
        while (length < endTime)
        {
            
            rushMode = true;
            float speed = .04f;

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

            playerPos = PlayerController.instance.transform.position - transform.position;
            //Get the angle from the position
            float playerAngle = Mathf.Atan2(playerPos.y, playerPos.x);
            float s = 2f;
            var step = s * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, PlayerController.instance.transform.position, step * 2f);


            if (bulletLength <= Time.time && MathF.Floor(globalTime) % 2 == 0)
            {

                for (int i = 0; i < 7; i++)
                {
                    var r2 = Quaternion.Euler(0, 0, 0);// playerAngle * Mathf.Rad2Deg + 90);

                    GameObject bullet = Instantiate(bulletPreFab, new Vector3(
                        transform.position.x + (transform.localScale.x / 5f * Mathf.Cos(playerAngle)),
                        transform.position.y + (transform.localScale.y / 5f * Mathf.Sin(playerAngle)), 1), r2);
                    //Debug.Log(bullet.transform.position);
                    bullet.transform.Rotate(0, 0, 290 - (i * 150));
                    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                    rb.AddForce(bullet.transform.right * .001f, ForceMode2D.Impulse);
                }
                bulletLength = Time.time + bulletFreq;
                yield return null;
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
                    if (puppets[i] != null)
                    {
                        puppets[i].GetComponent<puppetAttack>().spotlight = true;
                        StartCoroutine(puppets[i].GetComponent<puppetAttack>().Spotlight());
                    }

                }
            }
            DimLights.instance.Appear();
        }
        else if (spotlightActive)
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
        Debug.Log("BladeFlourish");
        // make puppets faster to be ahead of Cordelia
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
                    //Debug.Log(bullet.transform.position);
                    bullet.transform.Rotate(0, 0, 290 - (i * 10));
                    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                    rb.AddForce(bullet.transform.right * .00125f, ForceMode2D.Impulse);
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
        summoningDone = false;
        Debug.Log("PuppeteersGrasp");
        yield return new WaitWhile(() => m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        m_Animator.SetTrigger("HandsExit");
        playerPos = PlayerController.instance.transform.position;
        voidList.Sort((v1, v2) => Vector3.Distance(v2, playerPos).CompareTo(Vector3.Distance(v1, playerPos)));
        int max = 80;
        int voidAmount = 28*(int)m_LevelModifier; // actual is voidAmount/crowding
        if(voidAmount > max)
        {
            voidAmount = max;
        }
        voidAmount = 80;
        int crowding = 4;
        float rate = 1f;
        int spreadCount = 0;
        bool nextPhase = false;

        //Get the angle from the position
        float playerAngle = Mathf.Atan2(playerPos.y, playerPos.x);
        float s = 2f;
        var step = s * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, PlayerController.instance.transform.position, step * 2f);

        for (int i = 0; i < 7; i++)
        {
            var r2 = Quaternion.Euler(0, 0, 0);// playerAngle * Mathf.Rad2Deg + 90);

            GameObject bullet = Instantiate(bulletPreFab, new Vector3(
                transform.position.x + (transform.localScale.x / 5f * Mathf.Cos(playerAngle)),
                transform.position.y + (transform.localScale.y / 5f * Mathf.Sin(playerAngle)), 1), r2);
            //Debug.Log(bullet.transform.position);
            bullet.transform.Rotate(0, 0, 290 - (i * 150));
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(bullet.transform.right * .0007f, ForceMode2D.Impulse);
        }
        for (int i = voidList.Count-1; i > voidList.Count - voidAmount; i-=crowding)
        {
            //Debug.Log(i);
            //Debug.Log(i - voidAmount);

            // keeps summoning while other attacks can happen
            if(spreadCount > 4 && !nextPhase)
            {
                nextPhase = true;
                PhaseChange();
            }
            // finds the furthest and closest points
            int j = (int)Math.Pow(-1, spreadCount);
            if(j < 0)
            {
                j = voidList.Count - i;
            }
            else
            {
                j = i;
            }
            GameObject v = Instantiate(voidPreFab, voidList[j], Quaternion.identity);
            gloves.Add(v);
            spreadCount++;
            yield return new WaitForSeconds(rate);
        }
        summoningDone = true;
        Debug.Log("END");
    }

    private void setPuppetAttack()
    {
        if (puppets.Count != 0)
        {
            for (int i = 0; i < puppetCount; i++)
            {
                if (puppets[i] != null)
                {
                    puppets[i].GetComponent<puppetAttack>().reset();
                }

            }
        }
    }

    IEnumerator PhaseWait()
    {
        m_Animator.SetTrigger("Bounce");
        yield return new WaitForSeconds(.5f);
    }
    public void PhaseChange()
    {
        Debug.Log("start of phase");
        StartCoroutine(PhaseWait());
        Debug.Log("after phase wait");
        int temp = attackNum;
        // could make it so that only some attacks can't be twice in a row
        int diffAttackMod = 0;
        if (!summoningDone)
        {
            diffAttackMod = 1;
        }
        while (temp == attackNum) 
        {

            attackNum = UnityEngine.Random.Range(1, diffAttack - diffAttackMod);
            Debug.Log(attackNum);
        }
        int puppetsLeft = puppetCount;
        
        if (puppetSpawn)
        {
            foreach (var puppet in puppets)
            {
                if (puppet == null)
                {
                    puppetsLeft--;
                }
            }
            if(puppetsLeft == 0)
            {
                puppetRespawnTime--;
            }
        }
        else
        {
            if (puppetsLeft == 0)
            {
                puppetRespawnTime--;
            }
        }
        
        if(puppetRespawnTime == 0)
        {
            Debug.Log("Whut");
            attackNum = 4;
        }
        //attackNum = 3;
        setPuppetAttack();
        
        switch (attackNum)
        {
            case 1:
                playerPos = PlayerController.instance.gameObject.transform.position;
                StartCoroutine(SpinDance());
                //PhaseChange();
                break;
            case 2:
                StartCoroutine(Rush());
                //PhaseChange();
                break;
            case 3:
                StartCoroutine(BladeFlourish());
                //PhaseChange();
                break;
            case 4:
                StartCoroutine(SummonPuppets());
                //PhaseChange();
                break;
            case 5:
                StartCoroutine(DetonatePuppets());
                //PhaseChange();
                break;
            case 6:
                //StartCoroutine(KickDance());
                PhaseChange();
                break;
            case 7:
                StartCoroutine(PuppeteersGrasp());
                //PhaseChange();
                break;
            default:
                print("Incorrect Number.");
                break;
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
        else if (MathF.Floor(globalTime) % 15 != 0)
        {
            timeIndicator = true;
        }
    }

    public IEnumerator Death()
    {
        DimLights.instance.TurnOff();
        // kills any remaining puppets once Cordelia dies
        foreach (var puppet in puppets)
        {
            if (puppet is not null)
            {
                Destroy(puppet);
            }
        }

        foreach (var v in gloves)
        {
            if (v is not null)
            {
                Destroy(v);
            }
        }

        yield return new WaitWhile(() => m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        m_Animator.SetTrigger("Death");
        yield return new WaitForSeconds(3f);


        BossController.instance.BossDie();
        Destroy(gameObject);
    }

    public override void Die()
    {

        StopAllCoroutines();
        StartCoroutine(Death());
        
    }

}