using UnityEngine;

// Manages picking up weapons on pedestals.
public class WeaponPickup : MonoBehaviour
{
    public string title;
    public string description;
    public Vector3 offset;
    
    // Show tooltip when nearby. 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HUDManager.instance.ShowTooltip(title, description, transform.position + offset);
    }

    // Pick up a weapon when you press Q near it.
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKey(KeyCode.Q))
        {
            PlayerController.instance.weapon.pickupWeapon(gameObject);
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }

    // Hide tooltip when moving away.
    private void OnTriggerExit2D(Collider2D collision)
    {
        HUDManager.instance.HideTooltip();
    }

}
