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
    [SerializeField] private TrailRenderer trail;

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

    //iFrames Duration
    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;

    //Flashes that occur before the player gets out of iFrames
    [SerializeField] private int numberOfFlashes;

    private SpriteRenderer spriteOpacity;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 0.2f;

    private void Awake()
    {
        m_body = GetComponent<Rigidbody2D>();
        spriteOpacity = GetComponent<SpriteRenderer>();
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
        //In order to not have the player do any other movements if it is currently dashing we need to add this code block
        if(isDashing) {
            return;
        }
    }

    void FixedUpdate()
    {
        if(isDashing) {
            return;
        }

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

        if(Input.GetKeyDown(KeyCode.Space) && canDash) {
            StartCoroutine(Dash());
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
            StartCoroutine(Invulnerability());
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

    private IEnumerator Invulnerability() {
        Physics2D.IgnoreLayerCollision(7, 8, true);
        //invulnerability duration
        for(int i = 0; i < numberOfFlashes; i++){
            spriteOpacity.color = new Color(255, 0, 0, 20);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteOpacity.color = new Color(255, 255, 255, 255);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(7, 8, false);
    }

    private IEnumerator Dash() {
        canDash = false;
        isDashing = true;
        StartCoroutine(Invulnerability());
        float originalGravity = m_body.gravityScale;
        m_body.gravityScale = 0f;
        m_horizontal = Input.GetAxisRaw("Horizontal");
        m_vertical = Input.GetAxisRaw("Vertical");
        if (m_horizontal != 0 && m_vertical != 0)
        {
            m_horizontal *= moveLimiter;
            m_vertical *= moveLimiter;
        }
        // Vector2(xVelocity, yVelocity) in this case yVelocity is 0, might have to adjust this for isometric character
        m_body.velocity = new Vector2(m_horizontal * dashingPower, m_vertical * dashingPower);
        trail.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        trail.emitting = false;
        m_body.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

}
