using UnityEngine;

/// Makes Blag'thoroth's claws deal contact damage.
public class BlagClaw : MonoBehaviour
{
    public int Damage;
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.gameObject.GetComponent<Damageable>().takeDamage(Damage);
    }
}