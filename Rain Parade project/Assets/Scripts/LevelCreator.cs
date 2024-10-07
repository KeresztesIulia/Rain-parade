using UnityEngine;
using UnityEngine.U2D;
using System.Collections.Generic;

public class LevelCreator : MonoBehaviour
{
    [SerializeField] SpriteShape spriteShape;

    [Header("Categories")]
    [SerializeField] Transform groundHolder;
    [SerializeField] Transform groundObstacleHolder;
    [SerializeField] Transform windHolder;

    [Header("Prefabs")]
    [SerializeField] GameObject[] groundObstacles;
    [SerializeField] GameObject[] waterObstacles;
    [SerializeField] GameObject wind;

    [Header("Ground info")]
    [SerializeField] float width = 0.1f;
    [SerializeField] float minHeight = 2f;
    [SerializeField] float maxHeight = 5f;
    [SerializeField] int startLengthToDraw = 20;
    [SerializeField] float randomRange = 100;
    [SerializeField] int perlinDetail = 5;

    [Header("Obstacle info")]
    [SerializeField] float minDistanceBetweenObstacles = 3f;
    [SerializeField] float maxDistanceBetweenObstacles = 20f;
    [SerializeField] float minDistanceBetweenWinds = 5f;
    [SerializeField] float maxDistanceBetweenWinds = 20f;
    [SerializeField] float groundObstacleProbabilityThreshold = 0.2f;
    [SerializeField] float windProbabilityThreshold = 0.4f;
    [SerializeField] float maxWindHeight = 15f;

    [Header("Colors")]
    [SerializeField] Color groundColor;
    [SerializeField] Color groundRainedColor;
    [SerializeField] Color waterColor;
    [SerializeField] Color waterRainedColor;

    [Header("ReGeneration")]
    [SerializeField] float distanceForNewGeneration = 50;
    [SerializeField] Transform player;
    [SerializeField] float newGenerationDistance = 200;
    [SerializeField] float deletionDistance = 20;

    float lastGroundObstaclePlace = 0;
    float lastWindPlace = 0;
    float perGameRandomizer;
    float heightInterval;
    float waterHeight;

    float lastDeletionPlace = 0f;
    float lastGenerationPlace = 0f;


    static Dictionary<float, Ground> groundPieces = new();
    static Dictionary<float, GameObject> obstacleDict = new();
    static Dictionary<float, GameObject> windDict = new();

    private void OnEnable()
    {
        groundPieces.Clear();
        perGameRandomizer = Random.value * randomRange - randomRange/2;
        heightInterval = maxHeight - minHeight;
        waterHeight = heightInterval / 3 + minHeight;

        GenerateBetweenPoints(width / 2, startLengthToDraw);
        lastDeletionPlace = width / 2;
    }

