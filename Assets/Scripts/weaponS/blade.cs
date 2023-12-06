using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

// Handles logic for using the Blade weapon.
public class blade : Weapon {

    private Animator anim; //change var name

    private void Awake()
    {
        shootPoint = transform.Find("Animation Point").Find("ShootPoint");
        isFlipped = false;
        anim = GetComponent<Animator>();
    }

    //TODO: If mouse1 is held down and weapon is swapped cancel animation
    // Runs the slash animation, which calls Slash() through the animator.
    public override IEnumerator Shoot(){
        isShooting = true;
        anim.Play("Slash");
        yield return null;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        anim.Play("Idle");
        isShooting = false;
    }

    // Shoots the slash projectile forward. This is the actual "bullet".
    public void Slash()
    {
        GameObject bullet = Instantiate(bulletPreFab, shootPoint.position, shootPoint.rotation * Quaternion.Euler(0, 0, 180));
        bullet.GetComponent<Bullet>().damage = damage;
        Destroy(bullet, .35f);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        //Fire the slash
        rb.AddForce(shootPoint.transform.right * bulletForce, ForceMode2D.Impulse);
        currentHeat += heatPerShot;
        if(currentHeat >= maxHeat)
        {
            currentHeat = maxHeat;
            isOverheated = true;
        }
    }

    // Deals melee damage.
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Enemy") {
            //other.GetComponent<Enemy>().TakeDamage(damage);
            collision.GetComponent<Damageable>()?.takeDamage(damage);
            Debug.Log("Enemy hit");
        }
    }
}
