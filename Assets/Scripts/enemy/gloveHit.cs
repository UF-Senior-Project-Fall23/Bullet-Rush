using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gloveHit : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController.instance.GetComponent<Damageable>()?.takeDamage(1);
        }

    }
}
