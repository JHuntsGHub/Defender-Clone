using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsCollider : MonoBehaviour {

    private int offset;

	// Use this for initialization
	void Start () {
        offset = 32;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "BoundsCollider")
        {
            if (Player.movingRight)
            {
                transform.position = new Vector2(transform.position.x + offset, transform.position.y);
            }
            else
            {
                transform.position = new Vector2(transform.position.x - offset, transform.position.y);
            }
        }
    }
}
