using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    // Start is called before the first frame update
    void Start()
    {
        startMenu = GetComponent<StartMenu>();
        scoreText.SetActive(false);
        scoreValue.SetActive(false);
        gameOverTitle.SetActive(false);
        nextButton.SetActive(false);
        menuButton.SetActive(false);
        gameWinTitle.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.G))
        {
            GameEnd(0, "win");
        }
    }

    //score = end of game score / time = end of game time / title = game over OR game win title
    public void GameEnd(int score, string title)
    {
        gameObject.GetComponent<Move>().coinAlive = false; //stop the coin's movement

        GameObject titleObject;
        if (title == "win")
        {
            titleObject = gameWinTitle;
        }
        else
        {
            titleObject = gameOverTitle;
        }
        // TODO stop player moving

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
            default:
                menuButton.SetActive(true);
                break;
        }
    }
}
