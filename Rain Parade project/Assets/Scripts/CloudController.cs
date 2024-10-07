using UnityEngine;

public class CloudController : MonoBehaviour
{
    [SerializeField] GameObject rain;
    [SerializeField] float movementSpeed = 5;
    [SerializeField] float descendSpeed = 1;
    [SerializeField] float ascendSpeed = 2;
    [SerializeField] float chickenifyAtThisScore = 1500;

    [SerializeField] Sprite CHICKEN;

    SpriteRenderer spriteRenderer;

    float verticalSpeed;
    Rigidbody2D cloudRB;

    private void Start()
    {
        verticalSpeed = -descendSpeed;
        cloudRB = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            verticalSpeed = ascendSpeed;
            rain.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            verticalSpeed = -descendSpeed;
            rain.SetActive(false);
        }

        if (ScoreCounter.Score > chickenifyAtThisScore)
        {
            spriteRenderer.sprite = CHICKEN;
        }

        Vector2 moveAmount = new Vector2(movementSpeed * Time.deltaTime, verticalSpeed * Time.deltaTime);

        transform.Translate(moveAmount);
    }
}
