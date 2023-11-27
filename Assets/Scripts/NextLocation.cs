using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NextLocation : MonoBehaviour
{
    public UnityEvent EnterBossRoom;
    public UnityEvent EnterLootRoom;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameManager gameManager = GameManager.instance;
        if (collision.gameObject.tag == "Player" && !gameManager.inLootRoom)
        {
            Debug.Log("Teleport To Loot Room");
            // set teleport to LootRoomLocation
            collision.transform.position = gameManager.getLootRoomLocation();
            gameManager.inLootRoom = true;
            EnterLootRoom.Invoke();
        }
        else if (collision.gameObject.tag == "Player" && gameManager.inLootRoom)
        {
            //based on current level, call retrieves the next level's teleport location
            Debug.Log("Teleport To Next Level");
            collision.transform.position = gameManager.getNextLevelLocation();
            //increment the currentLevel
            gameManager.incrementLevel();
            gameManager.inLootRoom = false;
            EnterBossRoom.Invoke();
            BossController.instance.LoadBoss(BossController.instance.BossName);
            BossController.instance.SummonBoss(new Vector3(-75, 25), 40);
        }
    }
}
