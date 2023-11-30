using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerHealth : Damageable
{
    public float baseMaxHP;

    void Awake()
    {
        MaxHealth = baseMaxHP;
        CurrentHealth = baseMaxHP;
        
        PlayerController.instance.stats.onStatUpdate.AddListener(UpdateMaxHPStat);
        HPChange.AddListener(UpdateHPBar);
        HPChange.AddListener(UpdateHealthStat);
        Portal.EnterLootRoom.AddListener(LootRoomHeal);
    }

    public override void Die()
    {
        Destroy(gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Runs exclusively when the PlayerStats's Max HP manager changes.
    public void UpdateMaxHPStat()
    {
        var oldMaxHP = MaxHealth;
        MaxHealth = PlayerController.instance.stats.GetStat("Health").value;
        CurrentHealth += MaxHealth - oldMaxHP; // Heal or damage based on HP change.
    }

    public void UpdateHPBar(float current, float max)
    {
        Debug.LogWarning($"Setting Player HP Bar to {current}/{max}");
        FillableBar.AllBars["Player"].SetFill(current, max);
    }

    void UpdateHealthStat(float current, float max)
    {
        PlayerController.instance.stats.SetStat("Health", max);
    }

    void LootRoomHeal()
    {
        CurrentHealth += Mathf.Round(CurrentHealth * 0.33f);
    }

}
