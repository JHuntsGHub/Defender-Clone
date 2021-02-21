using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baiter : Enemy {

    // Use this for initialization
    protected override void Start () {
        base.Start();
        fireCooldown = 1.5f;
    }

    // Update is called once per frame
    protected override void Update () {
        base.Update();
        FireAtPlayer();
    }

    protected override void DoBehaviour()
    {
        switch (currentState)
        {
            case (int)CurrentState.Spawning:
                Spawn();
                break;

            case (int)CurrentState.MovingToPlayer:
                MoveNearPlayer(10, 3);
                break;

            case (int)CurrentState.Dying:
                BeingDestroyed();
                break;
        }
    }

    protected override void BeingDestroyed()
    {
        //Debug.Log("Baiter is being destroyed.");
        GameControl.Score += 200;
        base.BeingDestroyed();
    }

    protected override void Spawn()
    {
        //Debug.Log("Baiter spawning");
        currentState = (int)CurrentState.MovingToPlayer;
    }

}
