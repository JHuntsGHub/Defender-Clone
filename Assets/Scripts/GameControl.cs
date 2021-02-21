using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameControl : MonoBehaviour {
    
    public GameObject PlayerPrefab, Astronaut, Lander, Mutant, Baiter, Bomber, Mothership, Swarmer, PlanetExplosion;
    public static int remainingLives { get; set; }
    public static int remainingSmartBombs { get; set; }
    public static bool playerIsInvincible { get; set; }
    public static int numOfAstronauts { get; set; }
    public static bool smartBombActivated {get;set;}
    private Camera cam;
    private int enemyInWaveToSpawn;
    public static int currentWave { get; set; }
    private int[] enemies;
    public static int landersInWave { get; set; }
    public static int Score { get; set; }
    private float enemySpawnCooldown, enemySpawnCooldownLength, waveTransitionScreenCooldownLength, waveTransitionScreenCooldown;
    private GameObject[] playerLives = new GameObject[8];
    private Text waveText, scoreText;
    private int nextMilestoneForExtraLife;
    private bool waveTransitionScreenOn, planetExploded;
    public Canvas canvas_wave;


    void Start () {
        waveTransitionScreenCooldownLength = 5f;
        waveTransitionScreenCooldown = waveTransitionScreenCooldownLength;
        waveTransitionScreenOn = false;
        nextMilestoneForExtraLife = 10000;
        FindTextObjects();
        GetLifeAnimations();
        playerIsInvincible = false;
        enemySpawnCooldown = enemySpawnCooldownLength;
        enemySpawnCooldownLength = 3;
        currentWave = (int)CurrentWave.Wave1;
        enemies = getEnemiesForWave();
        numOfAstronauts = 15;
        PopulateWithAstronauts();
        Score = 0;
        remainingLives = 3;
        remainingSmartBombs = 3;
        smartBombActivated = false;
        cam = Camera.main;
        planetExploded = false;
	}
	
	void Update () {
        Cheats();
        HandleSpawning();
        WaveOverCheck();
        WaveText();
	}

    void LateUpdate()
    {
        UpdateScoreDisplay();
        SmartBomb();
        LifeOnUICheck();
        WaveTransitionScreen();
        ExplodePlanet();
    }

    private void WaveOverCheck()
    {
        if(landersInWave <= 0)
        {
            if(currentWave == (int)CurrentWave.Wave6)
            {
                Debug.Log("Wave 6 complete! You are victorious.");
                GoToGameOver();
            }
            else
            {
                waveTransitionScreenOn = true;
                DestroyRemainingEnemies();
                //Debug.Log("Wave Complete!! Moving to wave " + currentWave);
            }
        }
    }

    private void DestroyRemainingEnemies()
    {
        foreach (GameObject go in FindObjectsOfType<GameObject>())
        {
            if (go.tag == "Enemy")
            {
                Destroy(go);
            }
        }
    }

    private void WaveTransitionScreen()
    {
        if (waveTransitionScreenOn)
        {
            if(waveTransitionScreenCooldown == waveTransitionScreenCooldownLength)
            {
                canvas_wave.GetComponent<Canvas>().enabled = true;
                canvas_wave.GetComponent<CanvasWave>().UpdateDisplay();
            }

            waveTransitionScreenCooldown -= Time.deltaTime;

            if(waveTransitionScreenCooldown <= 0)
            {
                currentWave++;
                enemies = getEnemiesForWave();
                waveTransitionScreenCooldown = waveTransitionScreenCooldownLength;
                waveTransitionScreenOn = false;
            }
            
        }
        else
        {
            canvas_wave.GetComponent<Canvas>().enabled = false;
        }
    }

    private void HandleSpawning()
    {
        enemySpawnCooldown -= Time.deltaTime;
        if (!waveTransitionScreenOn && enemySpawnCooldown <= 0)
        {
            enemySpawnCooldown = enemySpawnCooldownLength;

            if (enemies.Length -1 >= enemyInWaveToSpawn)
            {
                Debug.Log("Spawning next enemy in wave " + currentWave + ". Enemy: " + (enemyInWaveToSpawn + 1));

                float playerXPos = FindObjectOfType<Player>().transform.position.x;
                if(Random.Range(-5, 5) < 0)
                    playerXPos += Random.Range(-15, -5);
                else
                    playerXPos += Random.Range(5, 15);

                Instantiate(EnemyToSpawn(enemies[enemyInWaveToSpawn]), new Vector2(playerXPos, Random.Range(-1f, 2.5f)), Quaternion.identity);
                enemyInWaveToSpawn++;
            }
            else
            {
                Debug.Log("Out of enemies to spawn.");
            }
        }
    }

    private GameObject EnemyToSpawn(int enemy)
    {
        switch (enemy)
        {
            case (int)EnemyType.Lander:
                return Lander;
            case (int)EnemyType.Mutant:
                return Mutant;
            case (int)EnemyType.Baiter:
                return Baiter;
            case (int)EnemyType.Bomber:
                return Bomber;
            case (int)EnemyType.Mothership:
                return Mothership;
            case (int)EnemyType.Swarmer:
                return Swarmer;
            default:
                return Lander;
        }
    }

    private enum CurrentWave
    {
        Wave1 = 1,
        Wave2,
        Wave3,
        Wave4,
        Wave5,
        Wave6
    }

    private enum EnemyType
    {
        Lander,
        Mutant,
        Baiter,
        Bomber,
        Mothership,
        Swarmer
    }

    private void PopulateWithAstronauts()
    {
        for(int x = 1; x <= numOfAstronauts; x++)
        {
            Instantiate(Astronaut, new Vector2(Random.Range(-15f, 15f), -4), Quaternion.identity);
        }
    }

    private void SmartBomb()
    {
        if (smartBombActivated)
        {
            if (!playerIsInvincible)
            {
                if (remainingSmartBombs > 0)
                {
                    foreach(GameObject go in FindObjectsOfType<GameObject>())
                    {
                        if(go.tag == "SmartBomb")
                        {
                            if (go.name == "UIbomb" + remainingSmartBombs)
                                go.GetComponent<Image>().enabled = false;
                        }
                    }
                    remainingSmartBombs--;
                }
            }
            smartBombActivated = false;
        }
    }

    private void GetLifeAnimations()
    {
        int currIndex = 0;
        foreach (GameObject go in FindObjectsOfType<GameObject>())
        {
            if (go.tag == "PlayerLife")
            {
                //Debug.Log("Adding " + go.name);
                playerLives[currIndex] = go;
                currIndex++;
            }
        }
    }
    

    /**
     * Displays the correct number of lives on the UI
     */ 
    private void LifeOnUICheck()
    {
        foreach(GameObject go in playerLives)
        {
            char[] charsInName = go.name.ToCharArray();
            char lastChar = charsInName[charsInName.Length - 1];

            if((int)(lastChar - '0') <= remainingLives)
            {
                //Debug.Log("Enabling animation for " + go.name);
                go.GetComponent<Image>().enabled = true;
            }
            else
            {
                //Debug.Log("Disabling animation for " + go.name);
                go.GetComponent<Image>().enabled = false;
            }
        }
    }

    void OnGUI()
    {
        if (playerIsInvincible)
        {
            GUI.Box(new Rect(10, cam.pixelHeight - 30, 100, 20), "Invincible");
        }

    }

    private int[] getEnemiesForWave()
    {
        switch (currentWave)
        {
            case (int)CurrentWave.Wave1:
                //Wave 1 contains: 7 Landers
                landersInWave = 7;
                enemyInWaveToSpawn = 0;
                return new int[] { (int)EnemyType.Lander, (int)EnemyType.Lander, (int)EnemyType.Lander, (int)EnemyType.Lander, (int)EnemyType.Lander, (int)EnemyType.Lander, (int)EnemyType.Lander };
            case (int)CurrentWave.Wave2:
                //Wave 2 contains: 10 Landers, 1 Mothership
                landersInWave = 10;
                enemyInWaveToSpawn = 0;
                return new int[] { (int)EnemyType.Lander, (int)EnemyType.Lander, (int)EnemyType.Lander, (int)EnemyType.Lander, (int)EnemyType.Lander, (int)EnemyType.Mothership, (int)EnemyType.Lander, (int)EnemyType.Lander, (int)EnemyType.Lander, (int)EnemyType.Lander, (int)EnemyType.Lander };
            case (int)CurrentWave.Wave3:
                //Wave 3 contains: 10 Landers, 3 Motherships, 1 Bomber
                landersInWave = 10;
                enemyInWaveToSpawn = 0;
                return new int[] { (int)EnemyType.Lander, (int)EnemyType.Lander, (int)EnemyType.Lander, (int)EnemyType.Mothership, (int)EnemyType.Lander, (int)EnemyType.Lander, (int)EnemyType.Mothership, (int)EnemyType.Lander, (int)EnemyType.Bomber, (int)EnemyType.Lander, (int)EnemyType.Mothership, (int)EnemyType.Lander, (int)EnemyType.Lander, (int)EnemyType.Lander };
            case (int)CurrentWave.Wave4:
                //Wave 4 contains: 10 Landers, 4 Motherships, 1 Bomber
                landersInWave = 10;
                enemyInWaveToSpawn = 0;
                return new int[] { (int)EnemyType.Lander, (int)EnemyType.Lander, (int)EnemyType.Mothership, (int)EnemyType.Bomber, (int)EnemyType.Lander, (int)EnemyType.Lander, (int)EnemyType.Lander, (int)EnemyType.Mothership, (int)EnemyType.Lander, (int)EnemyType.Lander, (int)EnemyType.Mothership, (int)EnemyType.Mothership, (int)EnemyType.Lander, (int)EnemyType.Lander, (int)EnemyType.Lander };
            case (int)CurrentWave.Wave5:
                //Wave 5 and six spawn enemies at a faster rate!!
                enemySpawnCooldownLength -= 1;
                //Wave 5 contains: 10 Landers, 5 Motherships, 2 Bombers, 1 Baiter
                landersInWave = 10;
                enemyInWaveToSpawn = 0;
                return new int[] { (int)EnemyType.Mothership, (int)EnemyType.Lander, (int)EnemyType.Lander, (int)EnemyType.Bomber, (int)EnemyType.Bomber, (int)EnemyType.Lander, (int)EnemyType.Lander, (int)EnemyType.Mothership, (int)EnemyType.Lander, (int)EnemyType.Lander, (int)EnemyType.Mothership, (int)EnemyType.Mothership, (int)EnemyType.Lander, (int)EnemyType.Lander, (int)EnemyType.Mothership, (int)EnemyType.Baiter, (int)EnemyType.Lander, (int)EnemyType.Lander };
            case (int)CurrentWave.Wave6:
                //Wave 6 contains: 12 Landers, 6 Motherships, 3 Bombers, 2 Baiters
                landersInWave = 12;
                enemyInWaveToSpawn = 0;
                return new int[] { (int)EnemyType.Mothership, (int)EnemyType.Lander, (int)EnemyType.Lander, (int)EnemyType.Mothership, (int)EnemyType.Lander, (int)EnemyType.Bomber, (int)EnemyType.Bomber, (int)EnemyType.Bomber, (int)EnemyType.Lander, (int)EnemyType.Lander, (int)EnemyType.Mothership, (int)EnemyType.Lander, (int)EnemyType.Lander, (int)EnemyType.Lander, (int)EnemyType.Mothership, (int)EnemyType.Mothership, (int)EnemyType.Lander, (int)EnemyType.Lander, (int)EnemyType.Baiter, (int)EnemyType.Mothership, (int)EnemyType.Baiter, (int)EnemyType.Lander, (int)EnemyType.Lander };
        }
        return null;
    }

    private void Cheats()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetButtonDown("Cheat"))
        {
            playerIsInvincible = !playerIsInvincible;
            string x = "";
            if (!playerIsInvincible)
                x = "not ";
            Debug.Log("Player is " + x + "invincible.");
        }

        if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetButtonDown("Reset"))
        {
            SceneManager.LoadScene("MainGame");
        }

        if (Input.GetKeyDown(KeyCode.KeypadMinus) || Input.GetButtonDown("Quit"))
        {
            Application.Quit();
        }
    }


    /**
     * This finds each of the two text objects on the UI and initializes them.
     */
    private void FindTextObjects()
    {
        Text[] textObjects = FindObjectsOfType<Text>();
        foreach(Text text in textObjects)
        {
            if (text.tag == "WaveText")
            {
                waveText = text;
            }
            if (text.tag == "ScoreText")
            {
                scoreText = text;
            }
        }
    }


    /**
     * This updates the text on the top right that displays the current wave.
     */
    private void WaveText()
    {
        if (remainingLives <= 0)
        {
            waveText.text = "Game Over";
            waveText.fontSize = 40;
        }
        else if (currentWave == (int)CurrentWave.Wave6 && landersInWave <= 0)
            waveText.text = "Victory!";
        else
            waveText.text = "Wave: " + currentWave;
    }

    /**
     * This updates the score text. It also gives the player an extra life every 10,000 points.
     */
    private void UpdateScoreDisplay()
    {
        scoreText.text = "Score: " + Score;
        if (Score >= nextMilestoneForExtraLife && remainingLives <= 8)
        {
            remainingLives++;
            nextMilestoneForExtraLife += 10000;
        }
    }

    private void ExplodePlanet()
    {
        if(!planetExploded && numOfAstronauts <= 0)
        {
            Debug.Log("Planet Exploding");
            Instantiate(PlanetExplosion, new Vector2(FindObjectOfType<Player>().transform.position.x, -4), Quaternion.identity);
            foreach (GameObject go in FindObjectsOfType<GameObject>())
            {
                if (go.tag == "Planet")
                {
                    Destroy(go);
                }
            }
            planetExploded = true;
        }
    }

    public static void GoToGameOver()
    {
        DataTransfer.Score = Score;
        SceneManager.LoadScene("GameOver");
    }
}
