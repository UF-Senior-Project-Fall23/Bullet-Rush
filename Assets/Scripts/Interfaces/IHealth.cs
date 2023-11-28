using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public bool Invulnerable { get; set; }

    GameObject gameObject { get; }
    public virtual void takeDamage(float damage)
    {
        if (!Invulnerable)
        {
            CurrentHealth -= damage;

            if (CurrentHealth <= 0)
                Die();

            gameObject.transform.GetComponentInChildren<healthBar>()?.takeDamage();
        }

        gameObject.GetComponent<PlayerHealth>()?.PlayerHealthChanged.Invoke();
    }

    void Die();
}
