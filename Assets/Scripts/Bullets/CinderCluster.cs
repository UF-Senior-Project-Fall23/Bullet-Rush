using UnityEngine;

// Represents a projectile that bursts into smaller projectiles after exploding. Used for Onyx and Blag'thoroth.
public class CinderCluster : MonoBehaviour
{
    public int ShardCount = 8;
    void OnDestroy()
    {
        for(float i = 0; i <= 2*Mathf.PI; i += Mathf.PI/(ShardCount/2))
        {
            GameObject bullet = Instantiate(
                GameManager.instance.getBulletPrefab("Cinder Cluster Shard"),
                transform.position,
                Quaternion.Euler(0, 0, i * Mathf.Rad2Deg)
            );

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(bullet.transform.up * 20f, ForceMode2D.Impulse);
        }
    }
}
