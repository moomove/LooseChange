using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

public class move : MonoBehaviour
{
    Rigidbody rb;

    KeyCode left1Input = KeyCode.A;
    KeyCode left2Input = KeyCode.LeftArrow;
    KeyCode right1Input = KeyCode.D;
    KeyCode right2Input = KeyCode.RightArrow;
    KeyCode up1Input = KeyCode.W;
    KeyCode up2Input = KeyCode.Space;
    KeyCode spinInput = KeyCode.S;

    public float speed = 50; //INSPECTOR acceleration speed
    public float tilt = 1; //INSPECTOR how much to tilt on button press
    public float maxAngle = 50; //INSPECTOR maximum tilt (right)
    public float minAngle = -50; //INSPECTOR minimum tilt (left)
    public float maxSpeed = 12; //INSPECTOR how fast is too fast
    public float jumpHeight = 700; //INSPECTOR how high to jump

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

    // Moves the coin, parameters of +1 or -1
    void moveCoin(int direction)
    {
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
