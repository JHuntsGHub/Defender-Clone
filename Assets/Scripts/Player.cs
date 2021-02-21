using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : Entity {

    private float vertSpeed, baseSpeed, boostSpeed;
    public static bool movingRight { get; set; }
    public static bool justDied { get; set; }
    public static bool rescueingAstronaut { get; set; }
    private Camera cam;
    private Quaternion rotation;
    public GameObject laser;
    private Renderer rend;
    private float laserCooldown, deathCooldown;
    private GameObject astronautBeingRescued;
    private Vector2 deathPosition;
    public Animator animator;
    
    void Start () {
        vertSpeed = 5f;
        baseSpeed = vertSpeed / 2;
        boostSpeed = vertSpeed + 2;
        movingRight = true;                     // true means ship is moving right, false means the ship is moving left.
        rescueingAstronaut = false;
        cam = Camera.main;
        rotation = new Quaternion();
        laserCooldown = 0;
        deathCooldown = 4;
        rend = GetComponentInParent<Renderer>();
    }
	
	void Update () {
       
        HandleDeath();
        if (!justDied)
        {
            Hyperdrive();
            DoMovement();
            FireLaser();
            MoveCamera();
        }
    }

    private void Hyperdrive()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            animator.SetBool("Hyperdriving", true);
            ForcedHyperdrive();
        }
    }

    private void ForcedHyperdrive()
    {
        transform.position = new Vector2(Random.Range(-16, 16), Random.Range(maxHeight, minHeight));
        ScatterRemainingEnemies();
        ScatterAstronauts();
    }

    private void DoMovement()
    {
        float xPosition = 0, yPosition = 0;
        ShipTrail.showTrail = false;
        
        /**
         * The following controlls vertical movement.
         */
        if(Input.GetAxis("Vertical") != 0)
        {
            yPosition += vertSpeed * Input.GetAxis("Vertical");
        }
        else
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                yPosition += vertSpeed;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                yPosition -= vertSpeed;
            }
        }

        /**
         * The following controlls horizontal movement.
         */
        if (Input.GetAxis("Horizontal") > 0 || Input.GetKey(KeyCode.RightArrow))
        {
            xPosition += boostSpeed;
            ShipTrail.showTrail = true;
            if (!movingRight)
            {
                rotation.eulerAngles = new Vector3(0, 0, 0);
                transform.rotation = rotation;
                movingRight = true;
            }
        }
        else if (Input.GetAxis("Horizontal") < 0 || Input.GetKey(KeyCode.LeftArrow))
        {
            xPosition += boostSpeed;
            ShipTrail.showTrail = true;
            if (movingRight)
            {
                rotation.eulerAngles = new Vector3(0, 180, 0);
                transform.rotation = rotation;
                movingRight = false;
            }
        }

        /**
         * Here we actually position the player and move the camera's x position to match it.
         */
        xPosition += baseSpeed;
        transform.Translate(new Vector2(xPosition, yPosition) * Time.deltaTime);
        OutOfVertRangeCheck();
    }

    private void MoveCamera()
    {
        cam.transform.position = new Vector3(transform.position.x, 0, -10);

        //int camOffset = 6;
        //int camMoveSpeed;
        //if (movingRight)
        //    camMoveSpeed = 18;
        //else
        //    camMoveSpeed = -18;

        //cam.transform.Translate(new Vector3(camMoveSpeed * Time.deltaTime, 0, -10));

        //if (cam.transform.position.x >= transform.position.x + camOffset)
        //{
        //    cam.transform.position = new Vector3(transform.position.x + camOffset, 0, -10);
        //}
        //else if (cam.transform.position.x <= transform.position.x - camOffset)
        //{
        //    cam.transform.position = new Vector3(transform.position.x - camOffset, 0, -10);
        //}
    }

    void FireLaser()
    {
        laserCooldown -= 1 * Time.deltaTime;
        if(laserCooldown <= 0)
        {
            if(Input.GetButton("Fire1"))
            {
                float offset = 0.7f;
                if (!movingRight)
                    offset *= -1;
                Instantiate(laser, new Vector2(transform.position.x + offset,transform.position.y), Quaternion.identity);
                laserCooldown = 0.3f;
            }
        }
    }

    /*
     * TODO
     * The problem with this is that it's not only detecting the ship's collider.
     * It's also detecting the collisions from the BoundsCollider and I'm not too sure how to fix it..
     */
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (!justDied && !GameControl.playerIsInvincible)
        {
            //TODO Player has to respawn at beggining and game controller has to reset environment with remaining astronauts and aliens.
            if (coll.gameObject.tag == "Enemy" || coll.gameObject.tag == "Bomb" || coll.gameObject.tag == "EnemyLaser")
            {
                BeingDestroyed();
            }
        }

        if (coll.gameObject.tag == "Astronaut" && !rescueingAstronaut && !coll.gameObject.GetComponent<Astronaut>().beingAbducted)// && transform.position.y > minHeight + 0.5)
        {
            Debug.Log("Astronaut hit.");
            astronautBeingRescued = coll.gameObject;
            astronautBeingRescued.GetComponent<Astronaut>().beingRescued = true;
            astronautBeingRescued.GetComponent<Astronaut>().bigFall = false;
            astronautBeingRescued.transform.parent = gameObject.transform;
            rescueingAstronaut = true;
        }
    }

    private void HandleDeath()
    {
        if (justDied)
        {
            deathCooldown -= Time.deltaTime;
            if (deathCooldown <= 0)
            {
                if (GameControl.remainingLives <= 0)
                    GameControl.GoToGameOver();

                deathCooldown = 4;
                justDied = false;
                ShipTrail.trailEnabled = true;
                ForcedHyperdrive();
                gameObject.GetComponent<Renderer>().enabled = true;
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
            }

            //justDied = false;
        }
    }

    override protected void BeingDestroyed()
    {
        Debug.Log("You is dead!! Get gud!!");
        justDied = true;
        GameControl.remainingLives--;

        gameObject.GetComponent<Renderer>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        ShipTrail.showTrail = false;
        ShipTrail.trailEnabled = false;
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }

    protected override void OutOfVertRangeCheck()
    {
        if(transform.position.y < minHeight + 0.1 && rescueingAstronaut)
        {
            astronautBeingRescued.transform.parent = null;
            astronautBeingRescued.GetComponent<Astronaut>().beingRescued = false;
            rescueingAstronaut = false;
            GameControl.Score += 500;
        }
        base.OutOfVertRangeCheck();
    }

    private void ScatterRemainingEnemies()
    {
        playerPos = FindObjectOfType<Player>().gameObject.transform.position;
        GameObject[] gameObjects = FindObjectsOfType<GameObject>();
        foreach(GameObject go in gameObjects)
        {
            if(go.tag == "Enemy")
            {
                bool nearPlayer = true;
                float xOffset = 2.5f;
                Vector2 newPosition;
                while (nearPlayer)
                {
                    newPosition = new Vector2(Random.Range(transform.position.x - 15, transform.position.x + 15), Random.Range(-1f, 2.5f));

                    if(newPosition.x < playerPos.x - xOffset || newPosition.x > playerPos.x + xOffset)
                    {
                        nearPlayer = false;
                        go.transform.position = newPosition;
                    }
                    
                }
                
            }
        }
    }

    private void ScatterAstronauts()
    {
        GameObject[] gameObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject go in gameObjects)
        {
            if (go.tag == "Astronaut")
            {
                if (!go.GetComponent<Astronaut>().beingAbducted)
                {
                    go.transform.position = new Vector2(Random.Range(transform.position.x - 15, transform.position.x + 15), -4);
                }
            }
        }
    }
}
