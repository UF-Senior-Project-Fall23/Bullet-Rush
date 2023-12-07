using UnityEngine;
using UnityEngine.Events;

// Represents a portal that when entered loads a new area.
public class Portal : MonoBehaviour
{

    public string destination;
    public bool deleteOnUse = false;
    bool listening = false;
    public bool isVictoryPortal;
    
    // Events for detecting when a specific room is entered.
    public static UnityEvent EnterBossRoom = new();
    public static UnityEvent EnterLootRoom = new();
    public static UnityEvent EnterStartRoom = new();
    
    // Called when a portal to the next boss is generated in a loot room.
    public static UnityEvent MakeBossPortal = new();
    
    // Detects when a room changes, with the inputs being the origin type and the destination type.
    public static UnityEvent<RoomType, RoomType> ChangeRoom = new();

    // Portal entering logic
    private void OnCollisionEnter2D(Collision2D playerCollision)
    {

        if (!playerCollision.gameObject.CompareTag("Player")) return;
        
        var gameManager = GameManager.instance;
        if (gameManager is null) return;
        
        var bossManager = BossController.instance;
        if (bossManager is null) return;

        // Stored for ChangeRoom purposes
        RoomType originalRoom = gameManager.roomType;
        RoomType newRoom;

        // Only add this listener once
        if (name == "Portal to Next level" && !listening)
        {
            MakeBossPortal.AddListener(SpawnBossPortal);
            listening = true;
        }
        
        // Determine where you're going and run the associated code
        switch (destination)
        {
            case "Loot Room":
                Debug.Log("Teleport To Loot Room");
                EnterLootRoom.Invoke();
            
                playerCollision.transform.position = gameManager.getLootRoomLocation();
                newRoom = RoomType.LootRoom;
                FindObjectOfType<MusicManager>()?.FadeOutThenPlay("Loot Room", 0.25f);
                
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
                
                // Initialization code, the rest is just the "Boss" case but again (C# is bad and doesn't have fallthrough)
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

        if (isVictoryPortal)
        {
            GameManager.instance.winstreak++;
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

    public void SpawnBossPortal()
    {
        var newPortal = Instantiate(BossController.instance.portalPrefab, GameManager.instance.getLootRoomExitLocation(),
            Quaternion.identity);
        newPortal.GetComponent<Portal>().destination = "Boss";
    }
}
