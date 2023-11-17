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
    private float m_CurrHP;

    private Animator m_Animator;

    bool m_invulnerable = false;

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

    }
    IEnumerator Pistol_Blast()
    {
        Debug.Log("Pistol_Blast");
        yield return new WaitWhile(() => m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        int yOffsetIndicator = 1;
        int xOffsetIndicator = 1;
        //Get the player postition relative to the boss
        Vector3 playerPos = PlayerController.instance.transform.position - transform.position;
        //Get the angle from the position
        float playerAngle = Mathf.Atan2(playerPos.y, playerPos.x);
        Debug.Log("PLAYER ANGLE" + playerAngle * Mathf.Rad2Deg);

        // if btwn -45 and 45, to the right
        if (playerAngle < (Mathf.PI / 4) && playerAngle > (-1 * Mathf.PI / 4))
        {
            Debug.Log("PISTOL RIGHT");
            m_Animator.SetTrigger("Pistol Blast");
            yOffsetIndicator = 1;
            xOffsetIndicator = 1;
        }
        else if (playerAngle > (Mathf.PI / 4) && playerAngle < (3 * Mathf.PI / 4))
        {
            Debug.Log("Pistol Up");
            m_Animator.SetTrigger("Pistol Up");
            yOffsetIndicator = 2;
            xOffsetIndicator = -1;
        }
        //135, 225 degrees (on sides) to the left
        else if (playerAngle > (3 * Mathf.PI / 4) || playerAngle < (-3 * Mathf.PI / 4))
        {
            Debug.Log("PISTOL LEFT");
            m_Animator.SetTrigger("Pistol Blast");
            yOffsetIndicator = 1;
            xOffsetIndicator = -1;
        }
        else
        {
            Debug.Log("Pistol Down");
            m_Animator.SetTrigger("Pistol Down");
            yOffsetIndicator = 0;
            xOffsetIndicator = 1;
        }


        //6 fast revolver shots
        List<GameObject> indicators = new(1);
        bool indicate = true;

        yield return new WaitForSeconds(1f);
        foreach (var _ in Enumerable.Range(0, 10))
        {
            //Get the player postition relative to the boss
            if (indicate)
            {
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
                rb.AddForce(bullet.transform.right * 45f, ForceMode2D.Impulse);
                indicate = true;
                yield return new WaitForSeconds(.1f);
            }
        }
        yield return new WaitForSeconds(2f);
        m_Animator.SetTrigger("Idle");
        PhaseChange();
    }
    IEnumerator Dual_Danger()
    {
        //Short Range Shotgun
        List<GameObject> indicators = new(4);
        foreach (var _ in Enumerable.Range(0, 3))
        {
            //Get the player postition relative to the boss
            Vector3 playerPos = PlayerController.instance.transform.position - transform.position;
            //Get the angle from the position
            float playerAngle = Mathf.Atan2(playerPos.y, playerPos.x);

            GameObject indicator = Instantiate(
                BossController.instance.inidcatorSmallPrefab,
                new Vector3(transform.position.x, transform.position.y, 1) + (playerPos / 2),
                Quaternion.Euler(0, 0, playerAngle * Mathf.Rad2Deg + 90)
            );

            indicator.transform.localScale = new Vector3(1, playerPos.magnitude, 1);
            indicators.Add(indicator);
            yield return new WaitForSeconds(.1f);
        }

        foreach (var indicator in indicators)
        {
            //Fire 10 bullet at the player based on its position
            foreach (var _ in Enumerable.Range(0, 9))
            {
                Vector3 randomOffset = Random.insideUnitSphere * 2.0f;
                GameObject bullet = Instantiate(
                    GameManager.instance.getBulletPrefab("Test Bullet"),
                    transform.position - ((transform.position - indicator.transform.position).normalized * 5f) + randomOffset,
                    indicator.transform.rotation
                );
                bullet.transform.Rotate(0, 0, 270);
                Destroy(indicator);

                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb.AddForce(bullet.transform.right * 12f, ForceMode2D.Impulse);
            }
        }

        yield return new WaitForSeconds(1f);
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
        int r = 0;
        //int r = Random.Range(0, 2);

        if (r == 0)
            StartCoroutine(Pistol_Blast());
        else
            StartCoroutine(Dual_Danger());
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

