using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondMoveScriptOLD : MonoBehaviour
{
    public Rigidbody2D diamondRigidbody;
    public bool moveDown = true; // Controls initial movement direction
    private float timer = 0f;
    private float moveInterval; // Time interval for movement

    // Start is called before the first frame update
    void Start()
    {
        diamondRigidbody = GetComponent<Rigidbody2D>();
        SetRandomInterval(); // Initialize the move interval
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime; // Accumulate time

        if (timer >= moveInterval)
        {
            // Reset the timer
            timer -= moveInterval;

            if (moveDown)
            {
                diamondRigidbody.MovePosition(diamondRigidbody.position + new Vector2(0, -1));
            }
            else
            {
                diamondRigidbody.MovePosition(diamondRigidbody.position + new Vector2(0, 1));
            }

            // Toggle the movement direction
            moveDown = !moveDown;

            // Set a new random interval
            SetRandomInterval();
        }
    }

    private void SetRandomInterval()
    {
        moveInterval = Random.Range(0.1f, 1f); // Random time between 0.1 and 1 seconds
    }
}
