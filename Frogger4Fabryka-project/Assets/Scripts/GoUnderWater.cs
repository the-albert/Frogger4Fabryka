using System.Collections;
using UnityEngine;

public class GoUnderWater : MonoBehaviour
{
    public float submergeSpeed;
    public float timeUnderWater;
    public float timeAboveWater;
    public float upperBound;
    public float lowerBound;

    private bool upperBoundReached;

    private void Start()
    {
        StartCoroutine(MoveUpAndDown());
    }

    // Moves the object up and down with given parameters
    IEnumerator MoveUpAndDown()
    {
        if (transform.position.y >= upperBound && !upperBoundReached)
        {
            yield return new WaitForSeconds(timeAboveWater);
            submergeSpeed *= -1;
            upperBoundReached = true;
        }
        else if (transform.position.y <= lowerBound && upperBoundReached)
        {
            // If player is attached to the platform remove one health
            if (transform.Find("Point To Move") != null)
            {
                StartCoroutine(GameObject.Find("Game Manager").GetComponent<GameManager>().HealthLoss());
            }
            
            yield return new WaitForSeconds(timeUnderWater);
            submergeSpeed *= -1;

            upperBoundReached = false;
        }

        yield return new WaitForSeconds(0.01f);
        transform.Translate(Vector3.up * Time.deltaTime * submergeSpeed);
        StartCoroutine(MoveUpAndDown());
    }
}
