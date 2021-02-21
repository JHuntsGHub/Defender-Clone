using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantLaser : EnemyLaser {

	// Use this for initialization
	void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}

    protected override Vector2 TargetVector2D()
    {
        float yOffset = 1.5f;
        int x = Random.Range(-5, 5);
        if (x > 0)
            yOffset *= -1;
        playerPos = new Vector2(playerPos.x, playerPos.y + yOffset);

        return (playerPos - (Vector2)transform.position).normalized;
    }

    void OnBecameInvisible()
    {
        base.OnBecameInvisible();
    }
}
