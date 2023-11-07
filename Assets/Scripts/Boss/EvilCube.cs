using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EvilCube : MonoBehaviour, Boss, IHealth
{
    public GameObject bulletPreFab;
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
        "Shoot", "Rage", "TransitionToRage", "RageShoot", "Death"
    };

    void Start()
    {
        m_CurrHP = MaxHP;
        m_MaxBulletVelocity = 40f;
    }

    IEnumerator Shoot(float delay)
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

        float distance = Vector3.Distance(transform.position, PlayerController.instance.transform.position);
        Debug.Log("Distance = " + distance);
        float distanceOffset = Math.Clamp(distance / 20, 0.5f, 1);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.ClampMagnitude((Vector2)playerPos + Vector2.ClampMagnitude(PlayerController.instance.GetComponent<Rigidbody2D>().velocity, 5), 1) * (m_MaxBulletVelocity * distanceOffset), ForceMode2D.Impulse);
        
        yield return new WaitForSeconds(delay);
        PhaseChange();
    }

    IEnumerator TransitionToRage()
    {
        Invulnerable = true;
        Color c = GetComponent<SpriteRenderer>().color;
        for (float red = 0; red <= 1; red += 0.005f)
        {
            c.r = red;
            c.b = 1 - red;
            GetComponent<SpriteRenderer>().color = c;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        Invulnerable = false;
        StartCoroutine(Rage());
    }

    IEnumerator Rage()
    {
        float rangeMax = 5f;
        float rangeMin = 2f;

        //Generates a random value in bewtween rangeMax and rangeMin
        float randX = Random.Range(-rangeMax, rangeMax);
        while (randX > -rangeMin && randX < rangeMin)
            randX = Random.Range(-rangeMax, rangeMax);

        float randY = Random.Range(-rangeMax, rangeMax);
        while (randY > -rangeMin && randY < rangeMin)
            randY = Random.Range(-rangeMax, rangeMax);

        //Teleports boss at this random position
        transform.position = (Vector2)PlayerController.instance.transform.position + new Vector2(randX, randY);
        yield return new WaitForSeconds(.5f);
        StartCoroutine(RageShoot(5));
    }

    IEnumerator RageShoot(int bullets)
    {
        for(int i = 0; i < bullets; i++)
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
            rb.AddForce(Vector2.ClampMagnitude((Vector2)playerPos, 1) * 25f, ForceMode2D.Impulse);
            yield return new WaitForSeconds(.1f);
        }

        yield return new WaitForSeconds(.5f);
        StartCoroutine(Rage());
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
        BossController.instance.BossDie(transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public void PhaseChange()
    {
        if (CurrentHealth <= MaxHealth / 2)
            StartCoroutine(TransitionToRage());
        else
            StartCoroutine(Shoot(0.5f));
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
        gameObject.GetComponent<Collider2D>().enabled = false;
        StopAllCoroutines();
        StartCoroutine(Death());
    }
}
