using UnityEngine;

// Represents the player's health pool and handles health-related logic.
public class PlayerHealth : Damageable
{
    public float baseMaxHP;
    
    private SpriteRenderer spriteRenderer;
    private bool invulFlashOn = false;
    private float timeBetweenFlashes = 0.125f;
    private float lastFlashTime;
    private float invulTime;

    // Sets max HP to base values, creates listeners for HP.
    void Awake()
    {
        baseMaxHP = PlayerController.instance.stats.GetStat("Health").value;
        MaxHealth = baseMaxHP;
        CurrentHealth = baseMaxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        PlayerController.instance.stats.onStatUpdate.AddListener(UpdateMaxHPStat);
        HPChange.AddListener(UpdateHPBar);
        HPChange.AddListener(UpdateHealthStat);
        TakeDamage.AddListener(DamageInvFrames);
        Portal.EnterLootRoom.AddListener(LootRoomHeal);
    }

    // Flashes the player if they're invulnerable.
    void Update()
    {
        if (Invulnerable)
        {
            InvulFlashHandler();
        }
    }

    // Handles the player dying, showing the death screen.
    public override void Die()
    {
        GetComponent<PlayerController>().weapon.DropWeapon();
        HUDManager.instance.ShowDeathScreen();
        HUDManager.instance.transform.Find("PlayerHealthFrame")?.gameObject.SetActive(false);
        FindObjectOfType<InterpPlayerAim>().enabled = false;
        gameObject.SetActive(false);
        BossController.instance.ForceBossDie();
    }

    // Sends the player to the start and resets their perks and other stats.
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

    // Adjust the HP bar above the player.
    public void UpdateHPBar(float current, float max)
    {
        Debug.LogWarning($"Setting Player HP Bar to {current}/{max}");
        FillableBar.AllBars["Player"].SetFill(current, max);
    }

    // Adjust the player's Health in the Stats module.
    void UpdateHealthStat(float current, float max)
    {
        PlayerController.instance.stats.SetStat("Health", max);
    }

    // Heals the player when entering a loot room.
    void LootRoomHeal()
    {
        float increase = Mathf.Round(MaxHealth * (1.0f / (GameManager.instance.getCurrentDifficultyInt() + 3)));
        Debug.Log($"Increase = {increase}");
        CurrentHealth += increase;
    }

    // Grants IFrames when the player takes damage.
    void DamageInvFrames(float damage)
    {
        SetInvulFrames(0.4f);
    }

    // Grants IFrames to the player. Sets up variables to handle the flashing animation.
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

    // Makes the player flash back and forth in opacity while they have IFrames.
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
