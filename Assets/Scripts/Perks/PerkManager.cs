using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;

// Handles perk obtaining and generation.
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

    // Set up singleton instance
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

    // Grants the player the given perk.
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

    // Removes the given perk from the player.
    // Does not actually remove the perk from any lists
    void removePerk(Perk perk)
    {
        //Debug.LogWarning($"Removing perk {perk.name}");
        if(perk.type == PerkType.Weapon)
        {
            for(int i = 0; i < perk.amount; i++)
                PlayerController.instance.weapon.currWeapon?.GetComponent<Weapon>().ResetStat(perk.modifying[i]);
        }
        else
        {
            for (int i = 0; i < perk.amount; i++)
            {
                PlayerController.instance.stats.ResetStat(perk.modifying[i]);
                PlayerController.instance.stats.onStatUpdate.Invoke();
            }
        }
    }
    
    // Removes all the perks held by the player.
    public void ResetHeldPerks()
    {
        //Debug.LogWarning($"It reset the perks. Before, heldPerks = {string.Join(", ", heldPerks)}");
        foreach (var perk in heldPerks)
        {
            removePerk(perk);
        }
        heldPerks.Clear();
    }
    
    // Changes a variable in the given object, used for updating player or weapon statistics.
    // TODO: Probably refactor this to not change the literal variable.
    public void UpdateStat<T>(T statsInstance, string stat, float value)
    {
        FieldInfo field = statsInstance.GetType().GetField(stat);
        field?.SetValue(statsInstance, (float)field.GetValue(statsInstance) * (1+(value/100)));
    }

    // Summons a perk item at the given position with the given type.
    public GameObject SpawnPerk(Perk perk, Vector3 position)
    {
        GameObject newPerk = Instantiate(perkPreFab, position, Quaternion.identity);
        newPerk.GetComponent<PerkPickup>().SetPerk(perk);
        return newPerk;
    }

    // Clears the lists of spawned and taken perks.
    public void ResetPerks()
    {
        spawnedPerks.Clear();
        takenPerks.Clear();
    }
    
    // Removes every spawned in perk object.
    public void DespawnPerks()
    {
        foreach(var perk in spawnedPerks)
        {
            Destroy(perk);
        }
        spawnedPerks.Clear();
    }
    
    // Generates perk objects at the pedestals in a loot room.
    void SpawnPerks(RoomType from, RoomType to)
    {
        DespawnPerks();
        
        if (to != RoomType.LootRoom) return;
        
        // Pick perks that haven't been taken this run.
        var perksToShow = perks
            .Where(kv => !takenPerks.Contains(kv.Value))
            .OrderBy(x => new Random().Next())
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
