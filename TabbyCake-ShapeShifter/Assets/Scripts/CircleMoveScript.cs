using System.Collections;
using UnityEngine;
using TMPro;

public class CircleMovementScript : MonoBehaviour
{
    private float speed = -100f; // Set your desired speed
    public float smoothTime = 0.3f; // Time to smooth the movement
    private Rigidbody2D rb;
    private Vector2 velocity = Vector2.zero; // Used for SmoothDamp
    private PlayerScript playerScript;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Calculate the target position
        Vector2 targetPosition = rb.position + new Vector2(speed * Time.deltaTime, 0);

        // Smoothly move towards the target position
        rb.position = Vector2.SmoothDamp(rb.position, targetPosition, ref velocity, smoothTime);

        if (playerScript.isGlitchPowerupActive)
        {
            StartCoroutine(ToggleSpriteRenderer());
        }
    }

    private IEnumerator ToggleSpriteRenderer()
    {
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Apply bounce force
        Vector2 bounceForce = new Vector2(0, 2f); // Adjust as needed
        rb.AddForce(bounceForce, ForceMode2D.Impulse);
    }
}
