using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    //used for spawning coins above menu
    private void Start() 
    {
        if (gameObject.name.Contains("kill"))
        {
            UIOff();
            UIOn(titleObjects);
            UpdateHighScoreValues();
            StartCoroutine(MenuSpawner(coinTime));
        }
    }

    public float coinTime;
    public GameObject[] objectArray; //INSPECTOR add all prefabs to be displayed on screen
    public GameObject[] highScoreObjects; //INSPECTOR to hide high score UI on start
    public GameObject[] titleObjects; //INSPECTOR to hide all title UI when high score button pressed 

    // destroy anything if it hits the kill box
    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.name.Contains("kill"))
        {
            Destroy(other.gameObject);
        }
    }

    IEnumerator MenuSpawner(float time)
    {
        while (true)
        {
            Instantiate(objectArray[Random.Range(0, objectArray.Length - 1)], new Vector3(Random.Range(-350, 330), 402, 326), Quaternion.identity); //create random object and spawn it in a random place
            yield return new WaitForSeconds(time);
        }
    }

    //change to a different scene
    public void SceneSwitcher(string scene) //INSPECTOR change scene based on input
    {
        SceneManager.LoadScene(scene);
    }

    //update high score screen text to match player scores
    void UpdateHighScoreValues()
    {
        for (int i = 0; i < highScoreObjects.Length-1; i++)
        {
            if (highScoreObjects[i].name.Contains("Value")) //text to be changed is named Value
            {
                if (highScoreObjects[i].name.Contains("Top")) //top score is a seperate case
                {
                    highScoreObjects[i].GetComponent<TextMeshProUGUI>().SetText(PlayerPrefs.GetInt("HighScoreTop", 00000).ToString());
                }
                else //if recent
                {
                    if (highScoreObjects[i].name.Contains("1"))
                    {
                        highScoreObjects[i].GetComponent<TextMeshProUGUI>().SetText(PlayerPrefs.GetInt("HighScoreRecent1", 00000).ToString());
                    }
                    if (highScoreObjects[i].name.Contains("2"))
                    {
                        highScoreObjects[i].GetComponent<TextMeshProUGUI>().SetText(PlayerPrefs.GetInt("HighScoreRecent2", 00000).ToString());
                    }
                    if (highScoreObjects[i].name.Contains("3"))
                    {
                        highScoreObjects[i].GetComponent<TextMeshProUGUI>().SetText(PlayerPrefs.GetInt("HighScoreRecent3", 00000).ToString());
                    }
                }
            }
        }
    }

    //disable all UI elements
    void UIOff()
    {
        Debug.Log("hid all");
        for (int j = 0; j <= highScoreObjects.Length-1; j++)
        {
            highScoreObjects[j].SetActive(false);
        }

        for (int j = 0; j <= titleObjects.Length-1; j++)
        {
            titleObjects[j].SetActive(false);
        }
    }

    //activate all objects for a desired UI screen
    void UIOn(GameObject[] screen)
    {
        Debug.Log("added " + screen.Length);
        //UpdateHighScoreValues(); //update the high score screen to match relevant data
        for (int j = 0; j < screen.Length; j++)
        {
            screen[j].SetActive(true);
        }
    }

    public void UISwapper(string screen)
    {
        UIOff(); //clear the screen
        switch (screen) //enable the desired UI
        {
            case "highscore":
                UIOn(highScoreObjects);
                break;
            case "title":
                UIOn(titleObjects);
                break;
            case "settings": //to be added
                break;
            default:
                Debug.Log("Invalid choice, StartMenu UISwapper for options");
                break;
        }
    }

    //updates the high score screen to match players acquired scores //to be used at the end of a level
    public void SetHighScore(int newScore)
    {
        if (newScore > PlayerPrefs.GetInt("HighScoreTop")) PlayerPrefs.SetInt("HighScoreTop", newScore); //if new score is higher than current highest, replace it
        PlayerPrefs.SetInt("HighScoreRecent3", PlayerPrefs.GetInt("HighScoreRecent2")); //set 3rd most recent to 2nd
        PlayerPrefs.SetInt("HighScoreRecent2", PlayerPrefs.GetInt("HighScoreRecent1")); //set 2nd most recent to 1st
        PlayerPrefs.SetInt("HighScoreRecent1", newScore); //set new score as most recent score
    }
}
