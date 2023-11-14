using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tommyGun : Weapon
{
    public override IEnumerator Shoot()
    {
        isShooting = true;

        while (Input.GetButton("Fire1") && !isOverheated) {
            GameObject bullet = Instantiate(bulletPreFab, shootPoint.position, shootPoint.rotation * Quaternion.Euler(0, 0, -90));
            bullet.GetComponent<Bullet>().damage = damage;
            Destroy(bullet, .75f);
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
