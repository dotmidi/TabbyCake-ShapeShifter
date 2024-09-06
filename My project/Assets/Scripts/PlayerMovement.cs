using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool onGround = false;
    [SerializeField] private float groundLength = 0.5f;
    [SerializeField] private float jumpSpeed = 15.0f;
    [SerializeField] private Vector3 colliderOffset;

    private Vector2 colliderOffsetUp;
    private Vector2 colliderOffsetDown;

    // Start is called before the first frame update
    void Start()
    {
        // Cache collider offsets to avoid recalculating
        colliderOffsetUp = (Vector2)(transform.position + colliderOffset);
        colliderOffsetDown = (Vector2)(transform.position - colliderOffset);
    }

    void FixedUpdate()
    {
        // Use BoxCast to check if the player is on the ground
        onGround = Physics2D.BoxCast(transform.position, new Vector2(1, groundLength), 0, Vector2.down, groundLength, groundLayer);

        // Player movement input
        float horizontalInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        // Jump if on ground and jump button is pressed
        if (Input.GetButtonDown("Jump") && onGround)
        {
            Jump();
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0); // Reset Y velocity before jumping
        rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
    }
}
