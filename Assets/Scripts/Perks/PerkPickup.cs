using UnityEngine;

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
    
    public void FixedUpdate()
    {
        float x = gameObject.transform.position.x;
        float y = startY + 0.25f + Mathf.Sin(2 * GameManager.gameTime) / 4f;
        gameObject.transform.position = new Vector3(x, y, 0);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        HUDManager.instance.ShowTooltip(title, perk.description, transform.position + offset);
    }

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

    private void OnTriggerExit2D(Collider2D collision)
    {
        HUDManager.instance.HideTooltip();
    }

    public void SetPerk(Perk perk)
    {
        this.perk = perk;
        GetComponent<SpriteRenderer>().sprite = perk.sprite;
        title = perk.name;
    }

    private void OnDestroy()
    {
        Instantiate(sparkle, transform.position, transform.rotation);
    }
}
