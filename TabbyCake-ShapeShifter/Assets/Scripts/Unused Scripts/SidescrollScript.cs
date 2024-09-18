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

    // Start is called before the first frame update
    void Start()
    {
        playerScript = player.GetComponent<PlayerScript>(); // Cache the playerScript reference
    }

    void FixedUpdate()
    {
        // Move left if player is alive, stop if not
        rb.velocity = playerScript.alive ? Vector2.left * moveSpeed : Vector2.zero;

        // Destroy the game object when it goes off-screen
        if (transform.position.x < -12)
        {
            Destroy(gameObject);
        }
    }

    /*
    // Spawning-related logic, can be uncommented when needed
    private TopAndBottomSpawn spawner;
    private bool doOnce = true;

    void Update()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        if (transform.position.x < 8 && doOnce)
        {
            spawner.xPosPrev = transform.position.x;
            spawner.farEnough = true;
            doOnce = false;
        }
    }
    */
}
