using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinRepel : MonoBehaviour
{
    public float radius;
    public float force = 25;

    private void Start()
    {
        force = force * 1000;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hazard"))
        {
            Sphere(force, radius);
        }
    }

    // Update is called once per frame
    /*void FixedUpdate()
    {
        if (objectType.ToLower().Contains("cube"))
        {
            Cube();
        }
        else if (objectType.ToLower().Contains("sphere"))
        {
            Sphere();
        }
    }*/
    //for testing purposes, uncomment code

    //do not call this one, call sphere instead
    /*void Cube(float force, float radius)
    {
        force = force * 1000;
        Vector3 effectorPosition = transform.position;
        Collider[] colliders = Physics.OverlapBox(effectorPosition, new Vector3(transform.localScale.x * .6f, transform.localScale.y * .6f, transform.localScale.z * .6f));

        foreach (Collider coin in colliders)
        {
            Rigidbody rigidbody = coin.GetComponent<Rigidbody>();

            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(force * Time.deltaTime, effectorPosition, radius);
                Debug.Log("repelling " + coin.name);
            }
        }
    }*/

    //pushes the object out from the hazard it has collided with
    public void Sphere(float force, float radius)
    {
        force = force * 1000;
        Vector3 effectorPosition = transform.position;
        Collider[] colliders = Physics.OverlapSphere(effectorPosition, radius);

        foreach (Collider coin in colliders)
        {
            Rigidbody rigidbody = coin.GetComponent<Rigidbody>();

            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(force, effectorPosition, radius);
                Debug.Log("repelling " + coin.name + " from " + transform.name);
            }
        }
    }
}
