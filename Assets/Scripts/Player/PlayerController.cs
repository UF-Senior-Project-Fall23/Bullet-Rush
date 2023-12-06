using UnityEngine;

// Represents the player.
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public PlayerHealth health;
    public PlayerStats stats;
    public PlayerMovement movement;
    public PlayerWeapon weapon;

    // Set up submodules and singleton instance.
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            health = GetComponent<PlayerHealth>();
            stats = GetComponent<PlayerStats>();
            movement = GetComponent<PlayerMovement>();
            weapon = GetComponent<PlayerWeapon>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
