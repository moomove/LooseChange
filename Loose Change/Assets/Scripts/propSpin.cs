using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class propSpin : MonoBehaviour
{
    public float speed = 20f;
    public Vector3 axis = Vector3.up;

    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(axis * speed * Time.deltaTime);
    }
}
