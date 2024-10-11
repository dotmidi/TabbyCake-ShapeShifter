using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public List<GameObject> pickups;
    public GameObject player;
    private float intervalBetweenObstacles = 30f;
    public float obstacleSpeedMultiplier;
    private float previousHighScore;
    private PlayerScript playerScript;
    private float nextIntervalReductionScore = 5000f;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = player.GetComponent<PlayerScript>();
        StartCoroutine(SpawnPickups());
    }

    // Update is called once per frame
    void Update()
    {
        float highScore = Mathf.Round(playerScript.HighScore);
        if (highScore != previousHighScore)
        {
            previousHighScore = highScore;

            if (highScore >= nextIntervalReductionScore && intervalBetweenObstacles > 20f)
            {
                // Debug.Log("High score reached! Reducing interval between obstacles.");
                intervalBetweenObstacles = 20f;
            }

            // Debug.Log("Current interval between obstacles: " + intervalBetweenObstacles);
        }
    }

    IEnumerator SpawnPickups()
    {
        while (true)
        {
            // Debug.Log("Waiting for " + intervalBetweenObstacles + " seconds to spawn the next pickup.");
            yield return new WaitForSeconds(intervalBetweenObstacles);
            SpawnPickup();
        }
    }

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

        // Debug.Log("Spawned pickup at position: " + shapeChanger.transform.position);
    }
}
