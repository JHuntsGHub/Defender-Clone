using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mutant : Enemy {

    // Use this for initialization
    protected override void Start () {
        base.Start();
        fireCooldown = 2;
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
                MoveNearPlayer(5, 2);
                break;

            case (int)CurrentState.Dying:
                BeingDestroyed();
                break;
        }
    }

    protected override void BeingDestroyed()
    {
        //Debug.Log("Mutant is being destroyed.");
        GameControl.Score += 150;
        GameControl.landersInWave--;
        base.BeingDestroyed();
    }

    protected override void Spawn()
    {
        //Debug.Log("Mutant spawning");
        currentState = (int)CurrentState.MovingToPlayer;
    }
}
