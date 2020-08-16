using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool isGameActive = false;
    
    private int levelScore = 0;
    private int winningPointsReached = 0;
    private int highScore = 0;

    
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI popupText;
    public TextMeshProUGUI endgameText;
    public List<Image> healthIcons;
    public GameObject startScreen;
    public AudioClip deathSound;
    public AudioClip winningSound;

    private PlayerController playerScript;
    private TimerScript timerScript;
    private AudioSource audioSource;

    // Sets apropriate number of health icons and disables the start window if needed
    private void Awake()
    {
        highScore = PlayerPrefs.GetInt("HighScore");
        highScoreText.text = "High Score: " + highScore;

        for (int i = 0; i<GameValues.playerHealth; i++)
        {
             healthIcons[i].gameObject.SetActive(true);
        }
        if(GameValues.currentLVL > 1 || GameValues.restarted)
        {
            startScreen.SetActive(false);
            GameValues.restarted = false;
            StartGame();
        }
    }

    // Runs the game 
    public void StartGame()
    {
        isGameActive = true;
        
        playerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        timerScript = GameObject.Find("TimeBar").GetComponent<TimerScript>();
        audioSource = GetComponent<AudioSource>();
        timerScript.StartTimer();

        scoreText.text = "Score: " + levelScore;
    }

    // Changes score value and updates UI score text
    public void AddScore(int points)
    {
        levelScore += points;
        scoreText.text = "Score: " + levelScore;
    }

    // Is called when Player reaches a winning point
    public void WinningPointReached(GameObject winningPoint)
    {
        winningPointsReached++;

        // plays sound and particles
        winningPoint.GetComponentInChildren<ParticleSystem>().Play();
        audioSource.PlayOneShot(winningSound, 1.5f);

        // Blocking the winning point
        winningPoint.GetComponent<BoxCollider>().enabled = false;
        winningPoint.transform.Find("Frog Model").gameObject.SetActive(true);

        // Increasing the score by the time left
        AddScore((int)timerScript.timeLeft * 50 * GameValues.scoreModifier);

        // Reseting timer and Player position
        timerScript.StartTimer();
        playerScript.ResetPlayerPosition();

        // Ends the game after 3 winnig points have been reached
        if (winningPointsReached == 3)
        {
            StartCoroutine(EndGame(true));
        }
    }

    // Removes one health and checks if there are any left
    public IEnumerator HealthLoss()
    {
        playerScript.deathParticle.Play();
        audioSource.PlayOneShot(deathSound, 3f);
        yield return new WaitForSeconds(0.1f);

        GameValues.playerHealth--;
        
        if (GameValues.playerHealth == 0)
        {
            StartCoroutine(EndGame(false));
            Destroy(playerScript.gameObject);
        }
        else
        {
            playerScript.ResetPlayerPosition();
            timerScript.StartTimer();
        }
        healthIcons[GameValues.playerHealth].enabled = false;
    }

    // Loads scene with values for given level
    public void LoadLVL (int lvl)
    {
        GameValues.SetValuesForLvl(lvl);

        if(lvl == 1)
        {
            GameValues.restarted = true;
        }
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Quits the game 
    public void QuitGame()
    {
        Application.Quit();
    }

    // Manages situations where player lost all lives or won 
    IEnumerator EndGame(bool won)
    {
        isGameActive = false;
        GameValues.wholeScore += levelScore;

        if (won)
        {
            popupText.text = "You completed level " + GameValues.currentLVL + "\n";
            popupText.text += "You scored: " + levelScore + "\n";
            popupText.text += "Your whole score: " + GameValues.wholeScore;

            popupText.transform.parent.gameObject.SetActive(true);

            while (!isGameActive)
            {
                if (Input.anyKeyDown)
                {
                    LoadLVL(GameValues.currentLVL + 1);
                }
                yield return null;
            }
        }
        else
        {
            // check if player scored more than last highscore
            if(GameValues.wholeScore > highScore)
            {
                PlayerPrefs.SetInt("HighScore", GameValues.wholeScore);
            }
            endgameText.text = "You reached level " + GameValues.currentLVL + " and scored: " + GameValues.wholeScore;
            endgameText.transform.parent.gameObject.SetActive(true);
        }
    }
}

