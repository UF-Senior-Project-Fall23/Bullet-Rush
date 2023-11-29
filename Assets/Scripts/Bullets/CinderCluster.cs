using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
