using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class weaponScript : MonoBehaviour
{
    public GameObject player;
    public float radius;
    public int damage;

    Vector2 mousePos;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            Vector2 lookAngle = mousePos - new Vector2(player.transform.position.x, player.transform.position.y);
            float mouseAngle = Mathf.Atan2(lookAngle.y, lookAngle.x);
            transform.position = new Vector3(
                player.transform.position.x + (radius * Mathf.Cos(mouseAngle)),
                player.transform.position.y + (radius * Mathf.Sin(mouseAngle)),
                1
            );

            rb.rotation = mouseAngle * Mathf.Rad2Deg - 90f;
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && player == null)
        {
            player = collision.gameObject;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
        }
    }
}
