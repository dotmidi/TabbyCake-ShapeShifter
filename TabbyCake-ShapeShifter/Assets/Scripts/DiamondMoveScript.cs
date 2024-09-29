using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondMoveScript : MonoBehaviour
{
    public Rigidbody2D diamondRigidbody;
    private PlayerScript playerScript;
    public bool moveDown = true; // Controls initial movement direction
    private float timer = 0f;
    private float moveInterval; // Time interval for movement
    public float lowerLimit = -4f; // Lower boundary for y position
    public float upperLimit = 4f; // Upper boundary for y position

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

        float currentY = diamondRigidbody.position.y;

        if (timer >= moveInterval)
        {
            // Reset the timer
            timer -= moveInterval;

            // Determine direction based on current position and limits
            if (moveDown && currentY > lowerLimit)
            {
                diamondRigidbody.MovePosition(diamondRigidbody.position + new Vector2(0, -1));
            }
            else if (!moveDown && currentY < upperLimit)
            {
                diamondRigidbody.MovePosition(diamondRigidbody.position + new Vector2(0, 1));
            }

            // Check if the diamond has reached the boundaries, and reverse direction
            if (currentY <= lowerLimit)
            {
                moveDown = false; // Start moving up
            }
            else if (currentY >= upperLimit)
            {
                moveDown = true; // Start moving down
            }

            // Set a new random interval
            SetRandomInterval();
        }
    }

    private void SetRandomInterval()
    {
        moveInterval = Random.Range(0.1f, 1f); // Random time between 0.1 and 1 seconds
    }
}
