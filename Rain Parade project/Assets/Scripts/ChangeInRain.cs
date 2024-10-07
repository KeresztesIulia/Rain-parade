using UnityEngine;

public class ChangeInRain : MonoBehaviour
{
    [SerializeField] bool changeIfRain;
    [SerializeField] bool changeIfNoRain;
    [SerializeField] Sprite rainSprite;
    [SerializeField] Sprite noRainSprite;

    bool changed = false;

    SpriteRenderer thisSprite;
    CloudController cloud;

    static float distanceMargin = 0.5f;

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
        thisSprite = GetComponent<SpriteRenderer>();
        cloud = FindObjectOfType<CloudController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (changeIfRain && collision.transform.tag == "Rain")
        {
            ChangeSprite(rainSprite);
        }
    }

    private void Update()
    {
        if (changeIfNoRain && FarEnough) ChangeSprite(noRainSprite);
    }

    void ChangeSprite(Sprite changedSprite)
    {
        if (changed) return;
        thisSprite.sprite = changedSprite;
        changed = true;
    }

}
