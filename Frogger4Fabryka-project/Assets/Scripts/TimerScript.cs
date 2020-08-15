using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public bool timerActive = false;
    
    public float maxTime = 25;
    public float timeLeft;
   
    private Image timeBar;
    private GameManager gameManager;

    // Gets reference to the game manager and time bar image
    private void Awake()
    {
        timeBar = GetComponent<Image>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Sets time left to given max time and starts timer
    public void StartTimer()
    {
        timeLeft = maxTime;
        timerActive = true;
    }

    // Decreasing time left and time bar
    void Update()
    {
        if (timerActive && gameManager.isGameActive)
        {
            if (timeLeft > 0f)
            {
                timeLeft -= Time.deltaTime;
                timeBar.fillAmount = timeLeft / maxTime;
            }
            else
            {
                timerActive = false;
                StartCoroutine(gameManager.HealthLoss());
            }
        }
    }
}
