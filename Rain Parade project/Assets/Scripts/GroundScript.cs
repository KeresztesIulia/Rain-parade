using UnityEngine;
using UnityEngine.U2D;

public class Ground : MonoBehaviour
{
    Color rainedColor;
    SpriteShapeRenderer groundRenderer;

    public enum GroundType { EARTH, WATER };
    GroundType groundType;

    public GroundType Type
    {
        get
        {
            return groundType;
        }
    }

    public static void Init(Ground ground, Color rainedColor, GroundType type)
    {
        ground.rainedColor = rainedColor;
        ground.groundType = type;
    }

    private void Start()
    {
        groundRenderer = GetComponent<SpriteShapeRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Rain")
        {
            groundRenderer.color = rainedColor;
        }
        else if (collision.tag == "Player")
        {
            if (GameHandler.GameOver != null) GameHandler.GameOver();
        }
    }

}
