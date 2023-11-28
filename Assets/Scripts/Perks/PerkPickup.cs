using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkPickup : MonoBehaviour
{
    public Perk perk;
    public string title;
    public string description;
    public Vector3 offset;

    public void Start()
    {

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        HUDManager.instance.ShowTooltip(title, description, transform.position + offset);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKey(KeyCode.Q))
        {
            PerkManager.instance.onAddPerk.Invoke(perk);
            gameObject.GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject);
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

}
