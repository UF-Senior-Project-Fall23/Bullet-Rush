using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillableBar : MonoBehaviour
{
    private Image barImage; // Automatically added, this script should be attached to a UI Image object.
    public string name;
    public static Dictionary<string, FillableBar> AllBars = new();

    void Awake()
    {
        barImage = GetComponent<Image>();
        AllBars[name] = GetComponent<FillableBar>();
    }

    // Call this method to update the fill amount based on a variable (e.g., health or heat)
    public void SetFill(float currentAmount, float maxAmount)
    {
        barImage.fillAmount = currentAmount / maxAmount;
    }
}