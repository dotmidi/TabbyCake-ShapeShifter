using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidescrollScriptTEST : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveSpeed = 10f;
    private PlayerScript playerScript;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerScript.alive)
        {
            rb.velocity = Vector3.left * moveSpeed;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
        if(transform.position.x < -25)
        {
            Destroy(gameObject);
        }
    }
}
