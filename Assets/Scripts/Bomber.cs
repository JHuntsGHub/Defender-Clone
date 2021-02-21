using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : Enemy {

    private bool leadBomber;
    private float bombCooldown;
    public GameObject BombPrefab;
    
    protected override void Start () {
        base.Start();
    }
    
    // Update is called once per frame
    protected override void Update () {
        base.Update();
    }

    protected override void DoBehaviour()
    {
        switch (currentState)
        {
            case (int)CurrentState.Spawning:
                Spawn();
                break;

            case (int)CurrentState.Moving:
                ContinueMoving();
                break;

            case (int)CurrentState.Dying:
                BeingDestroyed();
                break;
        }
    }

    private void DropBomb()
    {
        bombCooldown -= Time.deltaTime;
        if (bombCooldown <= 0)
        {
            Instantiate(BombPrefab, transform.position, Quaternion.identity);
            bombCooldown = SetBombCooldown();
        }
    }

    protected override void ContinueMoving()
    {
        base.ContinueMoving();
        DropBomb();
    }

    protected override void BeingDestroyed()
    {
        //Debug.Log("Bomber is being destroyed.");
        GameControl.Score += 250;
        base.BeingDestroyed();
    }

    protected override void Spawn()
    {
        AssignLeadBomber();
        bombCooldown = SetBombCooldown();
        //Debug.Log("Bomber spawning");
        currentState = (int)CurrentState.Moving;
    }

    private float SetBombCooldown()
    {
        return UnityEngine.Random.Range(7, 14); ;
    }

    private void AssignLeadBomber()
    {

    }
}
