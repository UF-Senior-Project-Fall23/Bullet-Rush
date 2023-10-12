using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLocation : MonoBehaviour
{
    
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Entered collision with " + collision.gameObject.name);
        if (collision.gameObject.name == "Level1")
        {
            transform.position = new Vector3(-100, 0, 0);//(where you want to teleport)
        }
        else if (collision.gameObject.name == "Level2")
        {
            transform.position = new Vector3(-100, 100, 0);//(where you want to teleport)
        }
        else if (collision.gameObject.name == "Boss1")
        {
            // update based on rooms added
            // need to add something to not show the camera moving to the new location
            // need to flag the boss fought, so it will not appear again
            var boss1 = Random.Range(1, 3);
            
            
            boss1 = 1; // Manual override for testing.

            if (boss1 == 1)
            {
                //transform.position = new Vector3(100, 0, 0);//(where you want to teleport)
                
                BossController.instance.LoadBoss("Cordelia");
            }
            else if(boss1 == 2)
            {
                transform.position = new Vector3(100, 125, 0);//(where you want to teleport)
            }
            
        }
    }
}
