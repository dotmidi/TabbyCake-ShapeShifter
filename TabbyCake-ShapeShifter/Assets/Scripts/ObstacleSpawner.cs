using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public List<GameObject> obstacles;
    public List<GameObject> pickups;
    public GameObject player;
    public float intervalBetweenObstacles;
    public float obstacleSpeedMultiplier;
    private float previousHighScore; // Track the previous high score to avoid unnecessary updates
    private PlayerScript playerScript; // Cache PlayerScript
    private float nextSpeedIncreaseScore = 1000f;
    private float nextIntervalReductionScore = 2000f;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = player.GetComponent<PlayerScript>(); // Cache the PlayerScript
        // StartCoroutine(SpawnObstacles());
        StartCoroutine(SpawnShapeChangers());
    }

    // Update is called once per frame
    void Update()
    {
        float highScore = Mathf.Round(playerScript.HighScore);
        if (highScore != previousHighScore)
        {
            previousHighScore = highScore;

            if (highScore >= nextSpeedIncreaseScore)
            {
                obstacleSpeedMultiplier *= 1.05f;
                nextSpeedIncreaseScore += 1000f; // Update the next score threshold
            }

            if (highScore >= nextIntervalReductionScore && intervalBetweenObstacles >= 0.4f)
            {
                intervalBetweenObstacles -= 0.1f;
                nextIntervalReductionScore += 2000f; // Update the next interval reduction score
            }
        }
    }

    public void SlowObstacles()
    {
        obstacleSpeedMultiplier *= 0.9f;
    }

    // Coroutine for spawning shape changers
    IEnumerator SpawnShapeChangers()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervalBetweenObstacles * 10);
            SpawnShapeChanger();
        }
    }

    // Coroutine for spawning obstacles
    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervalBetweenObstacles);
            SpawnObstacle();
        }
    }

    // Method to spawn a shape changer
    private void SpawnShapeChanger()
    {
        int randomIndex = Random.Range(0, pickups.Count);
        float randomY = Random.Range(-2f, 2f);
        GameObject shapeChanger = Instantiate(
            pickups[randomIndex],
            new Vector3(10, randomY, 0),
            Quaternion.identity
        );
        Rigidbody2D shapeChangerRigidbody = shapeChanger.GetComponent<Rigidbody2D>();
        shapeChangerRigidbody.velocity = Vector2.left * obstacleSpeedMultiplier;
        shapeChangerRigidbody.angularVelocity = 200f;
    }

    // Method to spawn an obstacle
    private void SpawnObstacle()
    {
        int randomIndex = Random.Range(0, obstacles.Count);
        float randomY = Random.value > 0.5f ? 3f : -3f; // Randomly choose between top or bottom

        // Instantiate the obstacle and adjust its rotation
        Quaternion rotation = randomY > 0 ? Quaternion.Euler(0, 0, 180) : Quaternion.identity;
        GameObject obstacle = Instantiate(
            obstacles[randomIndex],
            new Vector3(10, randomY, 0),
            rotation
        );
        Rigidbody2D obstacleRigidbody = obstacle.GetComponent<Rigidbody2D>();
        obstacleRigidbody.velocity = Vector2.left * obstacleSpeedMultiplier;
    }
}
