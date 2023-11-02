using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class StartRoom : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;
    private GameObject player;
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
        }
    }

    void Update()
    {
        changeColorOnKeyPress();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
}
