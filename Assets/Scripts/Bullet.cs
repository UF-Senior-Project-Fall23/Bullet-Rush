using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    bool alive = true;

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Bullet hit " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<enemyScript>().takeDamage(damage);
        }
        
        if (!collision.gameObject.CompareTag("CurrentWeapon") && !collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
            alive = false;
        }
        
    }

}
