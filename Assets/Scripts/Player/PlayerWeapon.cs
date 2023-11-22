using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerWeapon : MonoBehaviour
{
    public GameObject currWeapon;
    Weapon m_weaponScript;

    public void pickupWeapon(GameObject weapon) 
    {
        if(currWeapon != null)
        {
            currWeapon.tag = "Weapon";
            m_weaponScript.player = null;
            m_weaponScript.enabled = false;
            currWeapon.transform.SetPositionAndRotation(m_weaponScript.startingPosition.position, m_weaponScript.startingPosition.rotation);
            currWeapon.transform.Find("Pickup").GetComponent<Collider2D>().enabled = true;
            currWeapon = null;
        }

        weapon.tag = "CurrentWeapon";
        m_weaponScript = weapon.GetComponent<Weapon>();
        m_weaponScript.player = gameObject;
        m_weaponScript.enabled = true;
        currWeapon = weapon;
        HUDManager.instance.weaponText.text = "Weapon: " + weapon.name;
    }

}
