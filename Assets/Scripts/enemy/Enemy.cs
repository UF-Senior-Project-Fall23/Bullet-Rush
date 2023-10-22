using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IHealth
{
    public float speed = 1.0f;
    public float MaxHP = 5.0f;
    private float m_CurrHP;

    public float MaxHealth { get => MaxHP; set => MaxHP = value; }
    public float CurrentHealth { get => m_CurrHP; set => m_CurrHP = value; }

    void Start()
    {
        m_CurrHP = MaxHP;
    }

    public void Die()
    {
        Destroy(gameObject);
        GameManager.instance.AddScore(1);
    }
}
