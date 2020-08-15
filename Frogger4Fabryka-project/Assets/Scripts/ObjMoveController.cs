using UnityEngine;

public class ObjMoveController : MonoBehaviour
{
    public float moveSpeed;
    public float boundaryX;

    // Moving object at given speed and destroys it after reaching boundary
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed * GameValues.objSpeedModifier);

        if (transform.position.x > boundaryX || transform.position.x < -boundaryX)
        {
            Destroy(gameObject);
        }
    }
}
