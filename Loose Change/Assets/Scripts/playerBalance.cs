using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHazardBalance : MonoBehaviour
{
    // Start is called before the first frame update
    public float playerCurrBalance = 0;
    public float fallOverDegree = 60;
    public GameObject movementScipt;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //raycastdown or we could tie this to the y pos of the player granted we intend to have the pipe be a straight line all game
        //check how steep floor is
        //check for pitfalls and other hazards
        //update player movespeed when floor hazards

        //update player rotation

        if(playerCurrBalance <= -fallOverDegree || playerCurrBalance >= fallOverDegree) { 
        //playerfalls and looses function
        
        }
        
    }
}
