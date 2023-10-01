using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    public int damage;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Weapon")
            Destroy(gameObject);

        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Bullet HIT, damage = " + damage);
            collision.gameObject.GetComponent<enemyScript>().takeDamage(damage);
        }
    }

}
