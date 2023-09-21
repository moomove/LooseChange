using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class EndLevel : MonoBehaviour
{
    public GameObject[] UIelements; //all UI related to end of level except titles
    public GameObject gameOverTitle;
    public GameObject gameWinTitle;
    StartMenu startMenu;

    // Start is called before the first frame update
    void Start()
    {
        startMenu = GetComponent<StartMenu>();
        for (int i = 0; i < UIelements.Length; i++) //turn off all ui
        {
            UIelements[i].gameObject.SetActive(false);
        }
        gameOverTitle.SetActive(false);
        gameWinTitle.SetActive(false);
    }

    //score = end of game score / time = end of game time / title = game over OR game win title
    public void GameEnd(int score, float time, GameObject title)
    {
        // TODO stop player moving
        for (int i = 0; i < UIelements.Length;i++)
        {
            UIelements[i].gameObject.SetActive(true);
            if (UIelements[i].name.Contains("ScoreValue"))
            {
                UIelements[2].GetComponent<TextMeshProUGUI>().SetText(score.ToString());
            }
            else if (UIelements[i].name.Contains("TimeValue"))
            {
                UIelements[3].GetComponent<TextMeshProUGUI>().SetText(time.ToString());
            }
        }
        title.SetActive(true);
        startMenu.SetHighScore(score); //for title high score screen
    }
}
