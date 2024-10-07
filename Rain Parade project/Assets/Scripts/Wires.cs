using UnityEngine;

public class Wires : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player" || collision.transform.tag == "Rain")
        {
            if (GameHandler.GameOver != null) GameHandler.GameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player" || collision.transform.tag == "Rain")
        {
            if (GameHandler.GameOver != null) GameHandler.GameOver();
        }
    }
}
