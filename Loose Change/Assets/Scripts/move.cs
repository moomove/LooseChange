using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UIElements;

public class move : MonoBehaviour
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
    public float maxAngle = 50; //INSPECTOR maximum tilt (right)
    public float minAngle = -50; //INSPECTOR minimum tilt (left)
    public float maxSpeed = 12; //INSPECTOR how fast is too fast
    public float jumpHeight = 700; //INSPECTOR how high to jump

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Math.Abs(rb.velocity.x) < maxSpeed && Math.Abs(rb.velocity.z) < maxSpeed)//(Input.GetKey(KeyCode.W))
        {
            rb.AddRelativeForce(Vector3.forward * speed);
        }
        
        if (Input.GetKey(right1Input) || Input.GetKey(right2Input))
        {
            moveCoin(1); 
        }
        
        if (Input.GetKey(left1Input)|| Input.GetKey(left2Input)) //same as D but negative
        {
            moveCoin(-1);
        }

        if (Input.GetKeyDown(up1Input) || Input.GetKeyDown(up2Input)) //TODO stop player from jumping whilst they are in the air
        {
            Debug.Log("y velocity is " + rb.velocity.y);
            jumpCoin(jumpHeight);
        }

        if (Input.GetKeyDown(spinInput))
        {
            spinCoin(500);
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
        Debug.Log("spinning");
        StartCoroutine(spinAttack());
    }

    // The spin attack visual coroutine
    IEnumerator spinAttack()
    {
        for (int i = 0; i < 83; i++) //83 is 3 complete spins of 13 degrees
        {
            gameObject.transform.Rotate(0, 13, 0); //odd number to offset the coin's spin
            yield return new WaitForSeconds(.004f); 
        }
    }
}
