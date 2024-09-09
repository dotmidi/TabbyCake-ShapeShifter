using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinespawnScript : MonoBehaviour
{
    [SerializeField] private GameObject lines;
    private GameObject lastSpawn;
    private Vector3 spawnOffset = new Vector3(4, 0, 0);  // Pre-calculated constant offset for spawning

    // Start is called before the first frame update
    void Start()
    {
        lastSpawn = (GameObject)Instantiate(lines, transform.position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        if (lastSpawn.transform.position.x < 8)
        {
            lastSpawn = (GameObject)Instantiate(lines, lastSpawn.transform.position + spawnOffset, transform.rotation);
        }
    }
}
