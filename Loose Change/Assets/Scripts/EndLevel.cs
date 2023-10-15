using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class EndLevel : MonoBehaviour
{
    public GameObject scoreText;
    public GameObject scoreValue;
    public GameObject gameOverTitle;
    public GameObject gameWinTitle;
    public GameObject nextButton;
    public GameObject menuButton;
    StartMenu startMenu;
    public GameObject instructions;

    public bool gameWinObject = false; //INSPECTOR true if the object running this script is the end-of-level wall
    public bool gameLoseObject = false; //if the player hits the kill box

    // Start is called before the first frame update
    void Start()
    {
        if (gameWinObject) startMenu = GameObject.FindWithTag("Player").GetComponent<StartMenu>();
        else startMenu = GetComponent<StartMenu>();
        scoreText.SetActive(false);
        scoreValue.SetActive(false);
        gameOverTitle.SetActive(false);
        nextButton.SetActive(false);
        menuButton.SetActive(false);
        gameWinTitle.SetActive(false);
        if (instructions != null)
        {
            instructions.SetActive(false); //in case of coroutine failure
            StartCoroutine(instructionsCR());
        }
    }

    //show instruction card for 5 seconds 
    IEnumerator instructionsCR()
    {
        instructions.SetActive(false);
        yield return new WaitForSeconds(.5f);
        instructions.SetActive(true);
        yield return new WaitForSeconds(5);
        instructions.SetActive(false);
    }

    /*private void Update()
    {
        if (Input.GetKey(KeyCode.G))
        {
            GameEnd(0, "win");
        }
        
    }*/

    //score = end of game score / time = end of game time / title = game over OR game win title
    public void GameEnd(int score, string title)
    {
        if (gameWinObject || gameLoseObject) GameObject.FindWithTag("Player").GetComponent<Move>().coinAlive = false; //if not the coin, get from coin
        else gameObject.GetComponent<Move>().coinAlive = false; //if coin, stop movement

        GameObject titleObject;
        if (title == "win")
        {
            titleObject = gameWinTitle;
        }
        else
        {
            titleObject = gameOverTitle;
        }
        

        scoreText.SetActive(true);
        scoreValue.GetComponent<TextMeshProUGUI>().SetText(score.ToString()); //set score text to match player's achieved score
        scoreValue.SetActive(true);
        titleObject.SetActive(true);
        startMenu.SetHighScore(score); //for title high score screen
        string currentScene = SceneManager.GetActiveScene().name;
        Debug.Log(currentScene);
        switch (currentScene)
        {
            case "Level01":
                nextButton.SetActive(true);
                break;
            case "Level02":
                nextButton.SetActive(true);
                break;
            case "Level03":
            default:
                menuButton.SetActive(true);
                break;
        }
    }

    //if the player hits the end-of-level object, run the win UI
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit by " + collision.gameObject.name + " on " + gameObject.name + "Game win is " + gameWinObject + " and game lose is " + gameLoseObject);
        if (gameWinObject)
        {
            int score = (int)GameObject.FindWithTag("Player").GetComponent<ScoreTracker>().score; //get score before presenting it on game over screen and saving it to page
            GameEnd(score, "win");
        }
        if (gameLoseObject)
        {
            int score = (int)GameObject.FindWithTag("Player").GetComponent<ScoreTracker>().score; //get score before presenting it on game over screen and saving it to page
            GameEnd(score, "lose");
        }
    }

}
