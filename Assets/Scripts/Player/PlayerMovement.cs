using System.Collections;
using UnityEngine;

// Handles the player movement mechanics.
public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D m_body;
    float m_horizontal;
    float m_vertical;
    Vector3 m_zero = Vector3.zero;

    [Header("Basic Movement")]
    //How strong the movement smoothing is
    public float moveSmoothing = .05f;

    //How much the player's diagonal movement is limited
    public float moveLimiter = 0.7f;

    //How fast the player moves
    public float runSpeed = 20.0f;

    //iFrames Duration
    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;

    //Flashes that occur before the player gets out of iFrames
    [SerializeField] private int numberOfFlashes;

    [SerializeField] private TrailRenderer trail;

    private SpriteRenderer spriteRenderer;

    private bool canDash = true;
    private bool isDashing;
    // private float dashingPower = 30f;
    [Header("Dashing")]
    public float dashingPower = 60f;
    public float dashingTime = 0.01f;
    public float dashingCooldown = 1f;

    public void Awake()
    {
        m_body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Detects when the player dashes.
    void Update()
    {
        //In order to not have the player do any other movements if it is currently dashing we need to add this code block
        if (isDashing)
            return;

        if (Input.GetKeyDown(KeyCode.Space) && canDash)
            StartCoroutine(Dash());
    }

    // Moves the player with basic WASD or joystick input.
    void FixedUpdate()
    {
        m_horizontal = Input.GetAxisRaw("Horizontal");
        m_vertical = Input.GetAxisRaw("Vertical");

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


    // Causes the player to dash forward, becoming briefly invincible.
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        PlayerController.instance.health.SetInvulFrames(iFramesDuration);
        float originalGravity = m_body.gravityScale;
        m_body.gravityScale = 0f;
        m_horizontal = Input.GetAxisRaw("Horizontal");
        m_vertical = Input.GetAxisRaw("Vertical");
        if (m_horizontal != 0 && m_vertical != 0)
        {
            m_horizontal *= moveLimiter;
            m_vertical *= moveLimiter;
        }
        m_body.velocity = new Vector2(m_horizontal * dashingPower, m_vertical * dashingPower);
        trail.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        trail.emitting = false;
        m_body.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    // Resets whether the player can dash and their opacity.
    public void ResetMovement()
    {
        canDash = true;
        isDashing = false;
        spriteRenderer.color = new Color(255, 255, 255, 255);
    }
}