    void GenerateBetweenPoints(float pointA, float pointB)
    {
        for (float i = pointA; i < pointB; i += width)
        {
            // Set-up ground tile
            Ground.GroundType type;
            GameObject ground = new GameObject("Grood");
            ground.transform.parent = groundHolder;
            ground.transform.localPosition = new Vector2(0, 0);
            SpriteShapeController groundController = ground.AddComponent<SpriteShapeController>();
            SpriteShapeRenderer groundRenderer = ground.GetComponent<SpriteShapeRenderer>();
            ground.AddComponent<PolygonCollider2D>().isTrigger = true;
            Ground groundScript = ground.AddComponent<Ground>();
            groundRenderer.sortingOrder = 100;

            ScoreGiver groundScore = ground.AddComponent<ScoreGiver>();
            groundScore.Init(true, 1);

            groundController.spriteShape = spriteShape;

            float y1 = Mathf.PerlinNoise1D(i / perlinDetail + perGameRandomizer) * heightInterval + minHeight;
            float y2 = Mathf.PerlinNoise1D((i + width) / perlinDetail + perGameRandomizer) * heightInterval + minHeight;

            if (y1 < waterHeight && y2 < waterHeight)
            {
                groundRenderer.color = waterColor;
                Ground.Init(groundScript, waterRainedColor, Ground.GroundType.WATER);
                type = Ground.GroundType.WATER;
            }
            else
            {
                groundRenderer.color = groundColor;
                Ground.Init(groundScript, groundRainedColor, Ground.GroundType.EARTH);
                type = Ground.GroundType.EARTH;
            }

            Spline spline = groundController.spline;
            spline.Clear();
            spline.InsertPointAt(0, new Vector2(i, 0));
            spline.InsertPointAt(1, new Vector2(i, y1));
            spline.InsertPointAt(2, new Vector2(i + width, y2));
            spline.InsertPointAt(3, new Vector2(i + width, 0));

            for (int j = 0; j < spline.GetPointCount(); j++)
            {
                spline.SetTangentMode(j, ShapeTangentMode.Continuous);
            }

            groundController.splineDetail = 2;

            groundPieces.Add(i, groundScript);

            // Create ground obstacles
            float currentDistance = i - lastGroundObstaclePlace;
            float obstacleInterval = maxDistanceBetweenObstacles - minDistanceBetweenObstacles;
            float baseProbability = Mathf.Clamp01((currentDistance - minDistanceBetweenObstacles) / obstacleInterval);
            float finalProbability = Random.value * baseProbability;
            if (finalProbability > groundObstacleProbabilityThreshold)
            {
                float yMid = ground.transform.position.y + (y1 + y2) / 2;
                float x = ground.transform.position.x + i;
                if (type == Ground.GroundType.EARTH && groundObstacles.Length > 0)
                {
                    lastGroundObstaclePlace = i;
                    GameObject obstacle = Instantiate(groundObstacles[Random.Range(0, groundObstacles.Length * 1000) % groundObstacles.Length]);
                    obstacle.transform.position = new Vector2(x, yMid);
                    obstacle.transform.parent = groundObstacleHolder;
                }
                if (type == Ground.GroundType.WATER && waterObstacles.Length > 0)
                {
                    lastGroundObstaclePlace = i;
                    GameObject obstacle = Instantiate(waterObstacles[Random.Range(0, waterObstacles.Length * 1000) % waterObstacles.Length]);
                    obstacle.transform.position = new Vector2(x, yMid);
                    obstacle.transform.parent = groundObstacleHolder;
                }
            }

            // Create winds
            float currentWindDistance = i - lastWindPlace;
            float windInterval = maxDistanceBetweenWinds - minDistanceBetweenWinds;
            float baseWindProbability = Mathf.Clamp01((currentWindDistance - minDistanceBetweenWinds) / windInterval);
            float finalWindProbability = Random.value * baseWindProbability;
            if (finalWindProbability > windProbabilityThreshold)
            {
                float y = Random.Range(Mathf.Max(y1, y2) + ground.transform.position.y + 0.3f, maxWindHeight);
                float x = i;
                lastWindPlace = i;

                GameObject windClone = Instantiate(wind);
                windClone.transform.position = new Vector2(x, y);
                windClone.transform.parent = windHolder;

            }

            lastGenerationPlace = pointB;

        }
    }

    void Delete(float pointA, float pointB)
    {
        for (float i = pointA; i <= pointB; i+= width)
        {
            if (groundPieces.ContainsKey(i))
            {
                Destroy(groundPieces[i].gameObject);
                groundPieces.Remove(i);
            }
            if (obstacleDict.ContainsKey(i))
            {
                Destroy(obstacleDict[i]);
                obstacleDict.Remove(i);
            }
            if (windDict.ContainsKey(i))
            {
                Destroy(windDict[i]);
                windDict.Remove(i);
            }
        }
        lastDeletionPlace = pointB + width;
    }

    private void Update()
    {
        if (lastGenerationPlace - player.position.x < distanceForNewGeneration)
        {
            Delete(lastDeletionPlace, Mathf.Floor(player.position.x) - deletionDistance);
            GenerateBetweenPoints(lastGenerationPlace, lastGenerationPlace + newGenerationDistance);
        }
    }
}
