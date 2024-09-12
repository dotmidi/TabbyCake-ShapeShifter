using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkSpawnScript : MonoBehaviour
{
    public GameObject[] prefabChunks;
    private float timer;
    private float timerTrigger = 2f;

    //[SerializeField] private PlayerScript playerScript;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(
            prefabChunks[Random.Range(0, prefabChunks.Length)],
            transform.position,
            transform.rotation
        );
        Debug.Log("chunk spawned");
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > timerTrigger)
        {
            Instantiate(
                prefabChunks[Random.Range(0, prefabChunks.Length)],
                transform.position,
                transform.rotation
            );
            Debug.Log("chunk spawned");
            timer = 0;
        }
    }
}
