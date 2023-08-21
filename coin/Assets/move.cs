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
    public float speed = 100; //INSPECTOR
    public float tilt; //INSPECTOR how much to 
    float currentAngle = 0; //current tilt
    public float maxAngle; //INSPECTOR maximum tilt (right)
    public float minAngle; //INSPECTOR minimum tilt (left)
    public float turnAngle; //INSPECTOR angle to turn when tilting (A and D)

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddRelativeForce(Vector3.forward * speed);
            //Debug.Log("W");
        }
        if (Input.GetKey(KeyCode.D))
        {
            currentAngle += tilt;
            gameObject.transform.Rotate(0, turnAngle * tilt, 0);
        }
        if (Input.GetKey(KeyCode.A)) //same as D but negative
        {
            currentAngle += -tilt;
            //Debug.Log("current angle: " + currentAngle + " x :" + transform.rotation.x + " z: " + transform.rotation.z  );
            gameObject.transform.Rotate(0, turnAngle * -tilt, 0);

        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddRelativeForce(-(Vector3.forward * speed));
            //Debug.Log("S");
        }
    }
}
