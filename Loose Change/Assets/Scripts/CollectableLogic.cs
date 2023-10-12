using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class CollectableLogic : MonoBehaviour
{

    GameObject player;
    ScoreTracker scoreTracker;
    int scoreAdd = 100;
    public static GameObject scoreUI;

    // Start is called before the first frame update
    void Start()
    {
        scoreUI = GameObject.Find("CollectableScore");
        player = GameObject.FindWithTag("Player");
        scoreTracker = player.GetComponent<ScoreTracker>();

        StartCoroutine(spinCR(3, 0.02f, 2000000000));
        scoreUI.GetComponent<TextMeshProUGUI>().SetText(" ");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // The spinning visual coroutine
    IEnumerator spinCR(int spinDegrees, float timeToWait, int numberOfSpins)
    {
        for (int i = 0; i < numberOfSpins; i++) //83 is 3 complete spins of 13 degrees
        {
            gameObject.transform.Rotate(0, spinDegrees, 0); //odd number to offset the coin's spin
            yield return new WaitForSeconds(timeToWait);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.GetComponent<AudioSource>().Play();
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            scoreTracker.score += scoreAdd;
            Debug.Log("score is " + scoreTracker.score);
            StartCoroutine(scoreUICR(1.5f));
        }
    }

    IEnumerator scoreUICR(float timeToWait)
    {
        scoreUI.GetComponent<TextMeshProUGUI>().SetText("+" + scoreAdd);
        yield return new WaitForSeconds(timeToWait);
        scoreUI.GetComponent<TextMeshProUGUI>().SetText(" ");
        Destroy(gameObject);
    }
}
