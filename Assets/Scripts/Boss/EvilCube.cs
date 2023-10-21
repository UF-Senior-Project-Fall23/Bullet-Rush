using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilCube : MonoBehaviour, Boss
{
    public string[] Attacks { get; } =
{
        "Attack", "Invulnerable", "Vulnerable"
    };

    void Attack()
    {

    }

    void Invulnerable()
    {

    }

    void Vulnerable()
    {

    }

    void Boss.PhaseChange()
    {

    }

    void Boss.BossLogic(GameObject cb, Vector3 playerPos)
    {
         
    }
}
