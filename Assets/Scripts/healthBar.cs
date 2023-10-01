using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthBar : MonoBehaviour
{
    enemyScript entity;
    public GameObject greenBar;

    private Transform parentTransform;

    public float healthBarRelativeY;
    void Start()
    {
        entity = gameObject.GetComponentInParent<enemyScript>();
        if (entity == null)
        {
            gameObject.SetActive(false);
            return;
        }
        else
        {
            parentTransform = transform.parent;
        }
    }

    public void takeDamage()
    {
        float healthRatio = entity.health / entity.maxHealth;
        Debug.Log("healthRatio = " + healthRatio);
        greenBar.transform.localScale = new Vector3(healthRatio, greenBar.transform.localScale.y, greenBar.transform.localScale.z);
        greenBar.transform.localPosition = new Vector3(((1 - healthRatio) / 2) * -1, greenBar.transform.localPosition.y, greenBar.transform.localPosition.z);
    }

    private void Update()
    {
        Vector3 healthBarPosition = parentTransform.position + new Vector3(0f, healthBarRelativeY, 0f);
        transform.position = healthBarPosition;
    }
}
