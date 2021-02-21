using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    public AudioClip pewpew;
    AudioSource laserAudio;
    private float speed;
    private float startingXPosition;
    private const float maxDistance = 25;


	void Start () {
        laserAudio = GetComponentInParent<AudioSource>();
        speed = 20f;
        if (!Player.movingRight) { speed *= -1; }
        startingXPosition = transform.position.x;
        laserAudio.PlayOneShot(pewpew);
    }
	
	void Update () {
        transform.Translate(new Vector2(speed * Time.deltaTime, 0));
        
        if (transform.position.x > startingXPosition + maxDistance || transform.position.x < startingXPosition - maxDistance)
        {
            Destroy(gameObject);
        }

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "Astronaut")
        {
            GameControl.numOfAstronauts--;
            GameControl.Score -= 150;
            coll.gameObject.GetComponentInParent<Lander>().currentState = 1; // 1 is the move behaviour.
            coll.gameObject.GetComponentInParent<Lander>().abductingAstronaut = false;
            Destroy(coll.gameObject);
            Destroy(gameObject);
        }

        if (coll.gameObject.tag == "Enemy" || coll.gameObject.tag == "Lander"){
            coll.gameObject.GetComponent<Enemy>().currentState = 99; //99 is the death state!
            Destroy(gameObject);
        }
        
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}