using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Handles the boss code for Onyx
public class Onyx : Damageable, Boss
{
    float m_MaxBulletVelocity;

    public float speed = 5.0f;

    private Animator m_Animator;
    public ParticleSystem fireParticlesR;
    public ParticleSystem fireParticlesL;
    private bool m_Run = false;

    private bool reverseRunDirection = false;
    private bool isCoroutineRunning = false;

    bool m_invulnerable = false;

    //coroutine variables. needed so they can be stopped on collision
    Coroutine jetChargeCoroutine;
    Coroutine runCoroutine;

    private float difficultyMultiplier = 1f;
    private int level = 0;

    private Rigidbody2D OnyxRB;
    public string[] Attacks { get; } =
    {
        "Pistol_Blast", "Dual_Danger", "Summon", "Machine_Assault", "High_Explosive", "Smoke_Screen", "Tetra_Takedown", "Jet_Charge", "Radial_Fire"
    };

    // Sets up Onyx variables and difficulty
    void Start()
    {
        m_MaxBulletVelocity = 40f;
        m_Animator = GetComponent<Animator>();
        OnyxRB = GetComponent<Rigidbody2D>();

        difficultyMultiplier = GameManager.instance.getDifficultyModifier();
        Debug.Log("difficulty multiplier " + difficultyMultiplier);
        level = GameManager.instance.getCurrentLevel();
    }

    // Displays jet engine fire particles.
    void PlayFireParticles()
    {
        fireParticlesL.Play();
        fireParticlesR.Play();
    }

    // Stops jet engine fire particles.
    void StopFireParticles()
    {
        fireParticlesL.Stop();
        fireParticlesR.Stop();
    }

    // Aims a pistol blast at the player and shoots them a few times.
    IEnumerator Pistol_Blast()
    {
        Debug.Log("Pistol Blast");
        yield return new WaitWhile(() => m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        int yOffsetIndicator = 1;
        int xOffsetIndicator = 1;
        //Get the player postition relative to the boss
        Vector3 playerPos = PlayerController.instance.transform.position - transform.position;
        //Get the angle from the position
        float playerAngle = Mathf.Atan2(playerPos.y, playerPos.x);
        // Check for the 8 directions
        // TODO: Clean this up into a nice function as similar code is used elsewhere.
        if (playerAngle >= (-1 * Mathf.PI / 8) && playerAngle < (Mathf.PI / 8))
        {
            m_Animator.SetTrigger("Pistol Blast");
            yOffsetIndicator = 1;
            xOffsetIndicator = 3;
        }
        else if (playerAngle >= (Mathf.PI / 8) && playerAngle < (3 * Mathf.PI / 8))
        {
            m_Animator.SetTrigger("Pistol NE");
            yOffsetIndicator = 2;
            xOffsetIndicator = 1;
        }
        else if (playerAngle >= (3 * Mathf.PI / 8) && playerAngle < (5 * Mathf.PI / 8))
        {
            m_Animator.SetTrigger("Pistol Up");
            yOffsetIndicator = 3;
            xOffsetIndicator = -1;
        }
        else if (playerAngle >= (5 * Mathf.PI / 8) && playerAngle < (7 * Mathf.PI / 8))
        {
            m_Animator.SetTrigger("Pistol NW");
            yOffsetIndicator = 2;
            xOffsetIndicator = -2;
        }
        else if (playerAngle >= (7 * Mathf.PI / 8) || playerAngle < (-7 * Mathf.PI / 8))
        {
            m_Animator.SetTrigger("Pistol Blast");
            yOffsetIndicator = 1;
            xOffsetIndicator = -2;
        }
        else if (playerAngle >= (-7 * Mathf.PI / 8) && playerAngle < (-5 * Mathf.PI / 8))
        {
            m_Animator.SetTrigger("Pistol SW");
            yOffsetIndicator = -1;
            xOffsetIndicator = -2;
        }
        else if (playerAngle >= (-5 * Mathf.PI / 8) && playerAngle < (-3 * Mathf.PI / 8))
        {
            m_Animator.SetTrigger("Pistol Down");
            yOffsetIndicator = -2;
            xOffsetIndicator = 1;
        }
        else
        {
            m_Animator.SetTrigger("Pistol SE");
            yOffsetIndicator = -1;
            xOffsetIndicator = 2;
        }
        yield return new WaitForSeconds(.15f);
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
                if (_ == 0)
                {
                    yield return new WaitForSeconds(.4f);
                }
                else
                {
                    yield return new WaitForSeconds(.06f);
                }
            }
            else
            {
                //bullet
                GameObject indicator = indicators[0];
                Vector3 bulletPosition = new Vector3(transform.position.x + xOffsetIndicator, transform.position.y + yOffsetIndicator, 1) + (1 * playerPos / 5);
                GameObject bullet = Instantiate(
                    GameManager.instance.getBulletPrefab("Test Bullet"),
                    bulletPosition,
                    indicator.transform.rotation);
                if (BulletWillHitBoss(bulletPosition))
                {
                    Debug.Log("BULLET WILL HIT BOSS");
                }
                bullet.transform.Rotate(0, 0, 270);
                //get rid of indicator
                Destroy(indicator);
                indicators.RemoveAt(0);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                //depending on difficulty, set bulletSpeed
                float bulletSpeed = 30 * difficultyMultiplier;
                rb.AddForce(bullet.transform.right * bulletSpeed, ForceMode2D.Impulse);
                indicate = true;
                yield return new WaitForSeconds(.06f);
            }
        }
        m_Animator.SetTrigger("Run");

