using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlagClaw : MonoBehaviour
{
    public int Damage;
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.gameObject.GetComponent<IHealth>().takeDamage(Damage);
    }
}
