using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class Leaderboard : MonoBehaviour {

    protected const string filename = "lb.txt";
    public Text Leaders;

    void Start() {
        PrintFileContentsToScreen();
    }

    private string[] ReadFile()
    {
        if (!File.Exists(filename))
        {
            Debug.Log("Creating leaderboard file.");
            string[] defaultContents = { "AAA", "10000", "BBB", "5000", "CCC", "2500", "DDD", "2000", "EEE", "1200", "FFF", "600", "GGG", "300", "HHH", "150" };
            File.WriteAllLines(filename, defaultContents);
            return defaultContents;
        }

        string[] fileContents = File.ReadAllLines(filename);
        return fileContents;
    }

    protected string[][] Parsefile()
    {
        string[] fileContents = ReadFile();
        string[] names = new string[8];
        string[] scores = new string[8];

        for (int i = 0; i < 8; i++)
        {
            names[i] = fileContents[i * 2];
            scores[i] = fileContents[i * 2 + 1];
        }

        //for (int i = 0; i < 16; i++)
        //    Debug.Log(names[i]);

        string[][] result = new string[][]{names,scores};
        return result;
    }

    protected void PrintFileContentsToScreen()
    {
        string[][] parsedFile = Parsefile();

        string scores = "";

        for (int i = 0; i < 8; i++)
        {
            scores += parsedFile[0][i] + " " + parsedFile[1][i] + Environment.NewLine;
        }

        Leaders.text = scores;
    }

   

}
