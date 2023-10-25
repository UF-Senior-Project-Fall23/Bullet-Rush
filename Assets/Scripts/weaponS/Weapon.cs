using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //Player
    public GameObject player;

    //How far the weapon will be from the player
    public float radius;

    //How fast the bullet will travel
    public float bulletForce = 20f;

    //How long between shots
    public float bulletDelay = 1.0f;

    public int damage;
    public GameObject bulletPreFab;

    protected Transform shootPoint;
    protected bool isFlipped;
    protected bool isShooting;

    private void Awake()
    {
        shootPoint = transform.Find("ShootPoint");
        isFlipped = false;
    }

    void Update()
    {
        if (!isShooting && Input.GetButtonDown("Fire1"))
            StartCoroutine("Shoot");
        else if (Input.GetKeyDown(KeyCode.Q))
            PlayerController.instance.DropWeapon();

        //Gets the mouse position
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (player != null)
        {
            //Calculates the angle the player is looking based on mouse and player position
            Vector2 lookAngle = mousePos - new Vector2(player.transform.position.x, player.transform.position.y);
            float mouseAngle = Mathf.Atan2(lookAngle.y, lookAngle.x);
            transform.position = new Vector3(
                player.transform.position.x + (radius * Mathf.Cos(mouseAngle)),
                player.transform.position.y + (radius * Mathf.Sin(mouseAngle)),
                1
            );

            transform.rotation = Quaternion.Euler(0, 0, mouseAngle * Mathf.Rad2Deg + 180);

            //TODO: Change rotation values only once instead of every Frame
            //Checks if the mouse is behind the player
            if (isFlipped && (mouseAngle < -1 * math.PI / 2 || mouseAngle > math.PI / 2))
                isFlipped = false;
            else if (!isFlipped && mouseAngle > -1 * math.PI / 2 && mouseAngle < math.PI / 2)
                isFlipped = true;

            //Flips the weapon if the mouse is behind the player
            transform.Rotate(new Vector3(System.Convert.ToInt16(isFlipped) * 180, 0, 0)); 
        }
    }

    public virtual IEnumerator Shoot()
    {
        isShooting = true;
        while (Input.GetButton("Fire1"))
        {
            GameObject bullet = Instantiate(bulletPreFab, shootPoint.position, shootPoint.rotation * Quaternion.Euler(0, 0, -90));
            bullet.GetComponent<Bullet>().damage = damage;
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            //Fire the bullet
            rb.AddForce(shootPoint.transform.up * bulletForce, ForceMode2D.Impulse);
            yield return new WaitForSeconds(bulletDelay);
        }
        isShooting = false;
        yield return null;
    }
}
