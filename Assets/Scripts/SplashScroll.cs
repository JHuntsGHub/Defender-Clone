using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScroll : MonoBehaviour {

    private int scrollLength, scrollSpeed;
    private Vector2 startPos;
    private float countDownToMainMenu;
    private bool countDownBegun;
    
	void Start () {
        scrollLength = 10;
        scrollSpeed = 4;
        startPos = transform.position;
        countDownToMainMenu = 7f;
        countDownBegun = false;
    }
	
	void Update () {
        if (transform.position.y <= startPos.y + 10)
        {
            transform.Translate(new Vector2(0, scrollSpeed * Time.deltaTime));
            countDownBegun = true;
        }

        if (countDownBegun)
        {
            countDownToMainMenu -= Time.deltaTime;
            if(countDownToMainMenu <= 0)
                SceneManager.LoadScene("Menu");
        }    
	}
}
