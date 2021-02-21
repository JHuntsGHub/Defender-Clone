using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherShip : Enemy {

    public GameObject swarmerPrefab;
    private float releaseSwarmerCooldown;
    
    protected override void Start()
    {
        base.Start();
        fireCooldown = 1.5f;
        releaseSwarmerCooldown = Random.Range(3, 8);
    }
    
    protected override void Update()
    {
        base.Update();
        ReleaseSwarmer();
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

    protected override void BeingDestroyed()
    {
        Debug.Log("MotherShip is being destroyed.");
        int swarmersToSpawn = Random.Range(5, 7);
        for(int x = 0; x < swarmersToSpawn; x++)
        {
            Vector2 randInCircle = Random.insideUnitCircle * 2;
            Instantiate(swarmerPrefab, new Vector2(transform.position.x + randInCircle.x, transform.position.y + randInCircle.y), Quaternion.identity);
        }
        GameControl.Score += 1000;
        base.BeingDestroyed();
    }

    private void ReleaseSwarmer()
    {
        releaseSwarmerCooldown -= Time.deltaTime;
        if(releaseSwarmerCooldown <= 0)
        {
            releaseSwarmerCooldown = Random.Range(3, 8);
            Instantiate(swarmerPrefab, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        }
    }

    protected override void Spawn()
    {
        //Debug.Log("MotherShip spawning");
        currentState = (int)CurrentState.Moving;
    }
}
