using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidescrollScript : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;

    //[Header("Spawning")]
    //private TopAndBottomSpawn spawner;
    //private bool doOnce = true;

    // Start is called before the first frame update
    void Start()
    {
        //spawner = GameObject.FindGameObjectWithTag("TopAndBottomSpawner").GetComponent<TopAndBottomSpawn>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        /*
        if (transform.position.x < 8 && doOnce)
        {
            spawner.xPosPrev = transform.position.x;
            spawner.farEnough = true;
            doOnce = false;
        }
        */
        if (transform.position.x < -12)
        {
            Destroy(gameObject);
        }
    }
}
