using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 8f;
    public float boundaryX = 7f;
    public float boundaryZ = 5.95f;
    public float waterStart = 0.7f;

    private  float horizontalInput;
    private  float verticalInput;
    private  bool playerMoving;

    private bool onPlatform;

    public Transform pointToMove;
    private Vector3 startPos;

    private GameManager gameManager;
    private Animator playerAnim;
    private AudioSource audioSource;

    public ParticleSystem deathParticle;
    public AudioClip jumpSound;

    // Sets starting values and unparents "point to move"
    void Start()
    {
        startPos = transform.position;
        pointToMove.parent = null;
        onPlatform = false;

        playerAnim = GetComponent<Animator>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        audioSource = GetComponent<AudioSource>();
    }

    // Checks input and player position every frame
    void Update()
    {
        // Manage the player movement only when game is running
        if (gameManager.isGameActive)
        {
            // Getting Input
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            // Move frog (player) into direction of pointToMove
            transform.position = Vector3.MoveTowards(transform.position, pointToMove.position, playerSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, pointToMove.position) == 0f)
            {
                // checks if player stopped moving before starting to move again
                if ((Math.Abs(horizontalInput) == 1f || Math.Abs(verticalInput) == 1f) && !playerMoving)
                {
                    StartCoroutine(MovePlayer());
                }

                // Making sure player doesn't get off the map
                pointToMove.position = new Vector3
                    (
                        Mathf.Clamp(pointToMove.position.x, -boundaryX, boundaryX),
                        Mathf.Clamp(pointToMove.position.y, startPos.y, startPos.y),
                        Mathf.Clamp(pointToMove.position.z, -boundaryZ, boundaryZ)
                    );
            }

            // Checking if player fell into the water
            if (transform.position.z > waterStart && !onPlatform && !deathParticle.IsAlive())
            {
                StartCoroutine(gameManager.HealthLoss());
            }
        }
    }

    // Moving point where player needs to move in given direction one field at a time
    IEnumerator MovePlayer()
    {
        playerMoving = true;
        
        // moving "point to move" and rotating the player into the inputted direction
        if (Math.Abs(horizontalInput) == 1f)
        {
            pointToMove.position += new Vector3(horizontalInput, 0, 0);
            transform.Rotate(new Vector3(0, horizontalInput * 90, 0));
        }
        else if (Math.Abs(verticalInput) == 1f)
        {
            pointToMove.position += new Vector3(0, 0, verticalInput);
            if (verticalInput < 0)
            {
                transform.Rotate(new Vector3(0, 180, 0));
            }
        }

        // Playing Jumping Animation and sound
        playerAnim.Play("Jump");
        audioSource.PlayOneShot(jumpSound, 1.5f);
        yield return new WaitForSeconds(0.2f);

        // Resetting player rotation and animation
        transform.rotation = new Quaternion(0,0,0,0);
        playerAnim.Play("Idle");
        playerMoving = false;
    }

    // Checks if Player collided with car or reached winning point
    private void OnTriggerEnter(Collider other)
    {
        // Player collided with the car
        if (other.gameObject.CompareTag("Vehicle"))
        {
            StartCoroutine(gameManager.HealthLoss());
        }

        // Player reached winning point
        else if (other.gameObject.CompareTag("Winning Point"))
        {
            gameManager.WinningPointReached(other.gameObject);
        }
    }

    // Checks if player is on platform
    private void OnTriggerStay(Collider other)
    {
        // Checking if player is on platform
        if (other.gameObject.CompareTag("Platform") ||
            other.gameObject.CompareTag("Turtle Platform"))
        {
            onPlatform = true;
        }
    }

    // Checks if player got off the platform
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Platform") ||
            other.gameObject.CompareTag("Turtle Platform"))
        {
            onPlatform = false;
        }
    }

    // Moving player and "point to move" to start position
    public void ResetPlayerPosition()
    {
        if (gameManager.isGameActive)
        {
            pointToMove.parent = null;
            pointToMove.position = startPos;
            transform.position = startPos;
        }
    }
}
