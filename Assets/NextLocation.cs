using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLocation : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject YouGameObject;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Entered collision with what");
        Debug.Log("Entered collision with " + collision.gameObject.name);
        if (collision.gameObject == YouGameObject)
        {
            transform.position = new Vector3(-100, 0, 0);//(where you want to teleport)
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Entered collision with what");
        Debug.Log("Entered collision with " + collision.gameObject.name);
        if (collision.gameObject == YouGameObject)
        {
            transform.position = new Vector3(-100, 0, 0);//(where you want to teleport)
        }
    }
}
