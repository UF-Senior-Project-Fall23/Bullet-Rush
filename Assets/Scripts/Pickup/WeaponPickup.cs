using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public string title;
    public string description;
    public Vector3 offset;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        HUDManager.instance.ShowTooltip(title, description, transform.position + offset);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKey(KeyCode.Q))
        {
            PlayerController.instance.weapon.pickupWeapon(gameObject);
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        HUDManager.instance.HideTooltip();
    }

}
