using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLocation : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameManager gameManager = FindObjectOfType<GameManager>();

        if (collision.gameObject.tag == "Player" && !gameManager.inLootRoom)
        {
            if (gameManager != null)
            {

                Debug.Log("Teleport To Loot Room");
                // set teleport to LootRoomLocation
                collision.transform.position = gameManager.getLootRoomLocation();
                gameManager.inLootRoom = true;

            }
            else
            {
                Debug.LogError("GameManager not found.");
            }
        }
        else if (collision.gameObject.tag == "Player" && gameManager.inLootRoom)
        {
            if (gameManager != null)
            {
                //based on current level, call retrieves the next level's teleport location
                Debug.Log("Teleport To Next Level");
                collision.transform.position = gameManager.getNextLevelLocation();
                //increment the currentLevel
                gameManager.incrementLevel();
                gameManager.inLootRoom = false;
                BossController.instance.LoadBoss(BossController.instance.BossName);
                BossController.instance.SummonBoss(new Vector3(-75, 25), 40);
            }
            else
            {
                Debug.LogError("GameManager not found.");
            }
        }
    }
}
