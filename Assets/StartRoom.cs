using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class StartRoom : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;
    private GameObject player;

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

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsPlayerInsideSquare(player.transform.position, tutorialSquares[0]))
        {
            tutorialText.text = "Walk into the weapon to pick it up.";
        }
        else if (IsPlayerInsideSquare(player.transform.position, tutorialSquares[1]))
        {
            tutorialText.text = "Use \nWASD\nto move.";
        }
        else if (IsPlayerInsideSquare(player.transform.position, tutorialSquares[2]))
        {
            tutorialText.text = "Select Easy, Medium, or Hard";
        }
        else if (IsPlayerInsideSquare(player.transform.position, tutorialSquares[3]))
        {
            tutorialText.text = "Enter Here to Start";
        }
        else
        {
            tutorialText.text = "";
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
}
