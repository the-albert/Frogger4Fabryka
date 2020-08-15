using UnityEngine;

public class WaterPlatformAttach : MonoBehaviour
{
    private Transform playerPointToMove;

    private void Start()
    {
        playerPointToMove = GameObject.Find("Point To Move").GetComponent<Transform>();
    }

    // If player activated a trigger, platform's transform becomes parent of point to move
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            playerPointToMove.parent = transform;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            playerPointToMove.parent = null;
    }
}
