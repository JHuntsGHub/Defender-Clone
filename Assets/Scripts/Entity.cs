using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

    protected float maxHeight = 2.5f, minHeight = -3.509964f;  //No entity can ever be allowed to go outside of this range.
    protected Vector2 playerPos;
    public GameObject explosionPrefab;

    /*
     * only overriden versions of this should be called.
     */
    virtual protected void BeingDestroyed()
    {
        //TODO particle effects.
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    /**
     * OutOfVertRangeCheck() detects if the entity is above or below the allowed vertical positions. If so, it is corrected.
     */
    protected virtual void OutOfVertRangeCheck()
    {
        if (transform.position.y > maxHeight)
        {
            transform.position = new Vector2(transform.position.x, maxHeight);
        }
        if (transform.position.y < minHeight)
        {
            transform.position = new Vector2(transform.position.x, minHeight);
        }
    }

    /**
     * Returns a vector in the players direction in the speed passed into the paramaters.
     */
    protected Vector2 AimForPlayer(float xSpeed, float ySpeed, Vector2 playerPosition)
    {
        float xPos = 0, yPos = 0;
        if (transform.position.x > playerPosition.x)
        {
            xPos = -xSpeed;
        }
        else
        {
            xPos = xSpeed;
        }
        if (transform.position.y > playerPosition.y)
        {
            yPos = -ySpeed;
        }
        else
        {
            yPos = ySpeed;
        }
        return new Vector2(xPos, yPos);
    }

}
