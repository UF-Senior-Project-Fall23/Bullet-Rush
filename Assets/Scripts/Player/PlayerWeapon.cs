using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public GameObject currWeapon;
    Weapon m_weaponScript;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameObject facingWeapon = Physics2D.CircleCast(transform.position, 1.5f, transform.up, 0, LayerMask.GetMask("Weapon")).collider?.gameObject;
            if (facingWeapon != null)
                pickupWeapon(facingWeapon);
        }
        
    }

    private void pickupWeapon(GameObject weapon) 
    {
        if(currWeapon != null)
        {
            currWeapon.tag = "Weapon";
            m_weaponScript.player = null;
            m_weaponScript.enabled = false;
            currWeapon.GetComponent<Collider2D>().enabled = true;
            currWeapon.transform.SetPositionAndRotation(m_weaponScript.startingPosition.position, m_weaponScript.startingPosition.rotation);
            currWeapon = null;
        }

        weapon.tag = "CurrentWeapon";
        m_weaponScript = weapon.GetComponent<Weapon>();
        m_weaponScript.player = gameObject;
        m_weaponScript.enabled = true;
        weapon.GetComponent<Collider2D>().enabled = false;
        currWeapon = weapon;
        HUDManager.instance.weaponText.text = "Weapon: " + weapon.name;
    }

}
