using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CordeliaBullet : MonoBehaviour
{
    public int damage;
    bool m_alive = true;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Bullet hit " + collision.gameObject.name);

        if (!collision.gameObject.CompareTag("Boss") && !collision.gameObject.CompareTag("Bullet") && !collision.gameObject.CompareTag("Clone"))
        {
            Destroy(gameObject);
            m_alive = false;
            collision.GetComponent<Damageable>()?.takeDamage(damage);
        }

    }

}
