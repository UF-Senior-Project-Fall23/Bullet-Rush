using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI healthText;

    private float gameTime = 0f;
    private int score = 0;
    private int health = 100;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        gameTime += Time.deltaTime;
        timeText.text = "Time Elapsed: " + Mathf.Floor(gameTime).ToString();
        scoreText.text = "Score: " + score.ToString();
        healthText.text = "Health: " + health.ToString();
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

    public void DecreaseHealth(int type)
    {
        switch (type)
        {
            case 1:
                if(health == 0)
                {
                    //reset
                    health = 100;
                }
                else
                {
                    health -= 10;
                }
                
                break;
            case 2:
                //reset
                health = 100;
                break;
        }
    }
    public int checkHealth()
    {
        return health;
    }
}
