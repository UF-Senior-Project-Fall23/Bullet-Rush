using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLocation : MonoBehaviour
{
    public GameObject teleportLocation;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
            collision.transform.position = teleportLocation.transform.position;
    }
}
