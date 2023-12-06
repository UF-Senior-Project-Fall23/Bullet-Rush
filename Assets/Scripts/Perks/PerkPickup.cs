using UnityEngine;

// Manages perk pickups on pedestals
public class PerkPickup : MonoBehaviour
{
    public GameObject sparkle;
    public Perk perk;
    public string title;
    public Vector3 offset;

    private float startY;

    public void Start()
    {
        startY = gameObject.transform.position.y;
    }
    
    // Bobs up and down while on a pedestal.
    public void FixedUpdate()
    {
        float x = gameObject.transform.position.x;
        float y = startY + 0.25f + Mathf.Sin(2 * GameManager.gameTime) / 4f;
        gameObject.transform.position = new Vector3(x, y, 0);
    }

    // Show the perk description as a tooltip when nearby.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HUDManager.instance.ShowTooltip(title, perk.description, transform.position + offset);
    }

    // Pick up the perk when you press Q near it.
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKey(KeyCode.Q))
        {
            PerkManager.instance.onAddPerk.Invoke(perk);
            gameObject.GetComponent<Collider2D>().enabled = false;
            PerkManager.instance.DespawnPerks();
            Portal.MakeBossPortal.Invoke();
        }
    }

    // Hide tooltip when moving away.
    private void OnTriggerExit2D(Collider2D collision)
    {
        HUDManager.instance.HideTooltip();
    }

    // Sets the type of the perk pickup.
    public void SetPerk(Perk perk)
    {
        this.perk = perk;
        GetComponent<SpriteRenderer>().sprite = perk.sprite;
        title = perk.name;
    }

    // Creates a particle effect when destroyed.
    private void OnDestroy()
    {
        Instantiate(sparkle, transform.position, transform.rotation);
    }
}
