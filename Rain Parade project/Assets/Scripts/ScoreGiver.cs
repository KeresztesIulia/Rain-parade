using TMPro;
using UnityEngine;

public class ScoreGiver : MonoBehaviour
{
    [SerializeField] bool givePointsIfRain;
    [SerializeField] bool givePointsIfNoRain;
    [SerializeField] float pointsIfRain;
    [SerializeField] float pointsIfNoRain;

    [SerializeField] TextMeshPro scoreText;

    static float distanceMargin = 0.5f;

    CloudController cloud;
    SpriteRenderer thisSprite;

    bool added = false;
   
    bool FarEnough
    {
        get
        {
            float xCloud = cloud.transform.position.x;
            float xThis = thisSprite.transform.position.x;
            float halfsizeCloud = cloud.GetComponent<SpriteRenderer>().bounds.size.x / 2;
            float halfsizeThis = thisSprite.bounds.size.x / 2;
            return (xCloud - halfsizeCloud - xThis - halfsizeThis > distanceMargin);
        }
    }

    private void Start()
    {
        cloud = FindObjectOfType<CloudController>();
        thisSprite = GetComponent<SpriteRenderer>();
    }

    public void Init(bool rain, float points, bool noRain = false, float pointsNR = 0)
    {
        givePointsIfRain = rain;
        givePointsIfNoRain = noRain;
        pointsIfRain = points;
        pointsIfNoRain = pointsNR;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (givePointsIfRain && collision.transform.tag == "Rain")
        {
            AddScore(pointsIfRain);
        }
    }

    private void Update()
    {
        if (givePointsIfNoRain && FarEnough) AddScore(pointsIfNoRain);
    }

    void AddScore(float points)
    {
        if (added) return;
        ScoreCounter.AddScore(points);
        string scoreString = points > 0 ? ("+" + points.ToString()) : points.ToString();
        scoreText.text = scoreString;
        scoreText.gameObject.SetActive(true);
        added = true;
    }
}
