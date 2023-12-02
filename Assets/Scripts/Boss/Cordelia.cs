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

    private List<GameObject> puppets = new List<GameObject>();
    private List<GameObject> gloves = new List<GameObject>();
    private List<Vector3> voidList = new List<Vector3>();

    void Start()
    {
        m_DifficultyModifier = GameManager.instance.getCurrentDifficultyInt() * 0.5f + 1;
        m_LevelModifier = (GameManager.instance.getCurrentLevel() - 1) * 0.5f + 1;
        if(m_LevelModifier > 1 || m_DifficultyModifier > 1)
        {
            diffAttack = 9;
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

        {
            voidList.Add(new Vector3(-82.47555f, -0.6747541f, 0));
            voidList.Add(new Vector3(-74.66592f, -3.49f, 0));
            voidList.Add(new Vector3(-67.09f, -3.02f, 0));
            voidList.Add(new Vector3(-73.18208f, 1.824329f, 0));
            voidList.Add(new Vector3(-81.92888f, 4.948183f, 0));
            voidList.Add(new Vector3(-88.56707f, 5.651052f, 0));
            voidList.Add(new Vector3(-74.74401f, 8.853002f, 0));
            voidList.Add(new Vector3(-65.76293f, 8.384424f, 0));
            voidList.Add(new Vector3(-67.87153f, 4.089124f, 0));
            voidList.Add(new Vector3(-61.38953f, 1.980522f, 0));
            voidList.Add(new Vector3(-62.17f, -2f, 0));
            voidList.Add(new Vector3(-65.37244f, -5.985308f, 0));
            voidList.Add(new Vector3(-88.25468f, 0.1062098f, 0));
            voidList.Add(new Vector3(-93.0f, 1.36f, 0));
            voidList.Add(new Vector3(-98.09f, 4.25f, 0));
            voidList.Add(new Vector3(-91.92521f, 8.69681f, 0));
            voidList.Add(new Vector3(-84.50606f, 8.540617f, 0));
            voidList.Add(new Vector3(-78.96121f, 1.824329f, 0));
            voidList.Add(new Vector3(-85.36511f, 2.605293f, 0));
            voidList.Add(new Vector3(-75.83736f, 5.338666f, 0));
            voidList.Add(new Vector3(-79.89837f, -3.017645f, 0));
            voidList.Add(new Vector3(-79.89837f, -3.017645f, 0));
            voidList.Add(new Vector3(-101.5311f, 7.759653f, 0));
            voidList.Add(new Vector3(-96.92338f, 9.633965f, 0));
            voidList.Add(new Vector3(-105.4359f, 10.18064f, 0));
            voidList.Add(new Vector3(-101.2968f, 12.28924f, 0));
            voidList.Add(new Vector3(-109.03f, 13.3f, 0));
            voidList.Add(new Vector3(-106.6854f, 16.27216f, 0));
            voidList.Add(new Vector3(-101.2968f, 15.72548f, 0));
            voidList.Add(new Vector3(-103.0149f, 18.92743f, 0));
            voidList.Add(new Vector3(-95.83003f, 12.75782f, 0));
            voidList.Add(new Vector3(-89.8166f, 11.43018f, 0));
            voidList.Add(new Vector3(-79.58598f, 7.60346f, 0));
            voidList.Add(new Vector3(-67.48105f, 1.043366f, 0));
            voidList.Add(new Vector3(-70.683f, -5.360537f, 0));
            voidList.Add(new Vector3(-60.22f, -5.83f, 0));
            voidList.Add(new Vector3(-55.77f, -4.11f, 0));
            voidList.Add(new Vector3(-56.62566f, 0.4966908f, 0));
            voidList.Add(new Vector3(-71.07348f, 6.510111f, 0));
            voidList.Add(new Vector3(-95.04906f, 15.80358f, 0));
            voidList.Add(new Vector3(-89.50423f, 14.78832f, 0));
            voidList.Add(new Vector3(-84.89654f, 11.66447f, 0));
            voidList.Add(new Vector3(-79.42979f, 10.88351f, 0));
            voidList.Add(new Vector3(-73.96304f, 11.89876f, 0));
            voidList.Add(new Vector3(-70.21442f, 9.712062f, 0));
            voidList.Add(new Vector3(-62.95146f, 5.416761f, 0));
            voidList.Add(new Vector3(-57.09423f, 4.16722f, 0));
            voidList.Add(new Vector3(-52.33035f, -1.61191f, 0));
            voidList.Add(new Vector3(-51.78368f, 2.058619f, 0));
            voidList.Add(new Vector3(-46.70741f, 3.932931f, 0));
            voidList.Add(new Vector3(-52.09607f, 5.494859f, 0));
            voidList.Add(new Vector3(-57.64091f, 7.369171f, 0));
            voidList.Add(new Vector3(-61.54572f, 10.25874f, 0));
            voidList.Add(new Vector3(-65.99722f, 11.97686f, 0));
            voidList.Add(new Vector3(-78.25835f, 13.61688f, 0));
            voidList.Add(new Vector3(-83.647f, 14.31975f, 0));
            voidList.Add(new Vector3(-90.83186f, 18.22456f, 0));
            voidList.Add(new Vector3(-96.84528f, 19.08362f, 0));
            voidList.Add(new Vector3(-98.95389f, 22.44177f, 0));
            voidList.Add(new Vector3(-84.58415f, 17.5217f, 0));
            voidList.Add(new Vector3(-78.57073f, 16.97502f, 0));
            voidList.Add(new Vector3(-73.41637f, 15.56929f, 0));
            voidList.Add(new Vector3(-69.90204f, 13.61688f, 0));
            voidList.Add(new Vector3(-67.87153f, 16.97502f, 0));
            voidList.Add(new Vector3(-63.65433f, 15.17881f, 0));
            voidList.Add(new Vector3(-92.70618f, 21.34842f, 0));
            voidList.Add(new Vector3(-94.73668f, 24.78466f, 0));
            voidList.Add(new Vector3(-91.45663f, 27.28374f, 0));
            voidList.Add(new Vector3(-88.87946f, 24.39418f, 0));
            voidList.Add(new Vector3(-86.77085f, 20.64555f, 0));
            voidList.Add(new Vector3(-81.06982f, 20.09888f, 0));
            voidList.Add(new Vector3(-83.88129f, 23.06654f, 0));
            voidList.Add(new Vector3(-85.44321f, 26.89326f, 0));
            voidList.Add(new Vector3(-80.13266f, 25.25324f, 0));
            voidList.Add(new Vector3(-78.80502f, 28.0647f, 0));
            voidList.Add(new Vector3(-76.85262f, 22.51986f, 0));
            voidList.Add(new Vector3(-74.9783f, 19.16172f, 0));
            voidList.Add(new Vector3(-69.74584f, 20.02078f, 0));
            voidList.Add(new Vector3(-71.54206f, 22.98844f, 0));
            voidList.Add(new Vector3(-74.43163f, 25.79991f, 0));
            voidList.Add(new Vector3(-70.14f, 27.36f, 0));
            voidList.Add(new Vector3(-66.3877f, 23.8475f, 0));
            voidList.Add(new Vector3(-64.35719f, 19.00553f, 0));
            voidList.Add(new Vector3(-58.65616f, 13.0702f, 0));
            voidList.Add(new Vector3(-55.06373f, 9.868255f, 0));
            voidList.Add(new Vector3(-49.05031f, 8.150135f, 0));
            voidList.Add(new Vector3(-44.75501f, 6.900593f, 0));
            voidList.Add(new Vector3(-50.22176f, 11.66447f, 0));
            voidList.Add(new Vector3(-54.20467f, 14.31975f, 0));
            voidList.Add(new Vector3(-59.12474f, 16.58454f, 0));
            voidList.Add(new Vector3(-62.40479f, 21.7389f, 0));
            voidList.Add(new Vector3(-61.31144f, 25.09704f, 0));
            voidList.Add(new Vector3(-58.65616f, 19.39601f, 0));
            voidList.Add(new Vector3(-53.42371f, 17.20931f, 0));
            voidList.Add(new Vector3(-48.89411f, 14.86642f, 0));
            voidList.Add(new Vector3(-45.22358f, 10.18064f, 0));
            voidList.Add(new Vector3(-56.85994f, 22.59796f, 0));
            voidList.Add(new Vector3(-52.72084f, 20.02078f, 0));
            voidList.Add(new Vector3(-48.19125f, 17.59979f, 0));
            voidList.Add(new Vector3(-45.92645f, 12.83592f, 0));
            voidList.Add(new Vector3(-51.15891f, 22.44177f, 0));
            voidList.Add(new Vector3(-56.07898f, 25.33133f, 0));
            voidList.Add(new Vector3(-65.52864f, 26.42468f, 0));
            voidList.Add(new Vector3(-93.25285f, 4.323413f, 0));
            voidList.Add(new Vector3(-96.14241f, 6.7444f, 0));
            voidList.Add(new Vector3(-75.99355f, -0.7528496f, 0));
            voidList.Add(new Vector3(-71.54206f, -1.455718f, 0));
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

    IEnumerator StringDance()
    {
        Debug.Log("StringDance");
        /*Vector3 t = transform.position;
        t.y = t.y - 3f;
        Vector3 direction = (playerPos - t).normalized;
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        GameObject s = Instantiate(stringPreFab, t, Quaternion.Euler(0, 0, angle));

        float length = 0f;
        float endTime = 10f;
        playerPos = PlayerController.instance.transform.position;
        //playerPos = PlayerController.instance.gameObject.transform.position;
        while (length < endTime)
        {
            float moveSpeed = .01f;
            float stretchingSpeed = 10f;
            var movementVector = (playerPos - s.transform.position).normalized * moveSpeed;
            s.transform.Translate(movementVector);
            s.transform.localScale += new Vector3(stretchingSpeed * Time.deltaTime, 0f, 0f);

            length += Time.deltaTime;
            yield return null;
        }*/
        /*int stringsNum = 5;
        Vector3 t = transform.position;
        t.y = t.y - 3f;

        Vector3 direction = (playerPos - t).normalized;
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90);
        GameObject s = Instantiate(stringPreFab, transform.position, Quaternion.Euler(0, 0, angle));
        float length = 0f;
        float endTime = 10f;
        playerPos = PlayerController.instance.transform.position;
        //playerPos = PlayerController.instance.gameObject.transform.position;
        while (length < endTime)
        {
            t = transform.position;
            t.y = t.y - 3f;

            direction = (playerPos - t).normalized;//s.transform.position;

            // Cast a ray in that direction to detect obstacles (walls)
            RaycastHit2D hit = Physics2D.Raycast(t, direction*20, Mathf.Infinity);
            if(hit.collider != null)
            {
                Debug.Log(hit.collider.name);
            }
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Wall"))
            {
                // If the ray hits a wall, stop stretching
                //isStretching = false;
                Debug.Log("Wall Hit");
                // Set the position to the hit point
                t = hit.point;
                Debug.Log(hit.distance);
                s.transform.localScale = new Vector3(hit.distance, s.transform.localScale.y, 0f);
            }
            else
            {
                float stretchingSpeed = 20f;
                //float distance = Vector3.Distance(s.transform.position, playerPos);
                //s.transform.localScale = new Vector3(distance* Time.deltaTime, s.transform.localScale.y, s.transform.localScale.z);
                s.transform.localScale += new Vector3(stretchingSpeed * Time.deltaTime, 0f, 0f);
                Debug.DrawRay(t, direction*100f, UnityEngine.Color.red);
            }
            length += Time.deltaTime;
            yield return null;
        }*/
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
                    Debug.Log(bullet.transform.position);
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
        Debug.Log("PuppeteersGrasp");
        playerPos = PlayerController.instance.transform.position;
        voidList.Sort((v1, v2) => Vector3.Distance(v2, playerPos).CompareTo(Vector3.Distance(v1, playerPos)));

        int voidAmount = 20; // actual is voidAmount/crowding
        int crowding = 4;
        float rate = 1f;
        int spreadCount = 0;
        Debug.Log(voidList.Count - 1 - voidAmount);
        for (int i = voidList.Count-1; i > voidList.Count - voidAmount; i-=crowding)
        {
            //Debug.Log(i);
            //Debug.Log(i - voidAmount);
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

        PhaseChange();
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

    public void PhaseChange()
    {

        
        int temp = attackNum;
        // could make it so that only some attacks can't be twice in a row
        while (temp == attackNum) 
        {
            attackNum = UnityEngine.Random.Range(1, diffAttack);
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
                //StartCoroutine(StringDance());
                PhaseChange();
                break;
            case 8:
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

    public override void Die()
    {
        // kills any remaining puppets once Cordelia dies
        foreach (var puppet in puppets)
        {
            if(puppet is not null) 
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
        BossController.instance.BossDie();

        DimLights.instance.TurnOff();
        Destroy(gameObject);
    }

}