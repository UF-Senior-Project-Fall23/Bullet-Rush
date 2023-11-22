using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class StartRoom : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;
    private GameObject player;
    private GameManager gameManager;
    public Image wImage;
    public Image aImage;
    public Image sImage;
    public Image dImage;
    public Sprite greenWImageSprite;
    public Sprite greenAImageSprite;
    public Sprite greenSImageSprite;
    public Sprite greenDImageSprite;
    public Sprite grayWImageSprite;
    public Sprite grayAImageSprite;
    public Sprite graySImageSprite;
    public Sprite grayDImageSprite;


    // Start is called before the first frame update
    public List<GameObject> tutorialSquares;
    public List<GameObject> difficultyButtons;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        foreach (Transform child in transform)
        {
            if (child.gameObject.CompareTag("TutorialSquare"))
            {
                Debug.Log("Added square to list");
                tutorialSquares.Add(child.gameObject);
            }
            else if (child.gameObject.CompareTag("DifficultyButton"))
            {
                difficultyButtons.Add(child.gameObject);
                Debug.Log("Added difficulty button");
            }
        }
    }

    void Update()
    {
        changeColorOnKeyPress();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //check for trigger on tutorial message areas
        if (IsPlayerInsideSquare(player.transform.position, tutorialSquares[0]))
        {
            tutorialText.text = "Walk into the weapon to pick it up.";
            disableImages();
        }
        else if (IsPlayerInsideSquare(player.transform.position, tutorialSquares[1]))
        {
            tutorialText.text = "Use \nWASD\nto move.";
            enableImages();
        }
        else if (IsPlayerInsideSquare(player.transform.position, tutorialSquares[2]))
        {
            tutorialText.text = "Select Easy, Medium, or Hard";
            disableImages();
        }
        else if (IsPlayerInsideSquare(player.transform.position, tutorialSquares[3]))
        {
            tutorialText.text = "Enter Here to Start";
            disableImages();
        }
        else
        {
            tutorialText.text = "";
            disableImages();
        }
        //check for trigger on difficulty buttons
        if (IsPlayerOnButton(player.transform.position, difficultyButtons[0]))
        {
            gameManager.setDifficulty(Difficulty.Easy);
            updateDifficultyButtonColors();
        }
        else if (IsPlayerOnButton(player.transform.position, difficultyButtons[1]))
        {
            gameManager.setDifficulty(Difficulty.Medium);
            updateDifficultyButtonColors();
        }
        else if (IsPlayerOnButton(player.transform.position, difficultyButtons[2]))
        {
            gameManager.setDifficulty(Difficulty.Hard);
            updateDifficultyButtonColors();
        }
    }

    bool IsPlayerInsideSquare(Vector3 playerPosition, GameObject square)
    {
        BoxCollider2D squareCollider = square.GetComponent<BoxCollider2D>();
        if (squareCollider != null)
        {
            return squareCollider.bounds.Contains(playerPosition);
        }
        else
        {
            Debug.LogWarning("Square does not have a BoxCollider2D component.");
            return false;
        }
    }

    bool IsPlayerOnButton(Vector3 playerPosition, GameObject button)
    {
        BoxCollider2D buttonCollider = button.GetComponent<BoxCollider2D>();
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
        if (buttonCollider != null)
        {
            return buttonCollider.bounds.Contains(playerPosition);
        }
        else
        {
            Debug.LogWarning("Button does not have an BoxCollider2D component.");
            return false;
        }
    }

    void disableImages()
    {
        wImage.enabled = false;
        aImage.enabled = false;
        sImage.enabled = false;
        dImage.enabled = false;
        //set to gray
        wImage.sprite = grayWImageSprite;
        aImage.sprite = grayAImageSprite;
        sImage.sprite = graySImageSprite;
        dImage.sprite = grayDImageSprite;
    }
    void enableImages()
    {
        wImage.enabled = true;
        aImage.enabled = true;
        sImage.enabled = true;
        dImage.enabled = true;
    }
    void changeColorOnKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            wImage.sprite = greenWImageSprite; // You can specify the green sprite you want to use.
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            aImage.sprite = greenAImageSprite; // You can specify the green sprite you want to use.
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            sImage.sprite = greenSImageSprite; // You can specify the green sprite you want to use.
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            dImage.sprite = greenDImageSprite; // You can specify the green sprite you want to use.
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            wImage.sprite = grayWImageSprite;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            aImage.sprite = grayAImageSprite;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            sImage.sprite = graySImageSprite;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            dImage.sprite = grayDImageSprite;
        }
    }

    void updateDifficultyButtonColors()
    {

        if (gameManager.getCurrentDifficulty() == Difficulty.Easy)
        { // COLORS FOR EASY
            difficultyButtons[0].GetComponent<SpriteRenderer>().color = new Color(10 / 255.0f, 150 / 255.0f, 0);
            difficultyButtons[1].GetComponent<SpriteRenderer>().color = new Color(255 / 255.0f, 158 / 255.0f, 0);
            difficultyButtons[2].GetComponent<SpriteRenderer>().color = new Color(255 / 255.0f, 5 / 255.0f, 50 / 255.0f);
        }
        else if (gameManager.getCurrentDifficulty() == Difficulty.Medium)
        { // COLORS FOR MEDIUM
            difficultyButtons[0].GetComponent<SpriteRenderer>().color = new Color(19 / 255.0f, 255 / 255.0f, 0);
            difficultyButtons[1].GetComponent<SpriteRenderer>().color = new Color(200 / 255.0f, 100 / 255.0f, 0);
            difficultyButtons[2].GetComponent<SpriteRenderer>().color = new Color(255 / 255.0f, 5 / 255.0f, 50 / 255.0f);
        }
        else
        { // COLORS FOR HARD
            difficultyButtons[0].GetComponent<SpriteRenderer>().color = new Color(19 / 255.0f, 255 / 255.0f, 0);
            difficultyButtons[1].GetComponent<SpriteRenderer>().color = new Color(255 / 255.0f, 158 / 255.0f, 0);
            difficultyButtons[2].GetComponent<SpriteRenderer>().color = new Color(150 / 255.0f, 5 / 255.0f, 15 / 255.0f);
        }
    }
}
