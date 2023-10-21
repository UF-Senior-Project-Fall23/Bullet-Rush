using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    Rigidbody2D m_body;
    float m_horizontal;
    float m_vertical;
    Vector3 m_zero = Vector3.zero;
    bool m_alive = true;

    GameObject weapon;
    weaponScript m_weaponScript;
    
    //How strong the movement smoothing is
    public float moveSmoothing = .05f;
    
    //How much the player's diagonal movement is limited
    public float moveLimiter = 0.7f;

    //How fast the player moves
    public float runSpeed = 20.0f;

    //Player maximum health
    public int maxHealth = 10;

    //Player current health
    public int currHealth { get; private set; }

    private void Awake()
    {
        m_body = GetComponent<Rigidbody2D>();
        currHealth = maxHealth;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        if (m_alive)
        {
            //Gets raw movement input
            m_horizontal = Input.GetAxisRaw("Horizontal");
            m_vertical = Input.GetAxisRaw("Vertical");
        }
        else
        {
            m_horizontal = 0f;
            m_vertical = 0f;
        }

        //Limit movement if moving diagonal
        if (m_horizontal != 0 && m_vertical != 0)
        {
            m_horizontal *= moveLimiter;
            m_vertical *= moveLimiter;
        }


        // This finds the target velocity of the player
        Vector3 targetVelocity = new Vector2(m_horizontal * runSpeed, m_vertical * runSpeed);
        // Creates fluid movement by smoothing out the difference in target and current velocity
        m_body.velocity = Vector3.SmoothDamp(m_body.velocity, targetVelocity, ref m_zero, moveSmoothing);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision with " + collision.gameObject.name);
        //If player collides with and enemy remove hp
        if (collision.gameObject.tag == "Enemy")
        {
            decreaseHealth(1);
        }
        //Pick up weapon only if player doesnt have a weapon
        else if (collision.gameObject.tag == "Weapon" && weapon == null)
        {
            weapon = collision.gameObject;
            weapon.tag = "CurrentWeapon";
            m_weaponScript = weapon.GetComponent<weaponScript>();
            m_weaponScript.player = gameObject;
            m_weaponScript.enabled = true;
            weapon.GetComponent<Collider2D>().enabled = false;
            GameManager.instance.weaponText.text = "Weapon: " + weapon.name;
        }
    }

    public void dropWeapon()
    {
        weapon.tag = "Weapon";
        m_weaponScript.player = null;
        m_weaponScript.enabled = false;
        weapon.GetComponent<Collider2D>().enabled = true;
        weapon = null;
        m_weaponScript = null;
        GameManager.instance.weaponText.text = "Weapon: None";
    }

    public void decreaseHealth(int damage)
    {
        currHealth -= damage;

        if (currHealth <= 0)
        {
            currHealth = 0;
            m_alive = false;
        }
    }

}
