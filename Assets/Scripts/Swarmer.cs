using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swarmer : Enemy {

    private float xSpeed, ySpeed;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        fireCooldown = 1.5f;
        xSpeed = Random.Range(4, 7);
        ySpeed = Random.Range(2, 4);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void DoBehaviour()
    {
        switch (currentState)
        {
            case (int)CurrentState.Spawning:
                Spawn();
                break;

            case (int)CurrentState.MovingToPlayer:
                MoveTowardsPlayer(xSpeed, ySpeed);
                break;

            case (int)CurrentState.Dying:
                BeingDestroyed();
                break;
        }
    }

    protected override void BeingDestroyed()
    {
        //Debug.Log("Swarmer is being destroyed.");
        GameControl.Score += 150;
        base.BeingDestroyed();
    }

    protected override void Spawn()
    {
        //Debug.Log("Swarmer spawning");
        currentState = (int)CurrentState.MovingToPlayer;
    }
}
