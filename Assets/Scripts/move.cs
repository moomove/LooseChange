using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Move : MonoBehaviour
{
    Rigidbody rb;
    float currentAngle = 0; //current tilt

    KeyCode left1Input = KeyCode.A;
    KeyCode left2Input = KeyCode.LeftArrow;
    KeyCode right1Input = KeyCode.D;
    KeyCode right2Input = KeyCode.RightArrow;
    KeyCode up1Input = KeyCode.W;
    KeyCode up2Input = KeyCode.Space;
    KeyCode spinInput = KeyCode.S;

    public float speed = 50; //INSPECTOR acceleration speed
    public float tilt = 1; //INSPECTOR how much to tilt on button press
    public float maxSpeed = 12; //INSPECTOR how fast is too fast
    public float jumpHeight = 700; //INSPECTOR how high to jump

    public float playerBalance = 0;
    public int balanceDrain = 400;
    public float fallOverDegree = 60;
    public float health = 100;//momentum
    public float raycastDistance = 1.0f;

    public bool slippery = false;//perhaps if you go over slime you get a slippery debuff making you tilt faster

    public Text momentumUI;
    public Text balanceUI;


    // used between update and fixed update to ensure movement is not framerate reliant
    bool forwardMove = false;
    bool rightMove = false;
    bool leftMove = false;
    bool upMove = false;
    bool spinMove = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

    }

    // Update is called once per frame
    // user input is detected in update
    void Update()
    {
        momentumUI.text = "Momentum: " + health;
        balanceUI.text = "Balance: " + playerBalance;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.forward, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("Enemy")|| hit.collider.CompareTag("Hazard"))
            {
                HitHazard(hit.collider.gameObject);
            }
        }

        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
        {   
            if (hit.collider.CompareTag("Ground")) //checks if player is on ground and allows movment
            {
                if(health < 100)
                {
                    health += 0.01f;                    
                }

                balance(hit.collider.gameObject);
                //checks rotation of the ground object
                if (Math.Abs(rb.velocity.x) < maxSpeed && Math.Abs(rb.velocity.z) < maxSpeed)
                {
                    forwardMove = true;
                }

                if (Input.GetKey(right1Input) || Input.GetKey(right2Input))
                {
                    rightMove = true;
                }

                if (Input.GetKey(left1Input) || Input.GetKey(left2Input)) //same as D but negative
                {
                    leftMove = true;
                }

                if (Input.GetKeyDown(up1Input) || Input.GetKeyDown(up2Input)) //TODO stop player from jumping whilst they are in the air
                {
                    upMove = true;
                }

                if (Input.GetKeyDown(spinInput))
                {
                    spinMove = true;
                }
            }
            else if (hit.collider.CompareTag("Enemy"))// automatically jumps as bouncing of enemies seems fun and gives score maybe style points
            {                
                jumpCoin(jumpHeight);                
            }
            else if (hit.collider.CompareTag("Hazard"))//if ontop of this you insta die e.g. pitfall or idk cthulhu
            {
                HitHazard(hit.collider.gameObject);
            }
        }


    }

    // physics actions based on user input are performed in fixed update
    private void FixedUpdate()
    {
        if (forwardMove)
        {
            //Debug.Log("moving forward! velocity is " + rb.velocity.magnitude);
            rb.AddRelativeForce(Vector3.forward * speed);
        }

        if (rightMove)
        {
            moveCoin(1);
            rightMove = false;
        }

        if (leftMove)
        {
            moveCoin(-1);
            leftMove = false;
        }

        if (upMove)
        {
            jumpCoin(jumpHeight);
            upMove = false;
        }

        if (spinMove)
        {
            spinCoin(500);
            spinMove = false;
        }
    }

    private void HitHazard(GameObject hazard)
    {        
        Debug.Log("Object Detected: " + hazard.name);
        hazard.GetComponent<Obstacle>().Hit();
    }
    private void balance(GameObject ground)
    {
        
        playerBalance += (ground.transform.rotation.eulerAngles.z-180)/balanceDrain;
        

        Debug.Log(playerBalance);
        if (playerBalance <= -fallOverDegree || playerBalance >= fallOverDegree)
        {
            //playerfalls and playerlose()
          //  Debug.Log("YOU LOSE");
        }
    }

    // Moves the coin, parameters of +1 or -1
    void moveCoin(int direction)
    {
        currentAngle = tilt * direction;
        gameObject.transform.Rotate(0, tilt * direction, 0);
    }

    // Called when the user moves left or right to apply the appropriate tilt and balance
    void tiltCoin()
    {
        //TODO update coins tilt to match world
    }

    // Makes the coin jump upward
    void jumpCoin(float upwardsForce)
    {
        rb.AddRelativeForce(Vector3.up * upwardsForce);
    }

    // Makes the coin jump and spin
    void spinCoin(float jumpHeight)
    {
        jumpCoin(jumpHeight);
        //Debug.Log("spinning");
        StartCoroutine(spinAttackCR());
    }


    // The spin attack visual coroutine
    IEnumerator spinAttackCR()
    {
        for (int i = 0; i < 83; i++) //83 is 3 complete spins of 13 degrees
        {
            gameObject.transform.Rotate(0, 13, 0); //odd number to offset the coin's spin
            yield return new WaitForSeconds(.004f);
        }
    }

    // Gradually increase the speed of the coin
    // increaseTime is the time between each increase
    // increaseValue is the amount to increase
    // by updating maxSpeed, the code in Update and FixedUpdate will speed up the coin automatically
    IEnumerator speedIncreaseCR(float increaseTime, float increaseValue)
    {
        while (true) //TODO update this to "while coin is alive/level is running" OR rely on stopcoroutine to end speed increase
        {
            maxSpeed += increaseValue; // increase max speed of coin //potentially update speed here too if friction is too much
            //Debug.Log("max speed is " + maxSpeed + " and velocity is " + rb.velocity.magnitude);
            yield return new WaitForSeconds(increaseTime);
        }
    }
}
