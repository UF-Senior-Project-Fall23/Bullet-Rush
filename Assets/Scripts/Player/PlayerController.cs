using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public PlayerHealth health;
    public PlayerStats stats;
    public PlayerMovement movement;
    public PlayerWeapon weapon;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            health = GetComponent<PlayerHealth>();
            stats = GetComponent<PlayerStats>();
            movement = GetComponent<PlayerMovement>();
            weapon = GetComponent<PlayerWeapon>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
