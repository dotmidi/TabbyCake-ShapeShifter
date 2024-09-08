using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] public Canvas GameOverCanvas;

    // Gravity settings
    private bool isGravityUp = true;
    [SerializeField] private float gravityMultiplier = 1.0f;

    // Raycast settings
    [SerializeField] private LayerMask levelGeometryLayer;
    [SerializeField] private float raycastLength = 1.0f;
    private Vector2 rayDirection = Vector2.right;
    public bool alive = true;

    void Start()
    {
        alive = true;
    }

    void FixedUpdate()
    {
        // Physics check for raycast collision
        if (Physics2D.Raycast(transform.position, rayDirection, raycastLength, levelGeometryLayer))
        {
            alive = false;
        }
        if (!alive)
        {
            // show the game over canvas, canvas is called GameOverCanvas

            GameOverCanvas.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        // Handle gravity flip on spacebar press
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FlipGravity();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * raycastLength);
    }

    void FlipGravity()
    {
        // Toggle gravity direction and scale using a boolean flag
        rb.gravityScale = isGravityUp ? gravityMultiplier : -gravityMultiplier;
        isGravityUp = !isGravityUp;
    }
}
