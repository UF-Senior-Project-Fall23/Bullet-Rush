using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthBar : MonoBehaviour
{   
    enemyScript entity;
    public GameObject greenBar;
    void Start()
    {
        entity = gameObject.GetComponentInParent<enemyScript>();
        if (entity == null)
        {
            gameObject.SetActive(false);
            return;
        }
    }

    public void takeDamage()
    {
        float healthRatio = entity.health / entity.maxHealth;
        greenBar.transform.localScale = new Vector3(healthRatio, greenBar.transform.localScale.y, greenBar.transform.localScale.z);
        greenBar.transform.localPosition = new Vector3(((1-healthRatio)/2) * -1, greenBar.transform.localPosition.y, greenBar.transform.localPosition.z);
    }
}
