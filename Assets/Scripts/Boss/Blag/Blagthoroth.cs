using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

/// Handles the code for Blag'thoroth
public class Blagthoroth : Damageable, Boss
{
    public GameObject deathParticles;
    [SerializeField] public float baseMaxHP;

    private Animator m_Animator;
    private float m_DifficultyModifier;
    private int m_MaxAttack;
    private float m_LevelModifier;
    public float BaseAttackCooldown = 1f;
    
    bool m_carcinized = false;
    

    public string[] Attacks { get; } =
    {
        "Firebolt", "Cinder_Cluster", "Pinch", "Ember_Cascade", "Flame_Strike", "Carcinization", "Sweepers", "Radial_Blast", "Death"
    };

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_DifficultyModifier = GameManager.instance.getDifficultyModifier();
        m_LevelModifier = GameManager.instance.getLevelModifier();
        m_MaxAttack = 4 + GameManager.instance.getCurrentDifficultyInt() / 2;
    }
    
    /// Aims several bolts of fire at the player then fires them simultaneously.
    IEnumerator Firebolt(float countModifier, float speedModifier)
    {
        List<GameObject> indicators = new();

        foreach (var _ in Enumerable.Range(0, Convert.ToInt32(3*countModifier)))
        {
            //Get the player postition relative to the boss
            Vector3 playerPos = BossController.instance.GetPredictedPos(3.0f) - transform.position;
            //Get the angle from the position
            float playerAngle = Mathf.Atan2(playerPos.y, playerPos.x);

            GameObject indicator = BossController.instance.Indicate(
                new Vector3(transform.position.x, transform.position.y, 1) + (playerPos / 2),
                Quaternion.Euler(0, 0, (playerAngle * Mathf.Rad2Deg + 90))
            );

            indicator.transform.localScale = new Vector3(1, playerPos.magnitude, 1);
            indicators.Add(indicator);
            yield return new WaitForSeconds(.15f/speedModifier);
        }

        yield return new WaitForSeconds(.25f / speedModifier);

        foreach (var indicator in indicators)
        {
            //Fire a bullet at the player based on its position
            GameObject bullet = Instantiate(
                GameManager.instance.getBulletPrefab("Flame Bolt"),
                transform.position - ((transform.position - indicator.transform.position).normalized * 5f), 
                indicator.transform.rotation
            );

            bullet.transform.Rotate(0, 0, 270);
            BossController.instance.RemoveIndicator(indicator);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(bullet.transform.right * 35f, ForceMode2D.Impulse);
        }
        
        yield return new WaitForSeconds(1f);
        PhaseChange();
    }
    
    /// Spawns a ball of flame aimed at the player that bursts into smaller bullets on impact.
    IEnumerator Cinder_Cluster(float shardModifier, float speedModifier)
    {
        GameObject bulletPreFab = GameManager.instance.getBulletPrefab("Cinder Cluster");

        //Get the player postition relative to the boss
        Vector3 playerPos = PlayerController.instance.transform.position - transform.position;
        //Get the angle from the position
        float playerAngle = Mathf.Atan2(playerPos.y, playerPos.x);

        GameObject indicator = BossController.instance.Indicate(
            new Vector3(transform.position.x, transform.position.y, 1) + (playerPos / 2),
            Quaternion.Euler(0, 0, playerAngle * Mathf.Rad2Deg + 90)
        );

        indicator.transform.localScale = new Vector3(bulletPreFab.transform.localScale.x, playerPos.magnitude, 1);
        yield return new WaitForSeconds(.5f);

        //Fire a bullet at the player based on its position
        GameObject bullet = Instantiate(
            bulletPreFab,
            transform.position - ((transform.position - indicator.transform.position).normalized * 5f),
            indicator.transform.rotation
        );
        bullet.transform.Rotate(0, 0, 180);
        bullet.GetComponent<CinderCluster>().ShardCount = Convert.ToInt32(bullet.GetComponent<CinderCluster>().ShardCount * shardModifier);

        BossController.instance.RemoveIndicator(indicator);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(bullet.transform.up * 20f * speedModifier, ForceMode2D.Impulse);
        

        yield return new WaitForSeconds(1f);
        PhaseChange();
    }

    /// Makes one of Blag's claws flash green then do a large sweep over the map.
    IEnumerator Pinch()
    {
        yield return new WaitWhile(() => m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        m_Animator.SetTrigger("Pinch");
        int attack = Random.Range(0, 2);
        String arm = attack == 1 ? "Left" : "Right";
        GameObject attackingArm = transform.Find(arm + " Arm").gameObject;
        String fullarm = arm + (m_carcinized? "_Arm_Uncovered" : "_Arm");

        Color c = attackingArm.transform.Find("Blag_" + fullarm).GetComponent<SpriteRenderer>().color;
        for (float green = 1f; green >= 0; green -= 0.02f)
        {
            c.r = green;
            c.b = green;
            attackingArm.transform.Find("Blag_" + fullarm).GetComponent<SpriteRenderer>().color = c;
            yield return new WaitForSeconds(.01f);
        }
        for (float green = 0f; green <= 1; green += 0.02f)
        {
            c.r = green;
            c.b = green;
            attackingArm.transform.Find("Blag_" + fullarm).GetComponent<SpriteRenderer>().color = c;
            yield return new WaitForSeconds(.01f);
        }
        m_Animator.SetTrigger(arm);
        yield return new WaitUntil(() => m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Pinch " + arm));
        yield return new WaitWhile(() => m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        m_Animator.SetTrigger("Finish Attack");
        PhaseChange();
    }
    
    /// Makes Blag enter his second phase, where he sheds his armor and becomes more powerful.
    IEnumerator Carcinization()
    {
        //Getting every single gameobject of the boss :(
        GameObject body = transform.Find("Blag Body").gameObject;
        GameObject bodyUncovered = transform.Find("Blag Body Uncovered").gameObject;
        GameObject ps = body.transform.Find("Particle System").gameObject;
        GameObject rightArm = transform.Find("Right Arm").Find("Blag_Right_Arm").gameObject;
        GameObject rightArmUncovered = transform.Find("Right Arm").Find("Blag_Right_Arm_Uncovered").gameObject;
        GameObject leftArm = transform.Find("Left Arm").Find("Blag_Left_Arm").gameObject;
        GameObject leftArmUncovered = transform.Find("Left Arm").Find("Blag_Left_Arm_Uncovered").gameObject;

        //Setting flags
        m_carcinized = true;
        Invulnerable = true;
        m_Animator.SetTrigger("Carcinization");
        yield return new WaitForSeconds(1f);

        //Activating the uncovered sections
        ps.SetActive(true);
        bodyUncovered.SetActive(true);
        rightArmUncovered.SetActive(true);
        leftArmUncovered.SetActive(true);

        //Getting the color components of each part
        Color bc = body.GetComponent<SpriteRenderer>().color;
        Color buc = bodyUncovered.GetComponent<SpriteRenderer>().color;
        Color rc = rightArm.GetComponent<SpriteRenderer>().color;
        Color ruc = rightArmUncovered.GetComponent<SpriteRenderer>().color;
        Color lc = leftArm.GetComponent<SpriteRenderer>().color;
        Color luc = leftArmUncovered.GetComponent<SpriteRenderer>().color;
        for (float alpha = 1f; alpha >= 0; alpha -= 0.01f)
        {
            //Right arm
            rc.a = alpha;
            rightArm.GetComponent<SpriteRenderer>().color = rc;
            ruc.a = 1f - alpha;
            rightArmUncovered.GetComponent<SpriteRenderer>().color = ruc;

            //Left arm
            lc.a = alpha;
            leftArm.GetComponent<SpriteRenderer>().color = lc;
            luc.a = 1f - alpha;
            leftArmUncovered.GetComponent<SpriteRenderer>().color = luc;

            //Body
            bc.a = alpha;
            body.GetComponent<SpriteRenderer>().color = bc;
            buc.a = 1f - alpha;
            bodyUncovered.GetComponent<SpriteRenderer>().color = buc;
            yield return new WaitForSeconds(.02f);
        }

        //Deactivating covered parts
        ps.SetActive(false);
        body.SetActive(false);
        rightArm.SetActive(false);
        leftArm.SetActive(false);

        //Finish Phase
        m_Animator.SetTrigger("Finish Attack");
        Invulnerable = false;
        yield return new WaitUntil(() => m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
        PhaseChange();
    }
    
    /// Spawns an explosion at the player after a brief delay.
    IEnumerator Flame_Strike(float speedModifier, float sizeModifier)
    {
        float basesize = 4 * sizeModifier;
        GameObject indicator = BossController.instance.IndicateCircle(
           PlayerController.instance.transform.position,
           Quaternion.identity
        );
        indicator.transform.localScale = new Vector3(basesize, basesize, 1);

        float size = basesize;
        float startTime = Time.time;

        while (size > 0f)
        {
            var elapsedTime = Time.time - startTime;
            size = 4 - (Mathf.Clamp01(elapsedTime / (1/speedModifier)) * 4);
            indicator.transform.localScale = new Vector3(size, size, 1);
            yield return null;
        }

        var fs = Instantiate(
            GameManager.instance.getBulletPrefab("Flame Strike"),
            indicator.transform.position,
            indicator.transform.rotation
        );
        fs.transform.localScale *= sizeModifier;
        BossController.instance.RemoveIndicator(indicator);
        yield return new WaitForSeconds(.25f);
        PhaseChange();
    }
    
    /// Shoots a circle of spaced out flames around Blag a few times. 
    IEnumerator Radial_Blast(float speedModifier)
    {
        //How many bullets should be fired
        //NOTE: One spread will have one less or one more
        float blastcount = 10;

        //Angle in radians between bullets
        float blastCountScaled = (Mathf.PI / (blastcount));

        //Starting angle in radians
        float startingAngle = (Mathf.PI / 2) + (blastCountScaled/2);

        //Ending angle in radians
        float endingAngle = 3 * startingAngle - blastCountScaled;

        //Creates the indicators for the attack
        for (float i = startingAngle; i <= endingAngle; i += blastCountScaled)
        {
            var indicator = BossController.instance.Indicate(
                new Vector3(transform.position.x, transform.position.y, 1),
                Quaternion.Euler(0, 0, i * Mathf.Rad2Deg)
            );
            indicator.transform.localScale = new Vector3(1, 20, 1);
            indicator.transform.position += indicator.transform.up * 10;
        }
        
        //Breathing time
        yield return new WaitForSeconds(1.3f/speedModifier);
        
        //Remove indicators and fire bullets
        BossController.instance.removeAllIndicators();
        for (float i = startingAngle; i <= endingAngle; i += blastCountScaled)
        {
            GameObject bullet = Instantiate(
                GameManager.instance.getBulletPrefab("Radial Blast"),
                transform.position,
                Quaternion.Euler(0, 0, i * Mathf.Rad2Deg)
            );

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(bullet.transform.up * 12f * speedModifier, ForceMode2D.Impulse);
        }

        //Angle offset in radians of the attack
        float offset = blastCountScaled / 2;

        //Create indicators for the next wave with the correct offset
        for (float i = startingAngle + offset; i <= endingAngle - offset; i += blastCountScaled)
        {
            var indicator = BossController.instance.Indicate(
                new Vector3(transform.position.x, transform.position.y, 1),
                Quaternion.Euler(0, 0, i * Mathf.Rad2Deg)
            );
            indicator.transform.localScale = new Vector3(1, 20, 1);
            indicator.transform.position += indicator.transform.up * 10;
        }
        
        //Breathing time
        yield return new WaitForSeconds(1.3f/speedModifier);

        //Remove indicators for the next wave and fire the bullets
        BossController.instance.removeAllIndicators();
        for (float i = startingAngle + offset; i <= endingAngle - offset; i += blastCountScaled)
        {
            GameObject bullet = Instantiate(
                GameManager.instance.getBulletPrefab("Radial Blast"),
                transform.position,
                Quaternion.Euler(0, 0, i * Mathf.Rad2Deg)
            );

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(bullet.transform.up * 12f * speedModifier, ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(.5f);
        PhaseChange();
    }
    
    /// Does the death animation and kills Blag.
    IEnumerator Death()
    {
        //Change animation state and wait for it to finish
        m_Animator.SetTrigger("Death");
        yield return new WaitUntil(() => m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Death"));
        yield return new WaitWhile(() => m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        
        //Get all body parts
        GameObject bodyUncovered = transform.Find("Blag Body Uncovered").gameObject;
        GameObject rightArmUncovered = transform.Find("Right Arm").Find("Blag_Right_Arm_Uncovered").gameObject;
        GameObject leftArmUncovered = transform.Find("Left Arm").Find("Blag_Left_Arm_Uncovered").gameObject;

        //Get the color component of all body parts
        Color buc = bodyUncovered.GetComponent<SpriteRenderer>().color;
        Color ruc = rightArmUncovered.GetComponent<SpriteRenderer>().color;
        Color luc = leftArmUncovered.GetComponent<SpriteRenderer>().color;

        //Slowly turn them black
        for (float alpha = 1f; alpha >= 0; alpha -= 0.01f)
        {
            ruc.r = alpha;
            ruc.g = alpha;
            ruc.b = alpha;
            rightArmUncovered.GetComponent<SpriteRenderer>().color = ruc;

            luc.r = alpha;
            luc.g = alpha;
            luc.b = alpha;
            leftArmUncovered.GetComponent<SpriteRenderer>().color = luc;

            buc.r = alpha;
            buc.g = alpha;
            buc.b = alpha;
            bodyUncovered.GetComponent<SpriteRenderer>().color = buc;
            yield return new WaitForSeconds(.02f);
        }
        //Cool effects
        Instantiate(deathParticles, transform.position, transform.rotation);

        //Kill boss
        BossController.instance.BossDie();
        Destroy(gameObject);
    }
    
    /// Coordinates the different attacks that Blag can do.
    public void PhaseChange()
    {
        //Check to make sure carcinization should happen
        if (CurrentHealth <= MaxHealth / 2 && !m_carcinized && GameManager.instance.getCurrentDifficultyInt() > 0)
        {
            StartCoroutine(Carcinization());
            m_DifficultyModifier *= 1.5f;
            m_LevelModifier *= 1.5f;
        }
        //Choose random attack based on difficulty
        else
        {
            int r = Random.Range(0, m_MaxAttack);

            switch (r)
            {
                case 0:
                    StartCoroutine(Firebolt(m_DifficultyModifier, m_LevelModifier));
                    break;
                case 1:
                    StartCoroutine(Cinder_Cluster(m_DifficultyModifier, m_LevelModifier));
                    break;
                case 2:
                    StartCoroutine(Pinch());
                    break;
                case 3:
                    StartCoroutine(Flame_Strike(m_DifficultyModifier, m_LevelModifier));
                    break;
                case 4:
                    StartCoroutine(Radial_Blast(m_DifficultyModifier));
                    break;
                default:
                    PhaseChange();
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

    public override void Die()
    {
        Invulnerable = true;
        StopAllCoroutines();
        BossController.instance.removeAllIndicators();
        transform.Find("Blag Body Uncovered").GetComponent<SpriteRenderer>().color = Color.white;
        transform.Find("Right Arm").Find("Blag_Right_Arm_Uncovered").GetComponent<SpriteRenderer>().color = Color.white;
        transform.Find("Left Arm").Find("Blag_Left_Arm_Uncovered").GetComponent<SpriteRenderer>().color = Color.white;
        StartCoroutine(Death());
    }

    public void ForceDeath()
    {
        StopAllCoroutines();
        BossController.instance.removeAllIndicators();
        BossController.instance.BossDie();
        Destroy(gameObject);
    }
}

