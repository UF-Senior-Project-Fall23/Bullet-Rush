using UnityEngine;
using UnityEngine.Events;

// Represents an object that has health and can be damaged.
public abstract class Damageable : MonoBehaviour
{

    private float _mCurrHP;
    private float _mMaxHP;
    private bool _mInvulnerable;
    

    // Adjusts the maximum health of the entity.
    public float MaxHealth
    {
        get => _mMaxHP;
        set
        {
            _mMaxHP = value;
            HPChange.Invoke(CurrentHealth, value);
        }
    }
    
    // Adjusts the current health of the entity, fixed between 0 and the maximum.
    public float CurrentHealth
    {
        get => _mCurrHP;
        set
        {
            _mCurrHP = Mathf.Clamp(value, 0f, MaxHealth);
            HPChange.Invoke(_mCurrHP, MaxHealth);
        }
    }
    
    // Adjusts whether the entity is currently invulnerable.
    public bool Invulnerable { get => _mInvulnerable; set => _mInvulnerable = value; }

    GameObject gameObject { get; }
    
    // Events which can be accessed externally to check when a Damageable entity changes its health or takes damage.
    [HideInInspector] public UnityEvent<float> TakeDamage = new();
    [HideInInspector] public UnityEvent<float, float> HPChange = new();
    
    // Causes an entity to take damage, accounting for invulnerability.
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

    // Every damageable entity requires code handling its death, as it is called automatically by takeDamage().
    public abstract void Die();
}
