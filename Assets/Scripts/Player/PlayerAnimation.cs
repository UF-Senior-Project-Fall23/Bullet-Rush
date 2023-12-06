using System;
using UnityEngine;

// Sets up animator variables for the player.
public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private static readonly float HalfWidth = Screen.width / 2f;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");

        var lookX = 2 * Convert.ToSingle(Camera.main.ScreenToWorldPoint(Input.mousePosition).x > gameObject.transform.position.x) - 1;
        
        // Set whether the player is looking left or right, and whether they're moving.
        animator.SetFloat("X", lookX);
        animator.SetBool("IsMoving", x != 0 || y != 0);
        
    }
}
