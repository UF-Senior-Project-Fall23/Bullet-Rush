using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

// Represents statistics held by the player.
// Mostly adjusted through the inspector.
public class PlayerStats : MonoBehaviour
{
    // Set in the inspector, defines the stats the player can hold and which can be modified by perks.
    [SerializeField]
    private List<Stat> stats = new List<Stat>();

    [HideInInspector]
    public UnityEvent onStatUpdate;

    public Stat GetStat(string stat)
    {
        return stats.Where(i => i.name == stat).FirstOrDefault();
    }

    public string[] GetStatNames()
    {
        return stats.Select(i => i.name).ToArray();
    }

    public void SetStat(string stat, float value)
    {
        stats.Where(i => i.name == stat).FirstOrDefault().value = value;
    }

    // Increases a stat by a percentage.
    public void IncreaseStat(string stat, float value)
    {
        stats.Where(i => i.name == stat).FirstOrDefault().value *= (1+(value/100));
    }

    public void ResetStat(string stat)
    {
        stats.Where(i => i.name == stat).FirstOrDefault().ResetStat();
    }

    public void ResetAllStats()
    {
        foreach(var i in stats)
            i.ResetStat();
    }
}
