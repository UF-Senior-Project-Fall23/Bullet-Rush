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
    }

    IEnumerator Firebolt()
    {
        List<GameObject> indicators = new(5);

        foreach (var _ in Enumerable.Range(0, 4))
        {
            //Get the player postition relative to the boss
            Vector3 playerPos = PlayerController.instance.transform.position - transform.position;
            //Get the angle from the position
            float playerAngle = Mathf.Atan2(playerPos.y, playerPos.x);

            GameObject indicator = Instantiate(
                BossController.instance.indicatorPrefab,
                new Vector3(transform.position.x, transform.position.y, 1) + (playerPos / 2),
                Quaternion.Euler(0, 0, playerAngle * Mathf.Rad2Deg + 90)
            );

            indicator.transform.localScale = new Vector3(1, playerPos.magnitude, 1);
            indicators.Add(indicator);
            yield return new WaitForSeconds(.2f);
        }


        foreach (var indicator in indicators)
        {
            //Fire a bullet at the player based on its position
            GameObject bullet = Instantiate(
                GameManager.instance.getBulletPrefab("Flame Bolt"),
                transform.position - ((transform.position - indicator.transform.position).normalized * 5f),
                indicator.transform.rotation
            );

            bullet.transform.Rotate(0, 0, 270);
            Destroy(indicator);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(bullet.transform.right * 20f, ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(1f);
        PhaseChange();
    }

    IEnumerator Cinder_Cluster()
    {
        GameObject bulletPreFab = GameManager.instance.getBulletPrefab("Cinder Cluster");

        //Get the player postition relative to the boss
        Vector3 playerPos = PlayerController.instance.transform.position - transform.position;
        //Get the angle from the position
        float playerAngle = Mathf.Atan2(playerPos.y, playerPos.x);

        GameObject indicator = Instantiate(
            BossController.instance.indicatorPrefab,
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

        Destroy(indicator);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(bullet.transform.up * 20f, ForceMode2D.Impulse);


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
        int r = Random.Range(0, 2);

        if (r == 0)
            StartCoroutine(Firebolt());
        else
            StartCoroutine(Cinder_Cluster());
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

