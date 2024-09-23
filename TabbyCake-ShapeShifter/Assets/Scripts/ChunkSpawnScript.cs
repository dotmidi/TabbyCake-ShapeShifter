using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkSpawnScript : MonoBehaviour
{
    public GameObject[] prefabChunks;
    public GameObject player;
    private bool isSpawningChunks = false; // Flag to check if chunks are being spawned
    public float obstacleSpeedMultiplier;
    private PlayerScript playerScript;
    private float nextMultiplierCheck = 1000f; // Next score threshold to check for speed multiplier
    private float baseSpawnDelay = 1.5f; // Base delay for chunk spawning

    // Start is called before the first frame update
    void Start()
    {
        obstacleSpeedMultiplier = 10f;
        playerScript = player.GetComponent<PlayerScript>(); // Cache the PlayerScript reference
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSpawningChunks)
        {
            if (ShouldStartSpawningChunks())
            {
                StartCoroutine(SpawnChunksWithDelay());
            }
        }

        // Check HighScore at specific thresholds to increase speed multiplier
        float highScore = Mathf.Round(playerScript.HighScore);
        if (highScore >= nextMultiplierCheck)
        {
            obstacleSpeedMultiplier *= 1.1f;
            nextMultiplierCheck += 1000f; // Update threshold
        }
    }

    // Determine if we should start spawning chunks
    private bool ShouldStartSpawningChunks()
    {
        return true; // Replace this with your logic to determine if chunks should be spawned
    }

    // Calculate the delay between chunk spawns based on the highscore
    private float CalculateSpawnDelay()
    {
        float highScore = playerScript.HighScore;
        // Example: Shorten the delay as the highscore increases, up to a minimum threshold
        float minDelay = 0.5f;
        float maxDelay = 2.0f;
        float delay = Mathf.Clamp(baseSpawnDelay - (highScore / 10000f), minDelay, maxDelay);
        return delay;
    }

    // Filtered chunk list generation
    private List<GameObject> CreateChunkList()
    {
        List<GameObject> chunkList = new List<GameObject>();
        int chunkCount = Random.Range(2, 5);
        string[] chunkTags = { "TriangleChunk", "SquareChunk", "DiamondChunk", "CircleChunk" };
        // string[] chunkTags = { "DiamondChunk" };
        // string[] chunkTags = { "CircleChunk" };
        string chunkTag = chunkTags[Random.Range(0, chunkTags.Length)];

        if (chunkTag == "CircleChunk")
        {
            return chunkList;
        }

        // Filter chunks by tag and select random chunks from the filtered list
        GameObject[] filteredChunks = System.Array.FindAll(
            prefabChunks,
            chunk => chunk.tag == chunkTag
        );
        for (int i = 0; i < chunkCount; i++)
        {
            GameObject chunk = filteredChunks[Random.Range(0, filteredChunks.Length)];
            chunkList.Add(chunk);
        }

        return chunkList;
    }

    // Combined coroutine for chunk spawning
    private IEnumerator SpawnChunksWithDelay()
    {
        isSpawningChunks = true;

        List<GameObject> chunkList = CreateChunkList();
        float spawnDelay = CalculateSpawnDelay();

        if (chunkList.Count == 0)
        {
            SpawnCircleChunk();
            yield return new WaitForSeconds(spawnDelay);
        }

        foreach (GameObject chunk in chunkList)
        {
            SpawnChunk(chunk);
            yield return new WaitForSeconds(spawnDelay); // Delay for each chunk spawn based on the highscore
        }

        isSpawningChunks = false;
    }

    // Direct chunk spawning without an additional coroutine
    private void SpawnChunk(GameObject chunk)
    {
        GameObject spawnedChunk = Instantiate(chunk, new Vector3(10, 0, 1), Quaternion.identity);
        Rigidbody2D chunkRigidbody = spawnedChunk.GetComponent<Rigidbody2D>();
        chunkRigidbody.velocity = obstacleSpeedMultiplier * Vector2.left;
    }



    // method to spawn circle elements, these should spawn between y = -2 and y = 2
    private void SpawnCircleChunk()
    {
        GameObject spawnedChunk = Instantiate(
            prefabChunks[0],
            new Vector3(10, Random.Range(-4, 4), 1),
            Quaternion.identity
        );
        Rigidbody2D chunkRigidbody = spawnedChunk.GetComponent<Rigidbody2D>();
        chunkRigidbody.velocity = obstacleSpeedMultiplier * Vector2.left;
    }
}
