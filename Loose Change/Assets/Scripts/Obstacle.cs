using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float health = 1;        //player momentum/health required to destroy
    public bool slippery = false;   //applys the slippery debuff to player
    public float momentumLoss = 1;  //health/momentum loss when hit

    public GameObject player;
    private Move move;

    void Start()
    {
        move = player.GetComponent<Move>();
    }
    public void Hit()
    {        
        move.health -= momentumLoss;

        if(move.health <= 0)
        {
            Debug.Log("YOULOSe");
            //gameOver()
        }

        if(slippery == true)
        {
            move.slippery=true;
        }

        DestroyObject(gameObject);//destroys self so coin continues on
    }

    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        
    }
}
