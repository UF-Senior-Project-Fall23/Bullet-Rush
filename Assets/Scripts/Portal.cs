using System;
using UnityEngine;

public class Portal : MonoBehaviour
{

    public string destination;
    public bool deleteOnUse = false;
    
    private void OnCollisionEnter2D(Collision2D playerCollision)
    {

        if (!playerCollision.gameObject.CompareTag("Player")) return;
        
        //Debug.LogWarning("Attempting player collision");
        
        var gameManager = GameManager.instance;
        if (gameManager is null) return;
        
        var bossManager = BossController.instance;
        if (bossManager is null) return;
        
        
        switch (destination)
        {
            case "Loot Room":
                Debug.Log("Teleport To Loot Room");
            
                playerCollision.transform.position = gameManager.getLootRoomLocation();
                gameManager.roomType = RoomType.LootRoom;
                FindObjectOfType<MusicManager>()?.FadeOutThenPlay("Loot Room", 0.25f);
                
                // TODO: Delete this later and make it instead spawn when you pick up a perk
                var newPortal = Instantiate(bossManager.portalPrefab, gameManager.getLootRoomExitLocation(),
                    Quaternion.identity);
                newPortal.GetComponent<Portal>().destination = "Boss";
                
                break;
            
            case "Boss":
                Debug.Log("Teleport To Next Level");
                
                playerCollision.transform.position = gameManager.getNextLevelLocation();
                gameManager.roomType = RoomType.Boss;
                
                bossManager.StartBoss(gameManager.getCurrentLevel());
                gameManager.incrementLevel();
                break;
            
            case "Initialize":
                Debug.Log("Initialize & Teleport To Next Level");
                
                // Initialization code, the rest is just the boss case but again (C# is bad and doesn't have fallthrough)
                bossManager.GenerateRun();
                
                playerCollision.transform.position = gameManager.getNextLevelLocation();
                gameManager.roomType = RoomType.Boss;
                
                bossManager.StartBoss(gameManager.getCurrentLevel());
                gameManager.incrementLevel();
                break;
            
            case "Start":
                Debug.Log("Teleport To Start Area");
                
                gameManager.GoToStart();
                break;
            
            default:
                Debug.LogWarning("Attempting to teleport somewhere nonexistent");
                break;
                
        }

        if (deleteOnUse)
        {
            Destroy(gameObject);
        }
    }
}
