using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public float coinTime;
    public GameObject[] objectarray; //INSPECTOR add all prefabs to be displayed on screen

    // destroy anything if it hits the kill box
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit " +  other.gameObject.name);
        if (gameObject.name.Contains("kill"))
        {
            Destroy(other.gameObject);
        }
    }

    //used for spawning coins above menu
    private void Start() 
    {
        if (gameObject.name.Contains("kill")) //the kill box spawns and kills
        {
            StartCoroutine(MenuSpawner(coinTime));
        }
    }

    IEnumerator MenuSpawner(float time)
    {
        while (true)
        {
            Instantiate(objectarray[Random.Range(0, objectarray.Length - 1)], new Vector3(Random.Range(-350, 330), 402, 326), Quaternion.identity); //create random object and spawn it in a random place
            yield return new WaitForSeconds(time);
        }
    }

    public void SceneSwitcher(string scene) //INSPECTOR change scene based on input
    {
        SceneManager.LoadScene(scene);
    }
}
