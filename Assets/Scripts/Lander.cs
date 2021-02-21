using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lander : Enemy {

	private GameObject astronautBeingTargeted;
    public bool abductingAstronaut { set; get; }
    public GameObject mutantPrefab;

	protected override void Start () {
        base.Start();
        abductingAstronaut = false;
    }


    protected override void Update () {
        base.Update();
        FireAtPlayer();

    }

    protected override void DoBehaviour()
    {
        switch (currentState)
        {
            case (int)CurrentState.Spawning:
                Spawn();
                break;

            case (int)CurrentState.Moving:
                ContinueMoving();
                break;

            case (int)CurrentState.MovingToAstronaut:
                MoveTowardsAstronaut();
                break;

            case (int)CurrentState.LiftingAstronaut:
                LiftAstronaut();
                break;

            case (int)CurrentState.Dying:
                BeingDestroyed();
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.tag == "Astronaut")
        {
            if (!abductingAstronaut && !coll.gameObject.GetComponent<Astronaut>().beingRescued)
            {
                //Debug.Log("An astronaut has been targeted.");
                currentState = (int)CurrentState.MovingToAstronaut;
                astronautBeingTargeted = coll.gameObject;
                astronautBeingTargeted.GetComponent<Astronaut>().beingAbducted = true;
                abductingAstronaut = true;
            }
        }
    }

    private void MoveTowardsAstronaut()
    {
        //Debug.Log("Moving towards Astronaut");
        float astro_xPos;
        try
        {
            astro_xPos = astronautBeingTargeted.transform.position.x;
        }
        catch (MissingReferenceException)
        {
            currentState = (int)CurrentState.Moving;
            return;
        }

        if(Mathf.Round(transform.position.x) == Mathf.Round(astro_xPos) && transform.position.y < minHeight + 0.2)
        {
            //Debug.Log("Astronaut ready for abduction");
            astronautBeingTargeted.transform.parent = gameObject.transform;
            currentState = (int)CurrentState.LiftingAstronaut;
        }
        else
        {
            float xPosition = 0, yPosition = 0;

            if (transform.position.x > astro_xPos)
            {
                xPosition--;
            }
            else if (transform.position.x < astro_xPos)
            {
                xPosition++;
            }
            if (transform.position.y > minHeight)
            {
                yPosition--;
            }

            transform.Translate(new Vector2(xPosition * Time.deltaTime, yPosition * Time.deltaTime));
        }
    }

    private void LiftAstronaut()
    {
        transform.Translate(new Vector2(0, 1.5f * Time.deltaTime));

        if (transform.position.y > maxHeight - 0.2)
        {
            //Debug.Log("Astronaut lifted to top. Mutant spawning.");
            GameControl.numOfAstronauts--;
            GameControl.Score -= 150;
            Instantiate(mutantPrefab, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            Destroy(gameObject);
        }
    }

    protected override void BeingDestroyed()
    {
        if (abductingAstronaut)
        {
            try
            {
                astronautBeingTargeted.GetComponent<Astronaut>().beingAbducted = false;
                astronautBeingTargeted.transform.parent = null;

                if (astronautBeingTargeted.transform.position.y > -2)
                    astronautBeingTargeted.GetComponent<Astronaut>().bigFall = true;
            }
            catch (MissingReferenceException)
            {
                abductingAstronaut = false;
            }
        }
        //Debug.Log("Lander is being destroyed.");
        GameControl.Score += 150;
        GameControl.landersInWave--;
        base.BeingDestroyed();
    }

    protected override void Spawn()
    {
        //Debug.Log("Lander spawning");
        currentState = (int)CurrentState.Moving;
    }

}
