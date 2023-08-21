/**
 * This document is intended to demonstrate tilt, forward movement,
 * and spinning. 
 * 
 * 
 **/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toggle : MonoBehaviour
{
    Rigidbody rb;
    public float speed; //INSPECTOR
    public float tilt; //INSPECTOR how much to 
    float currentAngle = 0; //current tilt
    public float maxAngle; //INSPECTOR maximum tilt (right)
    public float minAngle; //INSPECTOR minimum tilt (left)
    public float turnAngle; //INSPECTOR angle to turn when tilting (A and D)
    
    public bool TILT = false; //tilt the object along Z axis
    public bool SPIN = false; //spin the object on Y axis
    public bool MOVE = false; //move using transform.forward and SPIN

    //more complicated movement could make use of TILT angle to calculate SPIN or turn of object
    //for example, Input.D adds tilt; tilt adds to overall angle; angle*turnspeed = object turns according to calculated angle
    //adjust drag on rigidbody to adjust inertia

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; //don't fall over
    }

    // Update is called once per frame
    void Update()
    {
        if (TILT)
        {
            if (Input.GetKey(KeyCode.D))
            {
                if (currentAngle < maxAngle)
                {
                    gameObject.transform.Rotate(0, 0, tilt, Space.Self);
                    currentAngle += tilt;
                }
            }
            if (Input.GetKey(KeyCode.A))
            {
                if (currentAngle > minAngle)
                {
                    gameObject.transform.Rotate(0, 0, -tilt, Space.Self);
                    currentAngle += -tilt;
                }
            }
    
        }
        if (SPIN)
        {
            if (Input.GetKey(KeyCode.D))
            {
                gameObject.transform.Rotate(0, turnAngle * tilt, 0);
            }
            if (Input.GetKey(KeyCode.A))
            {
                gameObject.transform.Rotate(0, -(turnAngle * tilt), 0);
            }
        }
        if (MOVE)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rb.AddRelativeForce(Vector3.forward * speed);
            }
            if (Input.GetKey(KeyCode.D))
            {
                {
                    gameObject.transform.Rotate(0, turnAngle * tilt, 0);
                }
            }
            if (Input.GetKey(KeyCode.A))
            {
                {
                    gameObject.transform.Rotate(0, -(turnAngle * tilt), 0);
                }
            }
            if (Input.GetKey(KeyCode.S))
            {
                rb.AddRelativeForce(-(Vector3.forward * speed));
            }
        }
    }
}
