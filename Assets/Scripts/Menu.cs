using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    public void StartPressed()
    {
        Debug.Log("Starting Game.");
        SceneManager.LoadScene("MainGame");
    }

    public void LeadersPressed()
    {
        SceneManager.LoadScene("Leaderboard");
    }

    public void ControlsPressed()
    {
        SceneManager.LoadScene("Controls");
    }

    public void QuitPressed()
    {
        Debug.Log("Closing program.");
        Application.Quit();
    }

    public void MenuPressed()
    {
        SceneManager.LoadScene("Menu");
    }
}
