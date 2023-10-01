using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponShoot : MonoBehaviour
{
    public GameObject bulletPreFab;
    Transform shootPoint;

    public float bulletForce = 20f;

    private void Start()
    {
        shootPoint = transform.Find("ShootPoint");
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && gameObject.GetComponent<weaponScript>().player != null){
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPreFab, shootPoint.position, shootPoint.rotation);
        bullet.GetComponent<bulletScript>().damage = gameObject.GetComponent<weaponScript>().damage;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        rb.AddForce(shootPoint.transform.up * bulletForce, ForceMode2D.Impulse);
    }
}
