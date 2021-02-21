using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : Entity {

    private Vector2 firingDirection;
    private float laserSpeed;
    
    // Use this for initialization
    protected void Start () {//TODO Add force as the propultion method instead of this way. This will allow for different speed lasers.
        playerPos = FindObjectOfType<Player>().gameObject.transform.position;
        firingDirection = TargetVector2D();
        laserSpeed = 11;
    }
	
	/**
     * Fires the enemies laser along the given path.
     */
	protected void Update () {
        transform.Translate(firingDirection * laserSpeed * Time.deltaTime);

        if(transform.position.y < minHeight + 0.2)
        {
            Destroy(gameObject);
        }
	}

    protected virtual Vector2 TargetVector2D()
    {
        return (playerPos - (Vector2)transform.position).normalized;
    }

    protected void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
