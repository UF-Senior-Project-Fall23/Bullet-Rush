using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class WeaponStats : ScriptableObject
{
    //How far the weapon will be from the player
    public float radius = 1.0f;

    //How fast the bullet will travel
    public float bulletForce = 20f;

    //How long between shots
    public float bulletDelay = 1.0f;

    //How long bullet is alive
    public float bulletLifetime = 1.0f;

    public float damage = 1.0f;

    public GameObject bulletPreFab;

    public float heatPerShot = 5.0f;
    public float maxHeat = 100.0f;
    public float cooldownRate = 20.0f;
}
