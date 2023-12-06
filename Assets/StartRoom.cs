using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Handles generating and displaying the Starting Area
public class StartRoom : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;
    private GameObject player;
    private GameManager gameManager;
    
    // Sprites/Objects for the WASD display in the start area.
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

    // List of trigger colliders for different tutorial messages
    public List<GameObject> tutorialSquares;
    
    // List of colliders for difficulty buttons
    public List<GameObject> difficultyButtons;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
        // Add colliders to lists
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
        // Adjusts the tutorial message shown based on where the player is standing.
        if (IsPlayerInsideSquare(player.transform.position, tutorialSquares[0]))
        {
            tutorialText.text = "Press Q while next to your weapon of choice.";
            disableImages();
        }
        else if (IsPlayerInsideSquare(player.transform.position, tutorialSquares[1]))
        {
            tutorialText.text = "Use \nWASD\nto move.";
            enableImages();
        }
        else if (IsPlayerInsideSquare(player.transform.position, tutorialSquares[2]))
        {
            tutorialText.text = "Walk over the Easy, Medium, or Hard button.";
            disableImages();
        }
        else if (IsPlayerInsideSquare(player.transform.position, tutorialSquares[3]))
        {
            tutorialText.text = "Enter the Portal to Start";
            disableImages();
        }
        else
        {
            tutorialText.text = "";
            disableImages();
        }
        
        
        // Sets the difficulty based on which button the player is standing on.
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

    // Checks if the player is within the bounds of a tutorial collider.
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

    // Checks if the player is within the bounds of a difficulty button.
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

    // Hides the WASD display.
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
    
    // Shows the WASD display.
    void enableImages()
    {
        wImage.enabled = true;
        aImage.enabled = true;
        sImage.enabled = true;
        dImage.enabled = true;
    }
    
    // Makes the WADS display change color based on which keys are pressed.
    void changeColorOnKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            wImage.sprite = greenWImageSprite;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            aImage.sprite = greenAImageSprite;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            sImage.sprite = greenSImageSprite;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            dImage.sprite = greenDImageSprite;
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

    // Changes the color of the difficulty buttons on the ground.
    // TODO: Cache all of the renderers and colors, and make it so it only updates the necessary stuff.
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
