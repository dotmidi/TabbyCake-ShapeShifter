using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidescrollScript : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject player;
    private PlayerScript playerScript;

    //[Header("Spawning")]
    //private TopAndBottomSpawn spawner;
    //private bool doOnce = true;

    // Start is called before the first frame update
    void Start()
    {
        //spawner = GameObject.FindGameObjectWithTag("TopAndBottomSpawner").GetComponent<TopAndBottomSpawn>();
        playerScript = player.GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript.alive)
        {
            rb.velocity = Vector2.left * moveSpeed;
        }
        else if (playerScript.alive == false)
        {
            rb.velocity = Vector2.zero;
        }


        /*
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        
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
