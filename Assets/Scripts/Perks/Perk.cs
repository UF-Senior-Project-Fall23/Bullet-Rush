using System.Collections.Generic;
using System.Linq;
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
    public Sprite sprite;

    [Tooltip("What does this perk modify")]
    public PerkType type;

    public int modificationSelected;
    public int typeSelected;

    [Tooltip("How many stats this perk modifies")]
    public int amount = 1;

    [Tooltip("What stat this perk modifies")]
    public List<string> modifying = new(1);

    [Tooltip("Percentage modifier")]
    public List<float> modifier = new(1);

    [TextArea(3, 10)] 
    [Tooltip("Tooltip description of what the perk does.")]
    public string description;

}

# if UNITY_EDITOR
[CustomEditor(typeof(Perk))]
public class PerkEditor : Editor
{
    SerializedProperty sprite;
    SerializedProperty type;
    SerializedProperty amount;
    SerializedProperty modifying;
    SerializedProperty modifier;
    SerializedProperty typeSelected;
    SerializedProperty modificationSelected;
    SerializedProperty description;
    string[] options;
    string[] mOptions;

    private void OnEnable()
    {
        modifying = serializedObject.FindProperty("modifying");
        modifier = serializedObject.FindProperty("modifier");
        amount = serializedObject.FindProperty("amount");
        type = serializedObject.FindProperty("type");
        sprite = serializedObject.FindProperty("sprite");
        typeSelected = serializedObject.FindProperty("typeSelected");
        modificationSelected = serializedObject.FindProperty("modificationSelected");
        description = serializedObject.FindProperty("description");
        UpdateLists();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(sprite);

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(type);
        if (EditorGUI.EndChangeCheck())
        {
            typeSelected.intValue = 0;
            if (type.enumValueIndex == 0)
            {
                for (int i = 0; i < modifying.arraySize; i++)
                    modifying.GetArrayElementAtIndex(i).stringValue = typeof(Weapon).GetFields().Where(i => i.FieldType == typeof(float)).Select(i => i.Name).ToArray()[0];
            }
            else
            {
                for (int i = 0; i < modifying.arraySize; i++)
                    modifying.GetArrayElementAtIndex(i).stringValue = FindAnyObjectByType<PlayerController>().stats.GetStatNames()[0];
            }
        }

        if (type.enumValueIndex == 0)
        {
            options = typeof(Weapon).GetFields().Where(i => i.FieldType == typeof(float)).Select(i => i.Name).ToArray();
        }
        else
        {
            //dont worry about it
            options = FindAnyObjectByType<PlayerController>().stats.GetStatNames();
        }

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(amount);
        if (EditorGUI.EndChangeCheck())
        {
            UpdateLists();
            modificationSelected.intValue = 0;
        }

        EditorGUI.BeginChangeCheck();
        modificationSelected.intValue = EditorGUILayout.Popup("Modification", modificationSelected.intValue, mOptions);
        if (EditorGUI.EndChangeCheck())
        {
            for(int i = 0; i < options.Length; i++)
            {
                if (options[i] == modifying.GetArrayElementAtIndex(modificationSelected.intValue).stringValue)
                {
                    typeSelected.intValue = i;
                    break;
                }  
            }
        }


        typeSelected.intValue = EditorGUILayout.Popup("Modifying", typeSelected.intValue, options);
        modifying.GetArrayElementAtIndex(modificationSelected.intValue).stringValue = options[typeSelected.intValue];
        EditorGUILayout.PropertyField(modifier.GetArrayElementAtIndex(modificationSelected.intValue), label: new GUIContent("Modifier"));

        EditorGUILayout.PropertyField(modifying);
        EditorGUILayout.PropertyField(modifier);

        UpdateLists();

        EditorGUILayout.PropertyField(description);
        
        serializedObject.ApplyModifiedProperties();
    }

    void UpdateLists()
    {
        if (modifying.arraySize != amount.intValue)
            ((Perk)target).modifying.Resize(amount.intValue, "");

        if (modifier.arraySize != amount.intValue)
            ((Perk)target).modifier.Resize(amount.intValue, 0);

        if (mOptions?.Length != amount.intValue)
        {
            mOptions = new string[amount.intValue];
            for (int i = 0; i < amount.intValue; i++)
                mOptions[i] = (i + 1).ToString();
        }
    }
}

# endif