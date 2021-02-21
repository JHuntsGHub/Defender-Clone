using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : Map {

    protected float speed;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        speed = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
        Move();
	}

    protected void Move(){
        PositionCheck();

        if (Player.movingRight){
            transform.Translate(new Vector2(speed* Time.deltaTime, 0));
        }
        else{
            transform.Translate(new Vector2(-speed* Time.deltaTime, 0));
        }
    }
}
