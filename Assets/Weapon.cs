using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firepoint;
    public float fireFOrce = 20f;

    public void Fire()
    {
        /*GameObject bullet = ObjectPool.SharedInstance.GetPooledObject(); 
            if (bullet != null) {
                bullet.transform.position = firepoint.transform.position;
                bullet.transform.rotation = firepoint.transform.rotation;
                bullet.SetActive(true);
            }*/
        GameObject bullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(firepoint.up * fireFOrce, ForceMode2D.Impulse);
    }
}
