using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkGiver : MonoBehaviour
{
    public Perk perk;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && perk != null)
            PerkManager.instance.onAddPerk.Invoke(perk);

        Destroy(gameObject);
    }
}
