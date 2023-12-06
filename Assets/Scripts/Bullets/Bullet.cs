using UnityEngine;

// Basic code for making bullets deal damage to things.
public class Bullet : MonoBehaviour
{
    public float damage;
    public bool IsPiercing = false;
    public bool IsExplosion = false;
    //bool m_alive = true;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Bullet hit " + collision.gameObject.name);
        if (!collision.gameObject.CompareTag("Bullet") /*&& !collision.gameObject.CompareTag("Player")*/)
        {
            if (collision.GetComponent<Damageable>() != null)
            {
                collision.GetComponent<Damageable>().takeDamage(damage);
            }
            else
            {
                collision.GetComponentInParent<Damageable>()?.takeDamage(damage);
            }

            if (!IsExplosion && (!IsPiercing || collision.gameObject.layer == LayerMask.NameToLayer("Background")))
                Destroy(gameObject);
            //m_alive = false;
        }
        
    }

}
