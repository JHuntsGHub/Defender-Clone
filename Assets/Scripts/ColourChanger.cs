using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColourChanger : MonoBehaviour {

    private Color colour;
    public SpriteRenderer defenderImage;
    public float r, g, b;
    public int state;
    private float wait, waitTime;

	void Start () {
        colour = defenderImage.color;
        waitTime = 0.01f;
        wait = waitTime;
        //state = (int)State.redDOWN;
    }
	
	void Update () {
        ChangeColour();
	}

    private void ChangeColour()
    {
        if(wait <= 0)
        {
            wait = waitTime;

            switch (state)
            {
                case (int)State.redUP:
                    if (r >= 0.95f)
                    {
                        state = (int)State.blueUP;
                    }
                    else
                        r += 0.05f;
                    break;
                case (int)State.blueUP:
                    if (b >= 0.95f)
                    {
                        state = (int)State.greenUP;
                    }
                    else
                        b += 0.05f;
                    break;
                case (int)State.greenUP:
                    if (g >= 0.95f)
                    {
                        state = (int)State.redDOWN;
                    }
                    else
                        g += 0.05f;
                    break;
                case (int)State.redDOWN:
                    if (r <= 0.05f)
                    {
                        state = (int)State.greenDOWN;
                    }
                    else
                        r -= 0.05f;
                    break;
                case (int)State.greenDOWN:
                    if (g <= 0.05f)
                    {
                        state = (int)State.blueDOWN;
                    }
                    else
                        g -= 0.05f;
                    break;
                case (int)State.blueDOWN:
                    if (b <= 0.05f)
                    {
                        state = (int)State.redUP;
                    }
                    else
                        b -= 0.05f;
                    break;
            }
            colour = new Color(r, g, b);
            defenderImage.color = colour;
        }
        else
        {
            wait -= Time.deltaTime;
        }
    }

    private enum State
    {
        redUP = 1,
        greenUP,
        blueUP,
        redDOWN,
        greenDOWN,
        blueDOWN
    }
}
