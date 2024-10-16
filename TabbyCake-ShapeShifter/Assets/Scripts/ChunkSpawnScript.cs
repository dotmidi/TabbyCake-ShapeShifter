using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkSpawnScript : MonoBehaviour
{
    public GameObject[] prefabChunks;
    public GameObject player;
    private bool isSpawningChunks = false;
    public float obstacleSpeedMultiplier;
    private PlayerScript playerScript;
    private float nextMultiplierCheck = 1000f;
    private float baseSpawnDelay = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        obstacleSpeedMultiplier = 10f;
        playerScript = player.GetComponent<PlayerScript>();
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
            nextMultiplierCheck += 1000f;
        }
    }

    // Determine if we should start spawning chunks
    private bool ShouldStartSpawningChunks()
    {
        return true;
    }

    // Calculate the delay between chunk spawns based on the highscore
    private float CalculateSpawnDelay()
    {
        float highScore = playerScript.HighScore;
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
        // string[] chunkTags = { "SquareChunk" };
        // string[] chunkTags = { "DiamondChunk" };
        // string[] chunkTags = { "CircleChunk" };
        // string[] chunkTags = { "TriangleChunk", "CircleChunk" };
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
            yield return new WaitForSeconds(spawnDelay);
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
            new Vector3(14, 3, 1),
            Quaternion.identity
        );
        Rigidbody2D chunkRigidbody = spawnedChunk.GetComponent<Rigidbody2D>();
        chunkRigidbody.velocity = obstacleSpeedMultiplier * Vector2.left;
    }
}
