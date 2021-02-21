using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTrail : MonoBehaviour {
    
    private Renderer rend;
    public static bool showTrail { get; set; }
    public static bool trailEnabled { get; set; }

	// Use this for initialization
	void Start () {
        rend = GetComponentInChildren<Renderer>();
        trailEnabled = true;
        showTrail = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (trailEnabled)
        {
            if (showTrail)
            {
                rend.enabled = true;
            }
            else
            {
                rend.enabled = false;
            }
        }
        else
        {
            rend.enabled = false;
        }
	}
}
