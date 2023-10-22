using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    bool m_alive = true;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Bullet hit " + collision.gameObject.name);

        collision.GetComponent<IHealth>()?.takeDamage(damage);
        
        if (!collision.gameObject.CompareTag("CurrentWeapon") && !collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
            m_alive = false;
        }
        
    }

}