        ;
    }

    // Determines whether a bullet is in a trajectory that will collide with the boss.
    bool BulletWillHitBoss(Vector3 bulletPosition)
    {
        // Perform a raycast from the boss's position to the bullet's position
        Vector3 bossPosition = transform.position;
        Vector3 playerPos = PlayerController.instance.transform.position - transform.position;
        Vector3 direction = (bulletPosition - playerPos).normalized;
        float distance = Vector3.Distance(playerPos, bulletPosition);
        RaycastHit2D hit = Physics2D.Raycast(bulletPosition, direction, distance);
        Debug.Log("boss position" + bossPosition);
        Debug.Log("direction" + direction);
        Debug.Log("distance" + distance);
        Debug.Log("\n");

        // Check if the raycast hits the boss's collider
        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            // The bullet will hit the boss
            Debug.Log("OUCH");
            return true;
        }

        // The bullet will not hit the boss
        Debug.Log("No ouch");
        return false;
    }

    // Shoots the player in a wide area using some dual shotguns.
    IEnumerator Dual_Danger()
    {
        Debug.Log("Dual Danger");
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
            yOffsetIndicator = 3;
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
            yOffsetIndicator = 1;
            xOffsetIndicator = -3;
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
            yOffsetIndicator = -3;
            xOffsetIndicator = 1;
        }
        else
        {
            m_Animator.SetTrigger("Dual SE");
            yOffsetIndicator = -2;
            xOffsetIndicator = 1;
        }
        //pause to let animation catch up to firing
        yield return new WaitForSeconds(.15f);

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

                //depending on difficulty, set bulletSpeed
                float bulletSpeed = 10 * difficultyMultiplier;
                rb.AddForce(bullet.transform.right * bulletSpeed, ForceMode2D.Impulse);
            }
        }
        m_Animator.SetTrigger("Run");

    }

    // Causes a large radial attack of bullets to strike out from Onyx
    IEnumerator Machine_Assault()
    {
        Debug.Log("Machine Assault");
        yield return new WaitWhile(() => m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);

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
            yield return new WaitForSeconds(.05f / difficultyMultiplier);
        }
        foreach (var indicator in indicators)
        {
            //Fire a bullet at the player based on its position
            GameObject bullet = Instantiate(
                GameManager.instance.getBulletPrefab("Test Bullet"),
                transform.position - ((transform.position - indicator.transform.position).normalized * 5f),
                indicator.transform.rotation
            );
            //rotate by sprite rotation offset
            bullet.transform.Rotate(0, 0, 270);
            BossController.instance.RemoveIndicator(indicator);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            //depending on difficulty, set bulletSpeed
            float bulletSpeed = 30 * difficultyMultiplier;
            rb.AddForce(bullet.transform.right * bulletSpeed, ForceMode2D.Impulse);
            yield return new WaitForSeconds(.05f / difficultyMultiplier);
        }
        m_Animator.SetTrigger("Run");

    }


    // Causes Onyx to shoot forward propelled by a jet engine, dealing high contact damage.


    // Throws several projectiles that burst into smaller projectiles.
    public IEnumerator HighExplosive()
    {
        m_Animator.SetTrigger("High Explosive");
        yield return new WaitWhile(() => m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);


        //throw cinder clusters (similar to blag attack)
        GameObject bulletPreFab = GameManager.instance.getBulletPrefab("Cinder Cluster");

        //Get the player postition relative to the boss
        Vector3 playerPos = PlayerController.instance.transform.position - transform.position;
        //Get the angle from the position
        float playerAngle = Mathf.Atan2(playerPos.y, playerPos.x);

        List<GameObject> indicators = new(3);
        //wait for animation to start throwing cinder clusters
        yield return new WaitForSeconds(1f);
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
            float bulletSpeed = 15 * difficultyMultiplier;
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(bullet.transform.up * bulletSpeed, ForceMode2D.Impulse);

            yield return new WaitForSeconds(.3f);
        }
        m_Animator.SetTrigger("Run");

    }

    // Prepares to drop several explosives on the player, indicating where they'll all drop before all exploding simultaneously.
    public IEnumerator Grenade()
    {
        Debug.Log("Grenade");
        yield return new WaitWhile(() => m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);

        m_Animator.SetTrigger("High Explosive");

        List<GameObject> indicators = new(5);
        for (int i = 0; i < 5; i++)
        {
            GameObject indicator = BossController.instance.IndicateCircle(
                PlayerController.instance.transform.position,
                Quaternion.identity
            );
            indicators.Add(indicator);
            indicator.transform.localScale = new Vector3(4, 4, 1);
            float startSize = indicator.transform.localScale.x;
            for (float size = startSize; size >= 0; size -= startSize / 50)
            {
                indicator.transform.localScale = new Vector3(size, size, 1);
                yield return new WaitForSeconds(.01f);
            }
        }
        foreach (var indicator in indicators)
        {
            Instantiate(
                GameManager.instance.getBulletPrefab("Grenade Strike"),
                indicator.transform.position,
                indicator.transform.rotation
            );
            BossController.instance.RemoveIndicator(indicator);
        }
        yield return new WaitForSeconds(.25f);
        m_Animator.SetTrigger("Run");

    }

    public IEnumerator JetCharge()
    {
        Debug.Log("Jet Charge");
        //charge in the direction of player
        //damage on contact with the player
        // if between -45 and 45 degrees, move to the right
        m_invulnerable = true;
        bool collided = false;

        yield return new WaitWhile(() => m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);


        // Get direction towards player
        Vector3 playerPos = PlayerController.instance.transform.position - transform.position;
        float playerAngle = Mathf.Atan2(playerPos.y, playerPos.x);
        Vector3 moveDirection = playerPos.normalized;

        float duration = .8f; // .8 seconds duration

        if (playerAngle < (Mathf.PI / 4) && playerAngle > (-1 * Mathf.PI / 4))
        {
            m_Animator.SetTrigger("JetRight");
        }
        else if (playerAngle > (Mathf.PI / 4) && playerAngle < (3 * Mathf.PI / 4))
        {
            m_Animator.SetTrigger("JetUp");
        }
        // 135, 225 degrees (on sides) to the left
        else if (playerAngle > (3 * Mathf.PI / 4) || playerAngle < (-3 * Mathf.PI / 4))
        {
            m_Animator.SetTrigger("JetLeft");
        }
        else
        {
            m_Animator.SetTrigger("JetDown");
        }
        //after setting animation, wait for change then start moving
        yield return new WaitForSeconds(.8f);
        //depending on difficulty, set bulletSpeed
        float jetSpeed = 12 * difficultyMultiplier;
        float timer = 0f;
        while (timer < duration && !collided)
        {
            float jetDistance = jetSpeed * Time.deltaTime;
            jetDistance = jetDistance > .04f ? .04f : jetDistance;

            // Check for collisions along the movement path
            if (CheckCollisionAlongPath(moveDirection, jetDistance))
            {
                collided = true;
                jetDistance = 0f; // Set jetDistance to 0 if collision occurs
            }

            transform.Translate(moveDirection * jetDistance, Space.World);

            timer += Time.deltaTime;
            yield return null;
        }


        m_invulnerable = false;
        m_Animator.SetTrigger("Run");
        yield return new WaitUntil(() => m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Run"));

    }

    // Dispatches Onyx's attacks.
    public void PhaseChange()
    {

    }

    // Function to check for collisions along the movement path
    private bool CheckCollisionAlongPath(Vector3 moveDirection, float distance)
    {
        RaycastHit hit;
        // Perform a raycast along the movement path
        if (Physics.Raycast(transform.position, moveDirection, out hit, distance))
        {
            // Check if the hit object has a tag indicating a collision (e.g., "Wall")
            return hit.collider.CompareTag("Wall");
        }

        // No collision along the path
        return false;
    }

    // Causes Onyx to run around, dealing contact damage.
    public IEnumerator Run()
    {
        Debug.Log("Run");
        yield return new WaitWhile(() => m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);

        m_Animator.SetTrigger("Run");
        float timer = 0f;

        float duration = 5 / difficultyMultiplier; // duration gets shorter the harder the difficulty
                                                   // Keep moving towards the player for the specified duration
        Debug.Log("Running duration = " + duration);
        while (timer < duration)
        {
            Vector3 directionToPlayer = PlayerController.instance.transform.position - transform.position;
            Vector3 moveDirection = reverseRunDirection ? -directionToPlayer.normalized : directionToPlayer.normalized;
            float distanceToMove = speed * Time.deltaTime;
            distanceToMove = distanceToMove > .02f ? .02f : distanceToMove;

            transform.Translate(moveDirection * distanceToMove, Space.World);

            timer += Time.deltaTime;
            yield return null;
        }

    }

    // Handles Onyx dying, with a small death animation.
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

    IEnumerator DriveAttacks()
    {
        while (true)
        {
            if (!isCoroutineRunning)
            {
                isCoroutineRunning = true;

                if (!m_Run)
                {
                    int randAttack = Random.Range(0, level + 3);
                    Debug.Log("Phase Change: Attacking");
                    StartCoroutine(StartAttackCoroutine(randAttack));
                }
                else
                {
                    Debug.Log("Phase Change: Running");
                    StartCoroutine(StartRunCoroutine());
                }
            }
            yield return null;
        }
    }


    private IEnumerator StartAttackCoroutine(int randAttack)
    {
        isCoroutineRunning = true;
        m_Run = false;
        switch (randAttack)
        {
            case 0:
                yield return StartCoroutine(Grenade());
                break;
            case 1:
                yield return StartCoroutine(Dual_Danger());
                break;
            case 2:
                yield return StartCoroutine(Machine_Assault());
                break;
            case 3:
                jetChargeCoroutine = StartCoroutine(JetCharge());
                yield return jetChargeCoroutine;
                break;
            case 4:
                yield return StartCoroutine(HighExplosive());
                break;
            case 5:
                yield return StartCoroutine(Pistol_Blast());
                break;
            default:
                break;
        }

        isCoroutineRunning = false;
        m_Run = true;
    }

    private IEnumerator StartRunCoroutine()
    {
        reverseRunDirection = false;
        isCoroutineRunning = true;
        m_Run = true;
        runCoroutine = StartCoroutine(Run());
        yield return runCoroutine;
        isCoroutineRunning = false;
        m_Run = false;
    }

    // Startup code.
    public IEnumerator StartPhase()
    {
        Invulnerable = true;
        yield return new WaitForSeconds(1f);
        Invulnerable = false;

        StartCoroutine(DriveAttacks());
    }

    // Handlex Onyx dying.
    public override void Die()
    {
        Invulnerable = true;
        StopAllCoroutines();
        StartCoroutine(Death());
    }

    public void ForceDeath()
    {
        StopAllCoroutines();
        BossController.instance.BossDie();
        Destroy(gameObject);
    }

    // Handles contact damage, varies based on which mode Onyx is in.
    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Player")
        {
            float damage = 2f;
            Debug.Log("onyx collision with wall or player");


            if (m_Run)
            {
                reverseRunDirection = !reverseRunDirection;

            }

            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<Damageable>().takeDamage(damage);
            }
        }

    }
}

