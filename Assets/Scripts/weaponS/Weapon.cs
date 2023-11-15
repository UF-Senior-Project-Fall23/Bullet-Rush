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
    protected bool isOverheated = false;
    public float currentHeat = 0.0f;
    public float heatPerShot = 5.0f;
    public float maxHeat = 100.0f;
    private float cooldownRate = 20.0f;

    private void Awake()
    {
        shootPoint = transform.Find("ShootPoint");
        isFlipped = false;
    }

    public void Update()
    {
        GameManager.instance.heatText.text = "Heat: " + currentHeat.ToString();

        //Overheating cooldown and the check
        if (isOverheated)
        {
            if (currentHeat <= 0)
            {
                currentHeat = 0.0f;
                isOverheated = false;
            }
        }

        if (!isShooting && Input.GetButton("Fire1") && !isOverheated) {
            StartCoroutine(Shoot());
        }
        else if (Input.GetKeyDown(KeyCode.Q)) {
            PlayerController.instance.DropWeapon();
        }

        if (!isShooting)
        {
            currentHeat -= cooldownRate * Time.deltaTime;
            currentHeat = Mathf.Max(currentHeat, 0.0f);
        }

        if (player != null)
        {
            UpdateWeaponPos();
        }
    }

    public virtual void UpdateWeaponPos()
    {
        //Gets the mouse position
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

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

    public virtual IEnumerator Shoot()
    {
        isShooting = true;

        while (Input.GetButton("Fire1") && !isOverheated) {
            GameObject bullet = Instantiate(bulletPreFab, shootPoint.position, shootPoint.rotation * Quaternion.Euler(0, 0, -90));
            bullet.GetComponent<Bullet>().damage = damage;
            Destroy(bullet, .5f);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            //Fire the bullet
            rb.AddForce(shootPoint.transform.up * bulletForce, ForceMode2D.Impulse);
            currentHeat += heatPerShot;
            //Once it passes the threshold, player can't shoot
            if(currentHeat >= maxHeat) {
                currentHeat = maxHeat;
                isOverheated = true;
                isShooting = false;
                yield break;
            }
            yield return new WaitForSeconds(bulletDelay);
        }

        isShooting = false;
        yield return null;
    }
}
