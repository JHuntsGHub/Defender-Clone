using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HandleScore : Leaderboard {

    private string playerName;
    public Canvas Victory, Loser;
    public Text ScoreText1, ScoreText2;

	// Use this for initialization
	void Start () {
        playerName = null;
        ScoreText1.text = "" + DataTransfer.Score;
        ScoreText2.text = "" + DataTransfer.Score;

        if (CheckScore(DataTransfer.Score))
        {
            Victory.enabled = true;
            Loser.enabled = false;
        }
        else
        {
            Victory.enabled = false;
            Loser.enabled = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(playerName != null)
        {
            InsertScore();
            SceneManager.LoadScene("Leaderboard");
        }
	}

    //returns true if a high score was entered. 
    private bool CheckScore(int score)
    {
        string[][] parsedFile = Parsefile();
        int[] scores = ConvertToInt(parsedFile[1]);

        //check if the score is greater than an existing high score.
        if (score > scores[7])
        {
            return true;
        }
        return false;
    }

    private int[] ConvertToInt(string[] stringScores)
    {
        int[] intScores = new int[16];

        for (int i = 0; i < 8; i++)
            intScores[i] = 0 + int.Parse(stringScores[i]);

        return intScores;
    }

    public void GetName(string name)
    {
        playerName = name;
        Debug.Log("playerName changed to " + name);
    }

    private void InsertScore()
    {
        string[][] parsedFile = Parsefile();
        int[] scores = ConvertToInt(parsedFile[1]);

        parsedFile[0][7] = playerName;
        scores[7] = DataTransfer.Score;

        for(int i = 7; i >= 1; i--)
        {
            if (scores[i] > scores[i - 1])
            {
                string tempS = parsedFile[0][i];
                int tempI = scores[i];

                parsedFile[0][i] = parsedFile[0][i - 1];
                scores[i] = scores[i - 1];

                parsedFile[0][i - 1] = tempS;
                scores[i - 1] = tempI;
            }
            else
                break;
        }

        string[] newLeaderboard = new string[16];
        for (int i = 0; i < 8; i++)
        {
            newLeaderboard[i * 2] = parsedFile[0][i];
            newLeaderboard[i * 2 + 1] = "" + scores[i];
        }
        File.WriteAllLines(filename, newLeaderboard);
    }
}
