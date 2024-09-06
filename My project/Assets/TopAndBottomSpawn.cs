using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopAndBottomSpawn : MonoBehaviour
{
    [SerializeField] private GameObject topAndBottom;
    private GameObject lastSpawn;
    Vector3 spawnOffset = new Vector3(4, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        lastSpawn = (GameObject)Instantiate(topAndBottom, transform.position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        if(lastSpawn.transform.position.x < 8)
        {
            lastSpawn = (GameObject)Instantiate(topAndBottom, lastSpawn.transform.position + spawnOffset, transform.rotation);
        }
    }
}
