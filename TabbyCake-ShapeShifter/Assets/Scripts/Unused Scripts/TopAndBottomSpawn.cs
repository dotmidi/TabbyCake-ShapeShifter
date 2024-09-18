using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopAndBottomSpawn : MonoBehaviour
{
    [SerializeField] private GameObject topAndBottom;
    private GameObject lastSpawn;
    private Vector3 spawnOffset = new Vector3(4, 0, 0);  // Pre-calculated constant offset for spawning

    // Start is called before the first frame update
    void Start()
    {
        lastSpawn = Instantiate(topAndBottom, transform.position, transform.rotation);  // Removed redundant casting
    }

    void FixedUpdate()
    {
        // Check if the last spawn is far enough to spawn a new object
        if (lastSpawn.transform.position.x < 8)
        {
            lastSpawn = Instantiate(topAndBottom, lastSpawn.transform.position + spawnOffset, transform.rotation);
        }
    }
}
