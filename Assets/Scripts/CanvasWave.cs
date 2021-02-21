using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasWave : MonoBehaviour {

    public Text waveCompleteText, bonusScoreText;
    public GameObject UI_AstroPrefab;
    private float startX, yPos;
    public bool addedBonus { set; get; }

	// Use this for initialization
	void Start () {
        startX = -539.56f;
        yPos = -272f;
	}
	
	// Update is called once per frame
	public void UpdateDisplay() {
        waveCompleteText.text = "Wave " + GameControl.currentWave + " complete!";
        int totalBonus = CountAstronauts() * 150;
        GameControl.Score += totalBonus;
        bonusScoreText.text = "Bonus: " + totalBonus + " for " + CountAstronauts() + " astronauts alive.";
        //DisplayAstronauts(CountAstronauts());
	}

    private int CountAstronauts()
    {
        Astronaut[] astronauts = FindObjectsOfType<Astronaut>();
        //Debug.Log("Found " + astronauts.Length + " astronauts.");
        return astronauts.Length;
    }

    //private void DisplayAstronauts(int numOfAstronauts)
    //{
    //    float xPos = startX;
    //    for(int i = 0; i < numOfAstronauts; i++)
    //    {
    //        Instantiate(UI_AstroPrefab, new Vector2(xPos, yPos), Quaternion.identity);
    //        xPos += 63.56f;
    //    }
    //}
}
