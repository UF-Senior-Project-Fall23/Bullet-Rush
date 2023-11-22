using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class PerkManager : MonoBehaviour
{
    public static PerkManager instance;

    [HideInInspector]
    public UnityEvent<Perk> onAddPerk;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        onAddPerk.AddListener(addPerk);
    }

    void addPerk(Perk perk)
    {
        if(perk.type == PerkType.Weapon)
        {
            UpdateStat(PlayerController.instance.weapon.currWeapon.GetComponent<Weapon>(), perk.modifying, perk.modifier);
        }
        else
        {
            PlayerController.instance.stats.IncreaseStat(perk.modifying, perk.modifier);
            PlayerController.instance.stats.onStatUpdate.Invoke();
        }
    }
    
    public void UpdateStat<T>(T instance, string stat, float value)
    {
        FieldInfo field = instance.GetType().GetField(stat);
        field?.SetValue(instance, (float)field.GetValue(instance) + value);
    }

}
