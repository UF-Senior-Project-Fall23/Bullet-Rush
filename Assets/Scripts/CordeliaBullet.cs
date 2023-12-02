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
            if(BossController.instance.currentBoss != null)
            {
                collision.GetComponent<Damageable>()?.takeDamage(damage);
            }
            
        }

    }
    float timeLeft = 0f;
    float timeLast = 3f;
    void Update()
    {
        timeLeft += Time.deltaTime;
        
        if(timeLeft > timeLast)
        {
            Destroy(gameObject);
        }
    }

}
