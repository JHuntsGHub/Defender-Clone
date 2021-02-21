using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astronaut : MonoBehaviour {

    public bool beingAbducted { set; get; }
    public bool beingRescued { set; get; }
    public bool bigFall { set; get; }
    private float changeDirectionCooldown;
    private int direction;
    private Quaternion rotation;

    
	void Start () {
        changeDirectionCooldown = Random.Range(2f, 5f);
        beingAbducted = false;
        beingRescued = false;
        direction = StartingDirection();
        rotation.eulerAngles = new Vector3(0, direction, 0);
        transform.rotation = rotation;
	}
	
	void Update () {

        if (!beingAbducted && !beingRescued)
        {
            if (transform.position.y > -4)
            {
                transform.Translate(new Vector2(0, -2.5f * Time.deltaTime));
            }
            else if(transform.position.y < -4)
            {
                transform.position = new Vector2(transform.position.x, -4);
            }
            else
            {
                /**
                 * This controlls the astronauts movement.
                 * The astronaut only moves left and right.
                 */
                if (changeDirectionCooldown < 0)
                {
                    direction = ChangeDirection();
                    rotation.eulerAngles = new Vector3(0, direction, 0);
                    transform.rotation = rotation;
                    changeDirectionCooldown = Random.Range(2f, 5f);
                }
                changeDirectionCooldown -= Time.deltaTime;
                transform.Translate(new Vector2(Time.deltaTime, 0));

                if (bigFall)
                {
                    GameControl.numOfAstronauts--;
                    GameControl.Score -= 150;
                    Debug.Log("Astronaut fell to death.");
                    Destroy(gameObject);
                }
            }
        }
    }

    private int ChangeDirection()
    {
        if(direction == 180)
        {
            return 0;
        }
        else
        {
            return 180;
        }
    }

    /**
     * This is called during the start method to allow the astronaut to face a random direction at initialisation.
     */
    private int StartingDirection()
    {
        int x = (int)Random.Range(0f, 10f);
        if(x > 5)
        {
            return 180;
        }
        else
        {
            return 0;
        }
    }
}
