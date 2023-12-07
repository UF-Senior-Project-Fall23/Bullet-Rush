using UnityEngine;

// Represents a statistic that can be held by the player.
// Used for scripting in the editor.
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
