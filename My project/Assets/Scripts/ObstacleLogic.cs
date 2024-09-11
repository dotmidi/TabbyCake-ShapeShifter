using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnBoundary : MonoBehaviour
{
    public float boundaryX = -15.0f;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < boundaryX)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log(other.gameObject.name);
        // Debug.Log(gameObject.name);
        // Debug.Log(gameObject.tag);

        // trigger PlayerScript's BlockHit method
        if (other.gameObject.name == "Player" && gameObject.tag == "RedObstacle")
        {
            // Debug.Log("Player hit the block, trigger side");
            other.gameObject.GetComponent<PlayerScript>().BlockHit();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // Debug.Log(other.gameObject.name);
        // Debug.Log(gameObject.name);
        // Debug.Log(gameObject.tag);

        // trigger PlayerScript's BlockHit method
        if (other.gameObject.name == "Player" && gameObject.tag == "YellowObstacle")
        {
            // Debug.Log("Player hit the block, trigger top");
            other.gameObject.GetComponent<PlayerScript>().SlowObstacles();
        }
    }
}
