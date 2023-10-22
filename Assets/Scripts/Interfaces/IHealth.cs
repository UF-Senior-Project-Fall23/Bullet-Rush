using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    GameObject gameObject { get; }
    public void takeDamage(int damage)
    {
        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
            Die();

        gameObject.transform.GetComponentInChildren<healthBar>()?.takeDamage();
    }

    void Die();
}
