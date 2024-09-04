using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool onGround = false;
    [SerializeField] private float groundLength = 0.5f;
    [SerializeField] private float jumpSpeed = 15.0f;
    [SerializeField] private Vector3 colliderOffset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer)
                || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);

        //movement
        Vector3 playerInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0).normalized;
        rb.velocity = new Vector2(playerInput.x * speed, rb.velocity.y);

        if(Input.GetKeyDown(KeyCode.UpArrow) && onGround)
        {
            Jump();
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
    }
}
