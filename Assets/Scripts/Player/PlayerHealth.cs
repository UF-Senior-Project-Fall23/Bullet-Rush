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
        GetComponent<PlayerController>().weapon.DropWeapon();
        HUDManager.instance.ShowDeathScreen();
        HUDManager.instance.transform.Find("PlayerHealthFrame")?.gameObject.SetActive(false);
        FindObjectOfType<InterpPlayerAim>().gameObject.SetActive(false);
        gameObject.SetActive(false);
        BossController.instance.ForceBossDie();
    }

    public void Respawn()
    {
        gameObject.SetActive(true);
        CurrentHealth = baseMaxHP;
        HPChange.Invoke(CurrentHealth, baseMaxHP);
        HUDManager.instance.HideDeathScreen();
        HUDManager.instance.transform.Find("PlayerHealthFrame")?.gameObject.SetActive(true);
        ((GameObject)SceneManager.GetActiveScene().GetRootGameObjects().GetValue(5)).SetActive(true);
        GameManager.instance.GoToStart();
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
