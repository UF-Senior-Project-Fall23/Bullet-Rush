using UnityEngine;

public class Cordelia : MonoBehaviour, Boss
{

    private string currentDanceType = null;
    private double attackSpeedModifier = 1;
    private bool phaseShifted = false;

    public string[] Attacks { get; } =
    {
        "SpinDance", "KickDance", "StringDance", "SummonPuppets", "DetonatePuppets", "Rush", "Spotlight", "BladeFlourish", "PuppeteersGrasp"
    };
    

    // Sets the dance to Spin.
    void SpinDance()
    {
        currentDanceType = "Spin";
    }

    void KickDance()
    {
        currentDanceType = "Kick";
    }

    void StringDance()
    {
        currentDanceType = "String";
    }

    void SummonPuppets()
    {

    }

    void DetonatePuppets()
    {

    }

    void Rush()
    {
        
    }

    void Spotlight()
    {

    }

    void BladeFlourish()
    {

    }

    void PuppeteersGrasp()
    {

    }
    

    void Boss.PhaseChange()
    {
        phaseShifted = true;
        attackSpeedModifier = 1.5;
    }

    void Boss.BossLogic()
    {
        
    }



}
