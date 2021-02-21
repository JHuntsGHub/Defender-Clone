using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {
    
    protected Camera cam;
    
	protected virtual void Start () {
        //cam = FindObjectOfType<Camera>();
        cam = Camera.main;
	}
	
	/*
     * Every update, we want to see how far the x coordinate is from the player's x coordinate.
     * If it is too far, we update this backgrounds image.
     */
	void Update () {
        PositionCheck();
	}

    protected void PositionCheck()
    {
        if (cam.transform.position.x > transform.position.x + 32)
        {
            transform.position = new Vector2(transform.position.x + 64, 0);
        }
        else if (cam.transform.position.x < transform.position.x - 32)
        {
            transform.position = new Vector2(transform.position.x - 64, 0);
        }
    }
}
