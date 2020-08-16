using UnityEngine;

public class ObjMoveController : MonoBehaviour
{
    public float moveSpeed;
    public float boundaryX;

    // Moving object at given speed and destroys it after reaching boundary
    void Update()
    {
        if (gameObject.CompareTag("Vehicle"))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed * GameValues.vehicleSpeedModifier);
        }
        else if(gameObject.CompareTag("Platform") || gameObject.CompareTag("Turtle Platform"))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed * GameValues.platformSpeedModifier);
        }

        if (transform.position.x > boundaryX || transform.position.x < -boundaryX)
        {
            Destroy(gameObject);
        }
    }
}
