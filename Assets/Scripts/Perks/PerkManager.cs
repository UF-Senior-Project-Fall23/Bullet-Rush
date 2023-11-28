using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class PerkManager : MonoBehaviour
{
    public static PerkManager instance;
    public GameObject perkPreFab;

    Dictionary<string, Perk> perks;

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
        perks = Resources.LoadAll<Perk>("Perks").ToDictionary(x => x.name, x => x);
    }

    void addPerk(Perk perk)
    {
        if(perk.type == PerkType.Weapon)
        {
            for(int i = 0; i < perk.amount; i++)
                UpdateStat(PlayerController.instance.weapon.currWeapon.GetComponent<Weapon>(), perk.modifying[i], perk.modifier[i]);
        }
        else
        {
            for (int i = 0; i < perk.amount; i++)
            {
                PlayerController.instance.stats.IncreaseStat(perk.modifying[i], perk.modifier[i]);
                PlayerController.instance.stats.onStatUpdate.Invoke();
            }
        }
    }
    
    public void UpdateStat<T>(T instance, string stat, float value)
    {
        FieldInfo field = instance.GetType().GetField(stat);
        field?.SetValue(instance, (float)field.GetValue(instance) * (1+(value/100)));
    }

    public GameObject SpawnPerk(Perk perk, Vector3 position)
    {
        GameObject newPerk = Instantiate(perkPreFab, position, Quaternion.identity);
        newPerk.GetComponent<PerkPickup>().SetPerk(perk);
        return newPerk;
    }
}
