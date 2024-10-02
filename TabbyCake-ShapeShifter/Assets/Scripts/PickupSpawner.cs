using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
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
        StartCoroutine(SpawnPickups());
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

    // Coroutine for spawning shape changers
    IEnumerator SpawnPickups()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervalBetweenObstacles * 20);
            SpawnPickup();
        }
    }

    // Method to spawn a shape changer
    private void SpawnPickup()
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
}
