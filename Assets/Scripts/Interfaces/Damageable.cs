using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Damageable : MonoBehaviour
{

    private float _mCurrHP;
    private float _mMaxHP;
    private bool _mInvulnerable;
    
    public float MaxHealth
    {
        get => _mMaxHP;
        set
        {
            _mMaxHP = value;
            HPChange.Invoke(CurrentHealth, value);
        }
    }
    
    public float CurrentHealth
    {
        get => _mCurrHP;
        set
        {
            _mCurrHP = Mathf.Clamp(value, 0f, MaxHealth);
            HPChange.Invoke(_mCurrHP, MaxHealth);
        }
    }
    public bool Invulnerable { get => _mInvulnerable; set => _mInvulnerable = value; }

    GameObject gameObject { get; }
    
    [HideInInspector] public UnityEvent<float> TakeDamage = new();
    [HideInInspector] public UnityEvent<float, float> HPChange = new();
    
    
    public void takeDamage(float damage)
    {
        if (!Invulnerable)
        {
            CurrentHealth -= damage;

            if (CurrentHealth <= 0)
                Die();
        }
        TakeDamage.Invoke(damage);
    }

    public abstract void Die();
}
