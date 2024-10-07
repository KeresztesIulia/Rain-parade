using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    static float score = 0;

    public static float Score
    {
        get
        {
            return score;
        }
    }

    private void OnEnable()
    {
        score = 0;
    }

    public static void AddScore(float score)
    {
        ScoreCounter.score += score;
    }

    private void Update()
    {
        text.text = score.ToString();
    }
}
