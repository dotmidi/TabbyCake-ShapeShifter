using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnBoundary : MonoBehaviour
{
    public float boundaryX = -15.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

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
        Debug.Log("Collided with " + other.gameObject.name);

        // trigger PlayerScript's BlockHit method
        if (other.gameObject.name == "Player")
        {
            Debug.Log("Player hit the block, trigger side");
            other.gameObject.GetComponent<PlayerScript>().BlockHit();
        }
    }
}
