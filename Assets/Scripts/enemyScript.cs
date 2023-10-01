using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour
{
    public float maxHealth = 5.0f;
    public float health = 5.0f;
    public float speed = 1.0f;

    public void takeDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
            die();

        gameObject.transform.GetComponentInChildren<healthBar>().takeDamage();
    }

    void die()
    {
        Destroy(gameObject);
    }
}
