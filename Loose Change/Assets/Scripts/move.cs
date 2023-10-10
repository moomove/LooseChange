using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditorInternal;
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
    public int balanceDrain = 2000; //higher is slower
    public float fallOverDegree = 60;
    public float health = 100;//momentum
    public float raycastDistance = 1.0f;

    public bool slippery = false;//perhaps if you go over slime you get a slippery debuff making you tilt faster
    public float goopSlowTime;

    public Text momentumUI;
    public Text balanceUI;

    public UnityEngine.UI.Slider balanceSlider = null;


    // used between update and fixed update to ensure movement is not framerate reliant
    bool forwardMove = false;
    bool rightMove = false;
    bool leftMove = false;
    bool upMove = false;
    bool spinMove = false;

    public bool coinAlive;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        coinAlive = true;

        StartCoroutine(speedIncreaseCR(10, 1));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hazard"))
        {
            int score = (int)gameObject.GetComponent<ScoreTracker>().score; //get score before presenting it on game over screen and saving it to page
            Debug.Log("HIT side of something CALLED " + collision);
            HitHazard(collision.gameObject); //remove damage
            if (health > 0) gameObject.GetComponent<Damage>().CoinDamage(.4f, .4f, 8); //run damage flash
            else
            {
                gameObject.GetComponent<Damage>().CoinDeath(); //run death 
                gameObject.GetComponent<EndLevel>().GameEnd(score, "Lose"); 
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Goop"))
        {
            Debug.Log("goop hit!");
            StartCoroutine(GoopCR(goopSlowTime));
        }
    }

    // Update is called once per frame
    // user input is detected in update
    void Update()
    {
        if (!coinAlive)
        {
            return; 
        }

        momentumUI.text = "Health: " + health;
        balanceUI.text = "Balance: " + playerBalance;

        RaycastHit hit;
        /*if (Physics.Raycast(transform.position, Vector3.forward, out hit, raycastDistance)) //FORWARD raycast
        {
            Debug.Log("hit " + hit.collider.gameObject.name);
            if (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("Hazard"))
            {
                Debug.Log("HIT side of something CALLED " + hit.collider.gameObject.name);
                HitHazard(hit.collider.gameObject); //remove damage
                if (health > 0) gameObject.GetComponent<Damage>().CoinDamage(.4f, .4f, 8); //run damage flash
                else gameObject.GetComponent<Damage>().CoinDeath(); //run death 
                gameObject.GetComponent<EndLevel>().GameEnd(0, "Lose"); //TODO replace 0 with score value
            }
        }*/
                
        if (Input.GetKey(right1Input) || Input.GetKey(right2Input))
        {
            rightMove = true;
        }

        if (Input.GetKey(left1Input) || Input.GetKey(left2Input)) //same as D but negative
        {
            leftMove = true;
        }
        if (Math.Abs(rb.velocity.x) < maxSpeed && Math.Abs(rb.velocity.z) < maxSpeed)
        {
            forwardMove = true;
        }
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance)) //DOWNWARDS raycast
        {   
            if (hit.collider.CompareTag("Ground")) //checks if player is on ground and allows movment
            {
                if (Input.GetKeyDown(up1Input) || Input.GetKeyDown(up2Input))
                {
                    upMove = true;
                }
                if (health < 100)
                {
                    //health += 0.01f; 
                }

                balance(hit.collider.gameObject); //checks rotation of the ground object

                if (Input.GetKeyDown(up1Input) || Input.GetKeyDown(up2Input)) 
                {
                    upMove = true;
                }

                if (Input.GetKeyDown(spinInput))
                {
                    spinMove = true;
                }
            }
            else if (hit.collider.CompareTag("Enemy"))// automatically jumps as bouncing off enemies seems fun and gives score maybe style points
            {                
                jumpCoin(jumpHeight);                
            }
            else if (hit.collider.CompareTag("Hazard"))//if ontop of this you insta die e.g. pitfall or idk cthulhu
            {
                Debug.Log("HIT top of something CALLED " + hit.collider.gameObject.name);
                HitHazard(hit.collider.gameObject); //remove damage
                if (health > 0) gameObject.GetComponent<Damage>().CoinDamage(.4f, .4f, 8); //run damage flash
                else
                {
                    gameObject.GetComponent<Damage>().CoinDeath(); //run death 
                    gameObject.GetComponent<EndLevel>().GameEnd(0, "Lose"); //TODO replace 0 with score value
                }
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
            forwardMove = false;
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
            Debug.Log("jumping UP");
            upMove = false;
        }

        if (spinMove)
        {
            spinCoin(500);
            Debug.Log("spinning");
            spinMove = false;
        }
    }

    private void HitHazard(GameObject hazard)
    {
        Debug.Log("Object Detected: ");// + hazard.name);
        if (!hazard.gameObject.CompareTag("Ground")) hazard.GetComponent<Obstacle>().Hit();
    }
    private void balance(GameObject ground)
    {        
        playerBalance += (ground.transform.rotation.eulerAngles.y-180)/balanceDrain;

        if (balanceSlider != null)
        {
            balanceSlider.value = playerBalance;
        }
        
        //Debug.Log(playerBalance);
        if (playerBalance <= -fallOverDegree || playerBalance >= fallOverDegree)
        {
            //gameObject.GetComponent<EndLevel>().GameEnd(score, "lose"); //TODO add some way to get the player's score
        }
    }

    // Moves the coin, parameters of +1 or -1
    void moveCoin(int direction)
    {
        currentAngle = tilt * direction;
        gameObject.transform.Rotate(0, tilt * direction, 0);
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
            speed += increaseValue; //increase total speed
            Debug.Log("max speed is " + maxSpeed + " and velocity is " + rb.velocity.magnitude);
            yield return new WaitForSeconds(increaseTime);
        }
    }

    IEnumerator GoopCR(float slowTime)
    {
        rb.velocity = new Vector3(rb.velocity.x / 2, rb.velocity.y, rb.velocity.z / 2);
        maxSpeed = maxSpeed / 2;
        speed = speed / 2;
        yield return new WaitForSeconds(slowTime);
        maxSpeed = maxSpeed * 2;
        speed = speed * 2;
    }


}
