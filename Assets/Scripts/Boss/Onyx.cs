using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Onyx : MonoBehaviour, Boss, IHealth
{
    float m_MaxBulletVelocity;

    public float MaxHP = 5.0f;
    public float speed = 5.0f;
    private float m_CurrHP;

    private Animator m_Animator;
    private bool m_Run = false;

    bool m_invulnerable = false;

    private int difficulty = 0; //0 = easy, 1 = medium, 2 = hard
    private int level = 0;

    //IHealth Stuff
    public float MaxHealth { get => MaxHP; set => MaxHP = value; }
    public float CurrentHealth { get => m_CurrHP; set => m_CurrHP = value; }
    public bool Invulnerable { get => m_invulnerable; set => m_invulnerable = value; }

    public string[] Attacks { get; } =
    {
        "Pistol_Blast", "Dual_Danger", "Summon", "Machine_Assault", "High_Explosive", "Smoke_Screen", "Tetra_Takedown", "Jet_Charge", "Radial_Fire"
    };

    void Start()
    {
        m_CurrHP = MaxHP;
        m_MaxBulletVelocity = 40f;
        m_Animator = GetComponent<Animator>();
        GameObject gameManagerObject = GameObject.Find("GameManager");

        if (gameManagerObject != null)
        {
            // Try to get the GameManager component attached to the GameManager GameObject
            GameManager gameManagerScript = gameManagerObject.GetComponent<GameManager>();

            if (gameManagerScript != null)
            {
                // Now you can access the difficulty variable
                difficulty = gameManagerScript.getCurrentDifficulty();
                level = gameManagerScript.getCurrentLevel();
                Debug.Log("Difficulty: " + difficulty);
            }
            else
            {
                Debug.LogError("GameManager component not found on GameManager GameObject.");
            }
        }
        else
        {
            Debug.LogError("GameManager GameObject not found in the scene.");
        }

    }
    IEnumerator Pistol_Blast()
    {
        int yOffsetIndicator = 1;
        int xOffsetIndicator = 1;
        //Get the player postition relative to the boss
        Vector3 playerPos = PlayerController.instance.transform.position - transform.position;
        //Get the angle from the position
        float playerAngle = Mathf.Atan2(playerPos.y, playerPos.x);
        // if btwn -45 and 45, to the right
        if (playerAngle < (Mathf.PI / 4) && playerAngle > (-1 * Mathf.PI / 4))
        {
            m_Animator.SetTrigger("Pistol Blast");
            yOffsetIndicator = 1;
            xOffsetIndicator = 1;
        }
        else if (playerAngle > (Mathf.PI / 4) && playerAngle < (3 * Mathf.PI / 4))
        {
            m_Animator.SetTrigger("Pistol Up");
            yOffsetIndicator = 2;
            xOffsetIndicator = -1;
        }
        //135, 225 degrees (on sides) to the left
        else if (playerAngle > (3 * Mathf.PI / 4) || playerAngle < (-3 * Mathf.PI / 4))
        {
            m_Animator.SetTrigger("Pistol Blast");
            yOffsetIndicator = 1;
            xOffsetIndicator = -1;
        }
        else
        {
            m_Animator.SetTrigger("Pistol Down");
            yOffsetIndicator = -2;
            xOffsetIndicator = 1;
        }
        //after setting the trigger, wait to make timing of shots line up with animation
        yield return new WaitForSeconds(.5f);
        //6 fast revolver shots
        List<GameObject> indicators = new(1);
        bool indicate = true;
        foreach (var _ in Enumerable.Range(0, 10))
        {
            if (indicate)
            {
                //Get the player postition relative to the boss
                playerPos = PlayerController.instance.transform.position - transform.position;
                //Get the angle from the position
                //y and x offset to so indicator does not come from belly button
                playerAngle = Mathf.Atan2(playerPos.y - yOffsetIndicator, playerPos.x - xOffsetIndicator);
                //create indicator
                GameObject indicator = Instantiate(
                    BossController.instance.inidcatorSmallPrefab,
                    new Vector3(transform.position.x + xOffsetIndicator, transform.position.y + yOffsetIndicator, 1) + (3 * playerPos / 5),
                    Quaternion.Euler(0, 0, playerAngle * Mathf.Rad2Deg + 90)
                );
                indicator.transform.localScale = new Vector3(1, playerPos.magnitude, 1);
                indicators.Add(indicator);
                //next loop, bullet
                indicate = false;
                yield return new WaitForSeconds(.1f);
            }
            else
            {
                //bullet
                GameObject indicator = indicators[0];
                GameObject bullet = Instantiate(
                    GameManager.instance.getBulletPrefab("Test Bullet"),
                    new Vector3(transform.position.x + xOffsetIndicator, transform.position.y + yOffsetIndicator, 1) + (1 * playerPos / 5),
                    indicator.transform.rotation);
                bullet.transform.Rotate(0, 0, 270);
                //get rid of indicator
                Destroy(indicator);
                indicators.RemoveAt(0);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                float bulletSpeed = 45f;
                //depending on difficulty, set bulletSpeed
                if (difficulty == 0)
                {
                    bulletSpeed = 35f;
                }
                else if (difficulty == 1)
                {
                    bulletSpeed = 45f;
                }
                else if (difficulty == 2)
                {
                    bulletSpeed = 55f;
                }
                rb.AddForce(bullet.transform.right * bulletSpeed, ForceMode2D.Impulse);
                indicate = true;
                yield return new WaitForSeconds(.1f);
            }
        }
        m_Animator.SetTrigger("Run");
        PhaseChange();
    }

    IEnumerator Dual_Danger()
    {
        yield return new WaitWhile(() => m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        int yOffsetIndicator = 1;
        int xOffsetIndicator = 1;
        Vector3 playerPos = PlayerController.instance.transform.position - transform.position;
        float playerAngle = Mathf.Atan2(playerPos.y, playerPos.x);
        // if btwn -45 and 45, to the right
        if (playerAngle >= (-1 * Mathf.PI / 8) && playerAngle < (Mathf.PI / 8))
        {
            m_Animator.SetTrigger("Dual Danger");
            yOffsetIndicator = 1;
            xOffsetIndicator = 3;
        }
        else if (playerAngle >= (Mathf.PI / 8) && playerAngle < (3 * Mathf.PI / 8))
        {
            m_Animator.SetTrigger("Dual NE");
            yOffsetIndicator = 2;
            xOffsetIndicator = 1;
        }
        else if (playerAngle >= (3 * Mathf.PI / 8) && playerAngle < (5 * Mathf.PI / 8))
        {
            m_Animator.SetTrigger("Dual Up");
            yOffsetIndicator = 2;
            xOffsetIndicator = -1;
        }
        else if (playerAngle >= (5 * Mathf.PI / 8) && playerAngle < (7 * Mathf.PI / 8))
        {
            m_Animator.SetTrigger("Dual NW");
            yOffsetIndicator = 2;
            xOffsetIndicator = -2;
        }
        else if (playerAngle >= (7 * Mathf.PI / 8) || playerAngle < (-7 * Mathf.PI / 8))
        {
            m_Animator.SetTrigger("Dual Danger");
            yOffsetIndicator = 0;
            xOffsetIndicator = -2;
        }
        else if (playerAngle >= (-7 * Mathf.PI / 8) && playerAngle < (-5 * Mathf.PI / 8))
        {
            m_Animator.SetTrigger("Dual SW");
            yOffsetIndicator = -2;
            xOffsetIndicator = -1;
        }
        else if (playerAngle >= (-5 * Mathf.PI / 8) && playerAngle < (-3 * Mathf.PI / 8))
        {
            m_Animator.SetTrigger("Dual Down");
            yOffsetIndicator = -2;
            xOffsetIndicator = 1;
        }
        else
        {
            m_Animator.SetTrigger("Dual SE");
            yOffsetIndicator = -1;
            xOffsetIndicator = 2;
        }
        //pause to let animation catch up to firing
        yield return new WaitForSeconds(.9f);

        List<GameObject> indicators = new(4);
        foreach (var _ in Enumerable.Range(0, 3))
        {
            //Get the player postition relative to the boss
            playerPos = PlayerController.instance.transform.position - transform.position;
            //Get the angle from the position
            playerAngle = Mathf.Atan2(playerPos.y - yOffsetIndicator, playerPos.x - xOffsetIndicator);
            //create indicator
            GameObject indicator = Instantiate(
                BossController.instance.inidcatorSmallPrefab,
                new Vector3(transform.position.x + xOffsetIndicator, transform.position.y + yOffsetIndicator, 1) + (3 * playerPos / 5),
                Quaternion.Euler(0, 0, playerAngle * Mathf.Rad2Deg + 90)
            );

            indicator.transform.localScale = new Vector3(1, playerPos.magnitude, 1);
            indicators.Add(indicator);
            yield return new WaitForSeconds(.05f);
        }
        foreach (var indicator in indicators)
        {
            //Fire 10 bullet at the player based on its position
            foreach (var _ in Enumerable.Range(0, 4))
            {
                Vector3 randomOffset = Random.insideUnitSphere * 2.0f;
                GameObject bullet = Instantiate(
                    GameManager.instance.getBulletPrefab("Test Bullet"),
                    new Vector3(transform.position.x + xOffsetIndicator, transform.position.y + yOffsetIndicator, 1) + (1 * playerPos / 5) + randomOffset,
                    indicator.transform.rotation
                );
                bullet.transform.Rotate(0, 0, 270 + Random.Range(-20f, 20f));
                Destroy(indicator);

                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

                float bulletSpeed = 10f;
                if (difficulty == 0)
                {
                    bulletSpeed = 10f;
                }
                else if (difficulty == 1)
                {
                    bulletSpeed = 13f;
                }
                else if (difficulty == 2)
                {
                    bulletSpeed = 20f;
                }
                rb.AddForce(bullet.transform.right * bulletSpeed, ForceMode2D.Impulse);
            }
        }
        m_Animator.SetTrigger("Run");
        PhaseChange();
    }

    IEnumerator Machine_Assault()
    {
        yield return new WaitWhile(() => m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);

        Debug.Log("MachineAssault");
        //12 indicators (radial circle)

        //after all 12 indicators, fire 12 bullets
        List<GameObject> indicators = new(12);
        Vector3 playerPos = PlayerController.instance.transform.position - transform.position;
        m_Animator.SetTrigger("Machine Assault");

        yield return new WaitForSeconds(1f);
        foreach (var i in Enumerable.Range(0, 24))
        {
            int half_indicator_len = 10;
            int theta = ((360 / 24) * i);
            Debug.Log("theta = " + theta);
            float x_indicator = transform.position.x + Mathf.Sin(Mathf.Deg2Rad * theta) * half_indicator_len;
            float y_indicator = transform.position.y - Mathf.Cos(Mathf.Deg2Rad * theta) * half_indicator_len;
            //create the indicator at a position with a rotation
            GameObject indicator = BossController.instance.Indicate(
                new Vector3(x_indicator, y_indicator, 1),
                Quaternion.Euler(0, 0, theta)
            );
            //transform x scale by 20
            indicator.transform.localScale = new Vector3(1, 2 * half_indicator_len, 1);
            indicators.Add(indicator);
            yield return new WaitForSeconds(.05f);
        }
        foreach (var indicator in indicators)
        {
            //Fire a bullet at the player based on its position
            Debug.Log("Boss Position" + transform.position);
            Debug.Log("Bullet Position: " + (indicator.transform.position.normalized * .5f));
            Debug.Log("Bullet Rotation: " + indicator.transform.rotation);
            GameObject bullet = Instantiate(
                GameManager.instance.getBulletPrefab("Test Bullet"),
                transform.position - ((transform.position - indicator.transform.position).normalized * 5f),
                indicator.transform.rotation
            );
            //rotate by sprite rotation offset
            bullet.transform.Rotate(0, 0, 270);
            BossController.instance.RemoveIndicator(indicator);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            float bulletSpeed = 45f;
            if (difficulty == 0)
            {
                bulletSpeed = 35f;
            }
            else if (difficulty == 1)
            {
                bulletSpeed = 45f;
            }
            else if (difficulty == 2)
            {
                bulletSpeed = 55f;
            }
            rb.AddForce(bullet.transform.right * bulletSpeed, ForceMode2D.Impulse);
            yield return new WaitForSeconds(.05f);
        }
        m_Animator.SetTrigger("Run");
        PhaseChange();
    }


    public IEnumerator JetCharge()
    {
        //charge in the direction of player
        //damage on contact with the player
        // if btwn -45 and 45, to the right

        yield return new WaitWhile(() => m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        Debug.Log("Jet Charge");


        //get direction towards player
        Vector3 playerPos = PlayerController.instance.transform.position - transform.position;
        float playerAngle = Mathf.Atan2(playerPos.y, playerPos.x);
        Vector3 moveDirection = playerPos.normalized;

        float timer = 0f;
        float duration = 2f; // 5 seconds duration
        if (playerAngle < (Mathf.PI / 4) && playerAngle > (-1 * Mathf.PI / 4))
        {
            m_Animator.SetTrigger("JetRight");
        }
        else if (playerAngle > (Mathf.PI / 4) && playerAngle < (3 * Mathf.PI / 4))
        {
            m_Animator.SetTrigger("JetUp");
        }
        //135, 225 degrees (on sides) to the left
        else if (playerAngle > (3 * Mathf.PI / 4) || playerAngle < (-3 * Mathf.PI / 4))
        {
            m_Animator.SetTrigger("JetLeft");
        }
        else
        {
            m_Animator.SetTrigger("JetDown");
        }
        //wait for animation before moving
        while (timer < duration)
        {
            float distanceToMove = 15f * Time.deltaTime;
            transform.Translate(moveDirection * distanceToMove, Space.World);

            timer += Time.deltaTime;
            yield return null;
        }
        m_Animator.SetTrigger("Run");
        PhaseChange();
    }

    public IEnumerator HighExplosive()
    {
        yield return new WaitWhile(() => m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);

        //throw grenades (similar to blag attack)
        GameObject bulletPreFab = GameManager.instance.getBulletPrefab("Cinder Cluster");

        //Get the player postition relative to the boss
        Vector3 playerPos = PlayerController.instance.transform.position - transform.position;
        //Get the angle from the position
        float playerAngle = Mathf.Atan2(playerPos.y, playerPos.x);

        List<GameObject> indicators = new(3);
        for (int i = 0; i < 3; i++)
        {
            GameObject indicator = BossController.instance.Indicate(
                new Vector3(transform.position.x, transform.position.y, 1) + (playerPos / 2),
                Quaternion.Euler(0, 0, playerAngle * Mathf.Rad2Deg + 60 + i * 30)
            );
            indicators.Add(indicator);
            indicator.transform.localScale = new Vector3(bulletPreFab.transform.localScale.x, playerPos.magnitude, 1);
            yield return new WaitForSeconds(.2f);
        }

        foreach (GameObject indicator in indicators)
        {
            //Fire a bullet at the player based on its position
            GameObject bullet = Instantiate(
                bulletPreFab,
                transform.position - ((transform.position - indicator.transform.position).normalized * 5f),
                indicator.transform.rotation
            );
            bullet.transform.Rotate(0, 0, 180);

            BossController.instance.RemoveIndicator(indicator);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(bullet.transform.up * 20f, ForceMode2D.Impulse);

            yield return new WaitForSeconds(.3f);
        }

        PhaseChange();
    }

    public IEnumerator Run()
    {
        yield return new WaitWhile(() => m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);

        m_Animator.SetTrigger("Run");
        float timer = 0f;
        float duration = 1f; // 1 second duration
        // Keep moving towards the player for the specified duration
        while (timer < duration)
        {
            Vector3 directionToPlayer = PlayerController.instance.transform.position - transform.position;
            Vector3 moveDirection = directionToPlayer.normalized;
            float distanceToMove = speed * Time.deltaTime;

            transform.Translate(moveDirection * distanceToMove, Space.World);

            // if (directionToPlayer.magnitude < 5.0f)
            // {
            //     // Break out of the loop and yield
            //     yield return StartCoroutine(Dual_Danger());
            // }

            timer += Time.deltaTime;
            yield return null;
        }
        PhaseChange();

    }

    IEnumerator Death()
    {
        Color c = GetComponentInChildren<SpriteRenderer>().color;
        for (float alpha = 1f; alpha >= 0; alpha -= 0.01f)
        {
            c.a = alpha;
            GetComponentInChildren<SpriteRenderer>().color = c;
            yield return null;
        }
        BossController.instance.BossDie();
        Destroy(gameObject);
    }

    public void PhaseChange()
    {

        if (!m_Run)
        {
            //int r = Random.Range(0, level + 2);
            //int r = Random.Range(0, 5);
            int r = 1;
            if (r == 0)
                StartCoroutine(Pistol_Blast());
            else if (r == 1)
                StartCoroutine(Dual_Danger());
            else if (r == 2)
                StartCoroutine(Machine_Assault());
            else if (r == 3)
                StartCoroutine(JetCharge());
            else if (r == 4)
                StartCoroutine(HighExplosive());

            m_Run = true;
        }
        else
        {
            StartCoroutine(Run());
            m_Run = false;
        }
    }

    public IEnumerator StartPhase()
    {
        Invulnerable = true;
        yield return new WaitForSeconds(1f);
        Invulnerable = false;

        PhaseChange();
    }

    public void Die()
    {
        Invulnerable = true;
        StopAllCoroutines();
        StartCoroutine(Death());
    }
}

