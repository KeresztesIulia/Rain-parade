using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField] float movementSpeed = 2f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (GameHandler.GameOver != null) GameHandler.GameOver();
        }
    }

    private void Update()
    {
        transform.Translate(new Vector2(-movementSpeed * Time.deltaTime, 0));
    }
}
