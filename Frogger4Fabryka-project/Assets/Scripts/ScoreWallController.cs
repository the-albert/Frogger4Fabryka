using UnityEngine;

public class ScoreWallController : MonoBehaviour
{
    public int scoreValue;
    public int currentRow;

    private Vector3 startPos;
    private GameManager gameManager;

    // Sets the trigger wall at the first row
    void Start()
    {
        startPos = transform.position;
        ResetWall();

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Move score wall if player enters and add points
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            transform.position += Vector3.forward;
            
            // Skip grass field and move to the water
            if (currentRow == 5)
                transform.position += Vector3.forward;
            
            gameManager.AddScore(scoreValue);
            currentRow++;
            scoreValue = 10 * currentRow * GameValues.scoreModifier;

            // Restart wall for next turn
            if (currentRow == 11)
            {
                ResetWall();
            }
        }
    }

    // Moves wall to the first row and resets the score value
    void ResetWall()
    {
        scoreValue = 10;
        currentRow = 1;
        transform.position = startPos;
    }
}
