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
        collision.GetComponentInParent<IHealth>()?.takeDamage(damage);
        
        Destroy(gameObject);
        m_alive = false;
    }

}
