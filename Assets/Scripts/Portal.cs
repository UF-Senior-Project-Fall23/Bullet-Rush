using System;
using UnityEngine;
using UnityEngine.Events;

public class Portal : MonoBehaviour
{

    public string destination;
    public bool deleteOnUse = false;
    
    public static UnityEvent EnterBossRoom = new();
    public static UnityEvent EnterLootRoom = new();
    public static UnityEvent EnterStartRoom = new();
    public static UnityEvent<RoomType, RoomType> ChangeRoom = new();
    
    private void OnCollisionEnter2D(Collision2D playerCollision)
    {

        if (!playerCollision.gameObject.CompareTag("Player")) return;
        
        //Debug.LogWarning("Attempting player collision");
        
        var gameManager = GameManager.instance;
        if (gameManager is null) return;
        
        var bossManager = BossController.instance;
        if (bossManager is null) return;

        RoomType originalRoom = gameManager.roomType;
        RoomType newRoom;
        
        switch (destination)
        {
            case "Loot Room":
                Debug.Log("Teleport To Loot Room");
                EnterLootRoom.Invoke();
            
                playerCollision.transform.position = gameManager.getLootRoomLocation();
                newRoom = RoomType.LootRoom;
                FindObjectOfType<MusicManager>()?.FadeOutThenPlay("Loot Room", 0.25f);
                
                // TODO: Delete this later and make it instead spawn when you pick up a perk
                var newPortal = Instantiate(bossManager.portalPrefab, gameManager.getLootRoomExitLocation(),
                    Quaternion.identity);
                newPortal.GetComponent<Portal>().destination = "Boss";
                
                break;
            
            case "Boss":
                Debug.Log("Teleport To Next Level");
                EnterBossRoom.Invoke();

                bossManager.StartBoss(gameManager.getCurrentLevel());

                playerCollision.transform.position = gameManager.getNextLevelLocation();
                newRoom = RoomType.Boss;
                
                gameManager.incrementLevel();
                break;
            
            case "Initialize":
                Debug.Log("Initialize & Teleport To Next Level");
                EnterBossRoom.Invoke();
                
                // Initialization code, the rest is just the boss case but again (C# is bad and doesn't have fallthrough)
                bossManager.GenerateRun();

                bossManager.StartBoss(gameManager.getCurrentLevel());
                playerCollision.transform.position = gameManager.getNextLevelLocation();
                newRoom = RoomType.Boss;
                
                gameManager.incrementLevel();
                break;
            
            case "Start":
                Debug.Log("Teleport To Start Area");
                EnterStartRoom.Invoke();

                newRoom = RoomType.Start;
                gameManager.GoToStart();
                break;
            
            default:
                Debug.LogWarning("Attempting to teleport somewhere nonexistent");
                newRoom = RoomType.Error;
                break;
                
        }

        if (newRoom != RoomType.Error)
        {
            gameManager.roomType = newRoom;
        }
        
        ChangeRoom.Invoke(originalRoom, newRoom);
        
        if (deleteOnUse)
        {
            Destroy(gameObject);
        }
    }
}
