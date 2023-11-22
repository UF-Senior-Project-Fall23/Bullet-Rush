using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public enum PerkType
{
    Weapon,
    Player
}

[CreateAssetMenu(fileName = "New Perk", menuName = "Perk")]
public class Perk : ScriptableObject
{
    [Tooltip("What does this perk modify")]
    public PerkType type;

    public int selected;

    [Tooltip("What stat this perk modifies")]
    public string modifying;

    [Tooltip("How much of the stat it modifies")]
    public float modifier;
}

[CustomEditor(typeof(Perk))]
public class PerkEditor : Editor
{
    private SerializedProperty type;
    private SerializedProperty modifying;
    private SerializedProperty modifier;
    private SerializedProperty selected;
    private string[] options;

    private void OnEnable()
    {
        modifying = serializedObject.FindProperty("modifying");
        modifier = serializedObject.FindProperty("modifier");
        type = serializedObject.FindProperty("type");
        selected = serializedObject.FindProperty("selected");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(type);
        if (EditorGUI.EndChangeCheck())
            selected.intValue = 0;

        if (type.enumValueIndex == 0)
        {
            options = typeof(Weapon).GetFields().Where(i => i.FieldType == typeof(float)).Select(i => i.Name).ToArray();
        }
        else
        {
            //dont worry about it
            options = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects().Where(i => i.name == "Player").FirstOrDefault().GetComponent<PlayerController>().stats.GetStatNames();
        }

        selected.intValue = EditorGUILayout.Popup("Modifying", selected.intValue, options);
        modifying.stringValue = options[selected.intValue];
        EditorGUILayout.PropertyField(modifier);

        serializedObject.ApplyModifiedProperties();
    }
}
