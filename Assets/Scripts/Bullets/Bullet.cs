using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    bool m_alive = true;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Bullet hit " + collision.gameObject.name);

        if(collision.GetComponent<IHealth>() != null)
            collision.GetComponent<IHealth>().takeDamage(damage);
        else
            collision.GetComponentInParent<IHealth>()?.takeDamage(damage);
        
        Destroy(gameObject);
        m_alive = false;
    }

}
