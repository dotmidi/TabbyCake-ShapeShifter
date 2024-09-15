using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public List<GameObject> obstacles;
    public List<GameObject> shapeChangers;
    public GameObject player;
    public float intervalBetweenObstacles;
    public float obstacleSpeedMultiplier;
    public float HighScore;

    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(SpawnObstacles());
        StartCoroutine(SpawnShapeChangers());
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("HighScore: " + HighScore);
        // Debug.Log("obstacleSpeedMultiplier: " + obstacleSpeedMultiplier);
        // Debug.Log("intervalBetweenObstacles: " + intervalBetweenObstacles);
        // // take HighScore from PlayerScript
        HighScore = player.GetComponent<PlayerScript>().HighScore;
        HighScore = Mathf.Round(HighScore);
        if (HighScore % 1000 == 0)
        {
            obstacleSpeedMultiplier *= 1.01f;
        }
        if (HighScore % 2000 == 0 && intervalBetweenObstacles >= 0.4f)
        {
            intervalBetweenObstacles -= 0.1f;
        }
    }

    public void SlowObstacles()
    {
        obstacleSpeedMultiplier *= 0.9f;
    }

    IEnumerator SpawnShapeChangers()
    {
        while (true)
        {
            // Debug.Log("Spawning shape changer");
            yield return new WaitForSeconds(intervalBetweenObstacles * 10);
            int randomIndex = Random.Range(0, obstacles.Count);
            float randomY = Random.Range(-2f, 2f);
            GameObject shapeChanger = Instantiate(
                shapeChangers[randomIndex],
                new Vector3(10, randomY, 0),
                Quaternion.identity
            );
            Rigidbody2D shapeChangerRigidbody = shapeChanger.GetComponent<Rigidbody2D>();
            shapeChangerRigidbody.velocity = Vector2.left * obstacleSpeedMultiplier;
            shapeChangerRigidbody.angularVelocity = 200f;
        }
    }

    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervalBetweenObstacles);
            int randomIndex = Random.Range(0, obstacles.Count);
            float randomY = Random.value > 0.5f ? 3f : -3f; // Randomly choose either -4 or 4
            if (randomY > 0)
            {
                GameObject obstacle = Instantiate(
                    obstacles[randomIndex],
                    new Vector3(10, randomY, 0),
                    Quaternion.Euler(0, 0, 180)
                );
                Rigidbody2D obstacleRigidbody = obstacle.GetComponent<Rigidbody2D>();
                obstacleRigidbody.velocity = Vector2.left * obstacleSpeedMultiplier;
            }
            else
            {
                GameObject obstacle = Instantiate(
                    obstacles[randomIndex],
                    new Vector3(10, randomY, 0),
                    Quaternion.identity
                );
                Rigidbody2D obstacleRigidbody = obstacle.GetComponent<Rigidbody2D>();
                obstacleRigidbody.velocity = Vector2.left * obstacleSpeedMultiplier;
            }
        }
    }
}
