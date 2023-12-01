using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Stat", menuName = "Stat")]
public class Stat : ScriptableObject
{
    public new string name;
    public string description;
    public float value;
    [SerializeField]
    float defaultValue;

    private void OnEnable()
    {
        ResetStat();
    }
    
    public void ResetStat() { value = defaultValue; }
}
