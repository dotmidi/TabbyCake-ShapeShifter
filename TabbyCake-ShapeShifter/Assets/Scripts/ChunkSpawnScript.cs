using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkSpawnScript : MonoBehaviour
{
    public GameObject[] prefabChunks;
    public GameObject player;
    private float spawnRate = 1f;
    private bool isSpawningChunks = false; // Flag to check if chunks are being spawned
    public float obstacleSpeedMultiplier = 1f;

    // Start is called before the first frame update
    void Start()
    {
        obstacleSpeedMultiplier = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        spawnRate -= Time.deltaTime;
        if (spawnRate <= 0 && !isSpawningChunks) // Check if chunks are not already being spawned
        {
            // spawnRate = 1f;
            Debug.Log("Spawning new chunk");
            StartCoroutine(SpawnChunksWithDelay());
        }

        // take HighScore from PlayerScript
        float HighScore = player.GetComponent<PlayerScript>().HighScore;
        HighScore = Mathf.Round(HighScore);
        if (HighScore % 1000 == 0)
        {
            obstacleSpeedMultiplier *= 1.1f;
        }
        Debug.Log(obstacleSpeedMultiplier);
    }

    public List<GameObject> CreateChunkList()
    {
        // Create list of chunks, should only have tag "TriangleChunk" or "SquareChunk", list should be 2-4 chunks long
        List<GameObject> chunkList = new List<GameObject>();
        int chunkCount = Random.Range(2, 5);
        string chunkTag = Random.Range(0, 2) == 0 ? "TriangleChunk" : "SquareChunk";
        for (int i = 0; i < chunkCount; i++)
        {
            GameObject chunk = prefabChunks[Random.Range(0, prefabChunks.Length)];
            if (chunk.tag == chunkTag)
            {
                chunkList.Add(chunk);
            }
        }
        return chunkList;
    }

    private IEnumerator SpawnChunksWithDelay()
    {
        isSpawningChunks = true; // Set flag to true while spawning chunks

        List<GameObject> chunkList = CreateChunkList();
        foreach (GameObject chunk in chunkList)
        {
            yield return StartCoroutine(SpawnChunkWithDelay(chunk));
        }

        isSpawningChunks = false; // Reset flag when all chunks have been spawned
    }

    private IEnumerator SpawnChunkWithDelay(GameObject chunk)
    {
        yield return new WaitForSeconds(1.5f); // Delay before spawning each chunk
        GameObject spawnedChunk = Instantiate(chunk, new Vector3(10, 0, 0), Quaternion.identity);
        Rigidbody2D chunkRigidbody = spawnedChunk.GetComponent<Rigidbody2D>();
        chunkRigidbody.velocity = obstacleSpeedMultiplier * Vector2.left;
    }
}
