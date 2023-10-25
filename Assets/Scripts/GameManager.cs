using UnityEngine;
using TMPro;


public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI weaponText;

    public GameObject lootRoomLocation;
    public GameObject level1Location;
    public GameObject level2Location;
    public GameObject level3Location;
    public GameObject level4Location;
    public GameObject level5Location;

    private GameObject[] levelCoordinates;

    private float gameTime = 0f;
    private int score = 0;

    private int currentLevel = 1;

    public bool inLootRoom = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //levelCoordinates[0] is the lootRoom location
            //levelCoordinates[1] is level 1 location
            //... and so on
            levelCoordinates = new GameObject[]
            {
                lootRoomLocation,
                level1Location,
                level2Location,
                level3Location,
                level4Location,
                level5Location
            };
        }
        else
        {
            Destroy(gameObject);
        }
        weaponText.text = "Weapon: None";
    }

    void Update()
    {
        gameTime += Time.deltaTime;
        timeText.text = "Time Elapsed: " + Mathf.Floor(gameTime).ToString();
        scoreText.text = "Score: " + score.ToString();
        healthText.text = "Health: " + PlayerController.instance.CurrentHealth.ToString();
    }

    public void AddScore(int type)
    {
        switch (type)
        {
            case 1:
                score += 10;
                break;
        }
    }

    public void incrementLevel()
    {
        Debug.Log("level incremented from: " + currentLevel + " to " + currentLevel + 1);
        currentLevel += 1;
    }
    public int getCurrentLevel()
    {
        return currentLevel;
    }
    public Vector3 getNextLevelLocation()
    {
        Vector3 nextLevelLocation = levelCoordinates[currentLevel + 1].transform.position;
        return nextLevelLocation;
    }
    public Vector3 getLootRoomLocation()
    {
        Vector3 lootRoomVector = levelCoordinates[0].transform.position;
        return lootRoomVector;
    }
}
