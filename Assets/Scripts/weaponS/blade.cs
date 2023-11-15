using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class blade : Weapon {

    private Animator anim; //change var name

    private void Awake()
    {
        shootPoint = transform.Find("Animation Point").Find("ShootPoint");
        isFlipped = false;
        anim = GetComponent<Animator>();
    }

    public override IEnumerator Shoot(){
        isShooting = true;
        anim.Play("Slash");
        yield return null;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        anim.Play("Idle");
        isShooting = false;
    }

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

    public override void UpdateWeaponPos() {
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

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Enemy") {
            //other.GetComponent<Enemy>().TakeDamage(damage);
            collision.GetComponent<IHealth>()?.takeDamage(damage);
            Debug.Log("Enemy hit");
        }
    }
}
