using UnityEngine;

public class PlayerHealth : Damageable
{
    public float baseMaxHP;
    
    private SpriteRenderer spriteRenderer;
    private bool invulFlashOn = false;
    private float timeBetweenFlashes = 0.125f;
    private float lastFlashTime;
    private float invulTime;

    void Awake()
    {
        MaxHealth = baseMaxHP;
        CurrentHealth = baseMaxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        PlayerController.instance.stats.onStatUpdate.AddListener(UpdateMaxHPStat);
        HPChange.AddListener(UpdateHPBar);
        HPChange.AddListener(UpdateHealthStat);
        TakeDamage.AddListener(DamageInvFrames);
        Portal.EnterLootRoom.AddListener(LootRoomHeal);
    }

    void Update()
    {
        if (Invulnerable)
        {
            InvulFlashHandler();
        }
    }

    public override void Die()
    {
        GetComponent<PlayerController>().weapon.DropWeapon();
        HUDManager.instance.ShowDeathScreen();
        HUDManager.instance.transform.Find("PlayerHealthFrame")?.gameObject.SetActive(false);
        FindObjectOfType<InterpPlayerAim>().enabled = false;
        gameObject.SetActive(false);
        BossController.instance.ForceBossDie();
    }

    public void Respawn()
    {
        gameObject.SetActive(true);
        CurrentHealth = baseMaxHP;
        Invulnerable = false;
        HPChange.Invoke(CurrentHealth, baseMaxHP);
        HUDManager.instance.HideDeathScreen();
        HUDManager.instance.transform.Find("PlayerHealthFrame")?.gameObject.SetActive(true);
        PlayerController.instance.movement.ResetMovement();
        FindObjectOfType<InterpPlayerAim>().enabled = true;
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
        CurrentHealth += Mathf.Round(MaxHealth * 0.33f);
    }

    void DamageInvFrames(float damage)
    {
        SetInvulFrames(0.4f);
    }

    public void SetInvulFrames(float seconds)
    {
        Invulnerable = true;
        if (seconds <= invulTime - Time.time) return; // Only override if the new IFrame time is longer
        if (Time.time >= invulTime)
        {
            invulFlashOn = true;
            spriteRenderer.color = new Color(255, 255, 255, 0.5f);
            lastFlashTime = Time.time;
        }
        invulTime = lastFlashTime + seconds;
    }

    void InvulFlashHandler()
    {
        if (Time.time - lastFlashTime >= timeBetweenFlashes)
        {
            invulFlashOn = !invulFlashOn;
            spriteRenderer.color = new Color(255, 255, 255, invulFlashOn ? 0.5f : 255);
            lastFlashTime = Time.time;
        }

        if (Time.time >= invulTime)
        {
            Invulnerable = false;
            invulFlashOn = false;
            spriteRenderer.color = new Color(255, 255, 255, 255);
        }
    }

}
