using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5;

    void Update()
    {

        Vector2 moveAmount = new Vector2(movementSpeed * Time.deltaTime, 0);

        transform.Translate(moveAmount);
    }
}
