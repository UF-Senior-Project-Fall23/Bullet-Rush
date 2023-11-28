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
    public GameObject pedestals;

    public static Dictionary<string, Perk> perks;

    [HideInInspector]
    public UnityEvent<Perk> onAddPerk;

    private List<GameObject> spawnedPerks = new();
    private List<Perk> takenPerks = new();
    private List<Perk> heldPerks = new(); // Differs from taken perks in that this can persist between runs.

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
        Portal.ChangeRoom.AddListener(SpawnPerks);
        perks = Resources.LoadAll<Perk>("Perks").ToDictionary(x => x.name, x => x);
    }

    void addPerk(Perk perk)
    {
        takenPerks.Add(perk);
        heldPerks.Add(perk);
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

    // Does not actually remove the perk from any lists
    void removePerk(Perk perk)
    {
        //Debug.LogWarning($"Removing perk {perk.name}");
        if(perk.type == PerkType.Weapon)
        {
            for(int i = 0; i < perk.amount; i++)
                UpdateStat(PlayerController.instance.weapon.currWeapon.GetComponent<Weapon>(), perk.modifying[i], ToInverse(perk.modifier[i]));
        }
        else
        {
            for (int i = 0; i < perk.amount; i++)
            {
                PlayerController.instance.stats.IncreaseStat(perk.modifying[i], ToInverse(perk.modifier[i]));
                PlayerController.instance.stats.onStatUpdate.Invoke();
            }
        }
    }

    // TODO: Find better way to do this. This is prone to rounding errors over long periods of usage
    private static float ToInverse(float x)
    {
        var inverse = 100 * (1 / (1 + x / 100) - 1) ;
        //Debug.LogWarning($"Input = {x}, Inverse = {inverse}");
        return inverse;
    }
    
    public void ResetHeldPerks()
    {
        //Debug.LogWarning($"It reset the perks. Before, heldPerks = {string.Join(", ", heldPerks)}");
        foreach (var perk in heldPerks)
        {
            removePerk(perk);
        }
        heldPerks.Clear();
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

    public void ResetPerks()
    {
        spawnedPerks.Clear();
        takenPerks.Clear();
    }
    
    public void DespawnPerks()
    {
        foreach(var perk in spawnedPerks)
        {
            Destroy(perk);
        }
        spawnedPerks.Clear();
    }
    
    void SpawnPerks(RoomType from, RoomType to)
    {
        DespawnPerks();
        
        if (to != RoomType.LootRoom) return;
        
        var perksToShow = perks
            .Where(kv => !takenPerks.Contains(kv.Value))
            .OrderBy(x => new System.Random().Next())
            .Take(3).Select(kv => kv.Value).ToList();

        int i = 0;
        foreach (Transform child in pedestals.transform)
        {
            Vector3 pos = child.position;
            pos.y += 1;
            spawnedPerks.Add(SpawnPerk(perksToShow[i++], pos));
        }
    }
}
