using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHPBar : MonoBehaviour
{
    public static BossHPBar instance;
    public FillableBar bossBarFill;
    public Image bossBarFrame;
    public TextMeshProUGUI bossBarName;

    public Sprite CordeliaFrame;
    public Sprite BlagFrame;
    public Sprite OnyxFrame;
    public Sprite DefaultFrame;

    public void Start()
    {
        if (instance is null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetFrame(string bossName)
    {
        Image img = bossBarFrame.GetComponent<Image>();
        string fancyName;
        switch (bossName)
        {
            case "Cordelia":
                img.sprite = CordeliaFrame;
                fancyName = "Cordelia, The Puppet";
                break;
            case "Blagthoroth":
                img.sprite = BlagFrame;
                fancyName = "Blag'thoroth, Ancient Demon";
                break;
            case "Onyx":
                img.sprite = OnyxFrame;
                fancyName = "Onyx, Armed Crusader";
                break;
            default:
                img.sprite = DefaultFrame;
                fancyName = "Boss, Fallback Monster";
                break;
        }

        bossBarName.text = $"« {fancyName} »";
    }

    public void Setup(GameObject boss)
    {
        Damageable bossHP = boss.GetComponent<Damageable>();
        if (bossHP is not null)
        {
            SetHPBarHidden(false);
            //Debug.LogWarning($"Setting boss bar fill ({bossBarFill}) to {bossHP.CurrentHealth}/{bossHP.MaxHealth}");
            bossBarFill.SetFill(bossHP.CurrentHealth, bossHP.MaxHealth);
            bossHP.HPChange.AddListener(UpdateHPBar);
        }
        else
        {
            Debug.LogWarning("Error! Boss does not extend Damageable.");
        }
    }

    void UpdateHPBar(float current, float max)
    {
        //Debug.LogWarning($"Setting boss bar fill to {current}/{max}");
        bossBarFill.SetFill(current, max);
    }

    public void SetHPBarHidden(bool hidden)
    {
        bossBarFrame.gameObject.SetActive(!hidden);
    }
        
}
