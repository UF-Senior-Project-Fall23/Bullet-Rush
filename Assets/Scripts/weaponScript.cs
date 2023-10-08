using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class weaponScript : MonoBehaviour
{
    public GameObject player;
    public float radius;
    public int damage;
    public GameObject bulletPreFab;
    Transform shootPoint;

    public float bulletForce = 20f;

    Vector2 mousePos;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        shootPoint = transform.Find("ShootPoint");
    }

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetButtonDown("Fire1") && gameObject.GetComponent<weaponScript>().player != null)
            Shoot();
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

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPreFab, shootPoint.position, shootPoint.rotation);
        bullet.GetComponent<bulletScript>().damage = damage;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        rb.AddForce(shootPoint.transform.up * bulletForce, ForceMode2D.Impulse);
    }
}
