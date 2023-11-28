using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, IHealth
{
    //Is Invulnerable
    bool m_invulnerable = false;

    float oldMaxHP;

    //Player current health
    float m_currHealth;

    public float MaxHealth { get => PlayerController.instance.stats.GetStat("Health").value; set => PlayerController.instance.stats.SetStat("Health", value); }
    public float CurrentHealth { get => m_currHealth; set => m_currHealth = value; }
    public bool Invulnerable { get => m_invulnerable; set => m_invulnerable = value; }

    [HideInInspector]
    public UnityEvent PlayerHealthChanged;

    void Awake()
    {
        m_currHealth = MaxHealth;
        oldMaxHP = MaxHealth;
        PlayerController.instance.stats.onStatUpdate.AddListener(UpdateMaxHP);
        PlayerHealthChanged.AddListener(HPChange);
    }

    public void Die()
    {
        Destroy(gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void UpdateMaxHP()
    {
        CurrentHealth += MaxHealth - oldMaxHP;
        oldMaxHP = MaxHealth;
        PlayerHealthChanged.Invoke();
    }

    public void HPChange()
    {
        FillableBar.AllBars["Player"].SetFill(CurrentHealth, MaxHealth);
    }

}
