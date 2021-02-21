using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity {

    protected float fireCooldown = 5, lastFired = 0, firingRange = 6.5f, moveInDirectionCooldown = 2;
    protected Vector2 directionToMove;
    protected BoxCollider2D box2D;
    public int currentState = 0;
    public GameObject enemyLaserPrefab;
    protected Camera cam;
    protected Renderer rend;

	// Everything in the start method MUST be done in each sub class!
	protected virtual void Start () {
        ChooseRandomMove();
        box2D = GetComponentInParent<BoxCollider2D>();
        playerPos = FindObjectOfType<Player>().gameObject.transform.position;
        //cam = FindObjectOfType<Camera>();
        cam = Camera.main;
        rend = GetComponentInParent<Renderer>();
    }
	
	protected virtual void Update () {
        DetectSmartBomb();
        DoBehaviour();
        OutOfVertRangeCheck();
    }

    protected enum CurrentState
    {
        Spawning,
        Moving,
        MovingToPlayer,
        MovingToAstronaut,
        LiftingAstronaut,
        Dying = 99
    }

    protected virtual void DoBehaviour()
    {
        /**
         * Stub. This controls the actions an enemy makes according to it's currentState.
         */
    }

    protected virtual void Spawn()
    {
        //Debug.Log("Enemy spawning");
        currentState = (int)CurrentState.MovingToPlayer;
    }

    protected void ChooseRandomMove()
    {
        directionToMove = Random.insideUnitCircle * 2;
    }

    protected virtual void ContinueMoving()
    {
        if(transform.position.y >= maxHeight || transform.position.y <= minHeight)
        {
            ChooseRandomMove();
        }
        if(moveInDirectionCooldown > 0)
        {
            transform.Translate(new Vector2(directionToMove.x * Time.deltaTime, directionToMove.y *Time.deltaTime));
            moveInDirectionCooldown -= 1 * Time.deltaTime;
        }
        else
        {
            ChooseRandomMove();
            moveInDirectionCooldown = Random.Range(5f, 10f);
        }
    }

    protected void MoveTowardsPlayer(float xSpeed, float ySpeed)//TODO this need a redo!
    {
        //Debug.Log("Enemy moving towards player.");
        playerPos = FindObjectOfType<Player>().gameObject.transform.position;
        transform.Translate(AimForPlayer(xSpeed, ySpeed, playerPos) * Time.deltaTime);
    }
    protected void MoveNearPlayer(float xSpeed, float ySpeed)
    {
        float yOffset = -1.5f;
        playerPos = FindObjectOfType<Player>().gameObject.transform.position;

        if (playerPos.y < 0)
            yOffset *= -1;

        playerPos = new Vector2(playerPos.x, playerPos.y + yOffset);
        transform.Translate(AimForPlayer(xSpeed, ySpeed, playerPos) * Time.deltaTime);
    }

    protected void FireAtPlayer()
    {
        //Debug.Log("Enemy shooting at player.");
        lastFired += 1 * Time.deltaTime;
        if (lastFired >= fireCooldown)
        {
            if(Vector2.Distance(transform.position, playerPos) < firingRange)
            {
                float offset = 0.5f;
                if (!Player.movingRight)
                    offset *= -1;
                Instantiate(enemyLaserPrefab, new Vector2(transform.position.x + offset, transform.position.y), Quaternion.identity);
                lastFired = 0f;
            }
        }
    }

    protected void DetectSmartBomb()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            if (!Player.justDied)
            {
                float xPos = transform.position.x;
                playerPos = FindObjectOfType<Player>().gameObject.transform.position;
                float offset = 10;
                Debug.Log("SmartBomb deployed.");
                if (xPos <= playerPos.x + offset && xPos >= playerPos.x - offset)
                {
                    if (GameControl.remainingSmartBombs > 0)
                    {
                        GameControl.smartBombActivated = true;
                        currentState = (int)CurrentState.Dying;
                    }
                }
            }
        }
    }
}
