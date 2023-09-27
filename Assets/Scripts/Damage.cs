using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public Material coinMaterial; //MUST BE SET TO "Rendering Mode: Transparent"
    Rigidbody rigidbody;

    public int numberOfFlashes;
    public float minTransparency;
    public float timeBetweenFlash;

    public string enemyTag; //the tag assigned to all obstacles and enemies for ease of removal

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKey(KeyCode.W))
        {
            CoinDamage(.3f, .5f, 8);
        }
        if (Input.GetKey(KeyCode.D))
        {
            CoinDeath();
        }*/ 
        //For testing purposes, uncomment to view damage and death behaviour
    }

    private void OnCollisionEnter(Collision collision)
    {
        CoinDamage(timeBetweenFlash, minTransparency, numberOfFlashes);
        if (collision.gameObject.tag == enemyTag)
        {
            Destroy(collision.gameObject); //TEMPORARY remove the object the player hit TODO
        }
    }

    //slows coin and makes it flash, to be called when coin hits an object
    void CoinDamage(float timeBetweenFlash, float minTransparency, int numberOfFlashes)
    {
        StartCoroutine(CoinFlashCR(timeBetweenFlash, minTransparency, numberOfFlashes));
        rigidbody.velocity = new Vector3(rigidbody.velocity.x/2, rigidbody.velocity.y, rigidbody.velocity.z/2); //halve horizontal motion
        //TODO subtract from health/momentum 
    }

    //visual detail, makes the coin flash, not to be called outside of coinDamage()
    IEnumerator CoinFlashCR(float seconds, float minTransparency, int flashesRemaining) 
    {
        Color colour = coinMaterial.color;
        if (minTransparency < 0.1f)
        {
            minTransparency = 0.1f;
        }
        if (flashesRemaining % 2 != 0) colour.a = 1; //max
        if (flashesRemaining % 2 == 0) colour.a = minTransparency; //lower value

        coinMaterial.color = colour; //refresh material

        yield return new WaitForSeconds(seconds); //wait for "seconds" seconds
        if (flashesRemaining >= 0) StartCoroutine(CoinFlashCR(seconds, minTransparency, flashesRemaining-1)); //if there are still flashes remaining, recursion
    }

    //stops the coin moving and plays death animation (to be added), to be called when momentum/health is 0
    void CoinDeath()
    {
        //TODO play death animation OR manually fall over (change rotation)
        GameObject.FindWithTag("Player").GetComponent<Move>().coinAlive = false;
        rigidbody.velocity = new Vector3(0, 0, 0);
        StartCoroutine(CoinFlashCR(.2f, .4f, 8)); //temporary, until we have death animation TODO
    }
}
