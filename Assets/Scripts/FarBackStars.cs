using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarBackStars : Stars {

	// Use this for initialization
	protected override void Start () {
        base.Start();
        speed = 2f;
	}
	
	// Update is called once per frame
	void Update () {
        Move();
	}
}
