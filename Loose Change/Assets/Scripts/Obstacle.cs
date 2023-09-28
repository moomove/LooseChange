using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float health = 1;        //player momentum/health required to destroy
    public bool slippery = false;   //applys the slippery debuff to player
    public float momentumLoss = 1;  //health/momentum loss when hit

    public string playerObject = "player";
    private GameObject player;
    private Move move;

    void Start()
    {
        player = GameObject.Find(playerObject);

        // Check if the object was found
        if (player != null)
        {
            // You can now work with the foundObject
            move = player.GetComponent<Move>();
        }
        else
        {
            Debug.LogError("Player Not Found");
        }        
    }
    public void Hit()
    {
        Debug.Log("hit");
        move.health -= momentumLoss;
        //sound effect and explosion maybe

        if(move.health <= 0)
        {
            Debug.Log("YOULOSe");
            //gameOver()
        }
        else
        {
            Debug.Log(gameObject);
            //DestroyObject(gameObject);//destroys self so coin continues on
        }

        DestroyObject(gameObject);

        if (slippery == true)
        {
            move.slippery=true;
        }

        
    }
}
