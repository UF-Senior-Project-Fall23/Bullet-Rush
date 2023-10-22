using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilCube : MonoBehaviour, Boss, IHealth
{
    public GameObject bulletPreFab;

    public float MaxHP = 5.0f;
    private float m_CurrHP;

    public float MaxHealth { get => MaxHP; set => MaxHP = value; }
    public float CurrentHealth { get => m_CurrHP; set => m_CurrHP = value; }

    public string[] Attacks { get; } =
    {
        "Attack", "Invulnerable", "Vulnerable"
    };

    void Start()
    {
        m_CurrHP = MaxHP;
    }

    IEnumerator Attack()
    {
        //Get the player postition relative to the boss
        Vector3 playerPos = PlayerController.instance.transform.position - transform.position;
        //Get the angle from the position
        float playerAngle = Mathf.Atan2(playerPos.y, playerPos.x);
        //Fire a bullet at the player based on its position
        GameObject bullet = Instantiate(bulletPreFab, new Vector3(
                transform.position.x + (transform.localScale.x * Mathf.Cos(playerAngle)),
                transform.position.y + (transform.localScale.y * Mathf.Sin(playerAngle)),
                1), transform.rotation);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.ClampMagnitude((Vector2)playerPos, 1) * 20f, ForceMode2D.Impulse);
        yield return new WaitForSeconds(.5f);
        StartCoroutine(Attack());
    }

    IEnumerator Invulnerable()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(Attack());
    }

    IEnumerator Vulnerable()
    {
        yield return null;
    }

    IEnumerator Death()
    {
        Color c = GetComponent<SpriteRenderer>().color;
        for (float alpha = 1f; alpha >= 0; alpha -= 0.01f)
        {
            c.a = alpha;
            GetComponent<SpriteRenderer>().color = c;
            yield return null;
        }
        BossController.instance.BossDie();
        Destroy(gameObject);
    }

    IEnumerator Boss.StartPhase()
    {
        StartCoroutine(Invulnerable());
        yield return null;
    }

    void Boss.PhaseChange()
    {

    }

    public void Die()
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        StopAllCoroutines();
        StartCoroutine(Death());
    }
}
