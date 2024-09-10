using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField]
    private Rigidbody2D rb;
    public float health = 1f;

    [Header("Game Over Settings")]
    [SerializeField]
    public GameObject GameOverCanvas;
    public TMP_Text ScoreText;
    public TMP_Text GameOverScoreText;

    // Gravity settings
    [Header("Gravity Settings")]
    private bool isGravityUp = true;

    [SerializeField]
    private float gravityMultiplier = 1.0f;

    // Raycast settings
    [Header("Raycast Settings")]
    [SerializeField]
    private LayerMask levelGeometryLayer;

    [Header("Raycast Settings")]
    [SerializeField]
    private float raycastLength = 1.0f;
    private Vector2 rayDirection = Vector2.right;
    public bool alive = true;
    public float HighScore;
    public float lineMoveSpeed = 2f;

    void Start()
    {
        HighScore = 0;
        alive = true;
    }

    void FixedUpdate()
    {
        // Physics check for raycast collision
        if (Physics2D.Raycast(transform.position, rayDirection, raycastLength, levelGeometryLayer))
        {
            alive = false;
        }
        // if coming into contact with a specific object, the player moves backwards 
        if (Physics2D.Raycast(transform.position, rayDirection, raycastLength, levelGeometryLayer))
        {
            transform.position = new Vector2(transform.position.x - lineMoveSpeed * Time.deltaTime, transform.position.y);
        }
        // if the player goes out of bounds, they die. this is x position -9.5
        if (transform.position.x < -9.5f)
        {
            alive = false;
        }
        if (!alive)
        {
            // freeze all objects in the scene
            Time.timeScale = 0;
            GameOverCanvas.gameObject.SetActive(true);
            ScoreText.gameObject.SetActive(false);
            GameOverScoreText.text = "High Score: " + HighScore.ToString("0");
        }
    }

    void Update()
    {
        // Handle gravity flip on spacebar press
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FlipGravity();
        }

        // flip gravity when swiping up
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                if (touch.deltaPosition.y > 0)
                {
                    GravityDown();
                }
                else
                {
                    GravityUp();
                }
            }
        }

        // Update score
        if (alive)
        {
            HighScore += (Time.deltaTime * 1000) * 0.1f;
            ScoreText.text = HighScore.ToString("0");
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

    void GravityUp()
    {
        rb.gravityScale = gravityMultiplier;
    }

    void GravityDown()
    {
        rb.gravityScale = -gravityMultiplier;
    }

    public void BlockHit()
    {
        Debug.Log("Player hit by block, playerscript side");
        // the player has 2 lives, if they get hit by a block, they lose a life
        health -= 0.5f;
        // make the player's sprite disappear and reappear 5 times
        StartCoroutine(FlashSprite());
        // if the player has no lives left, they die
        if (health <= 0)
        {
            alive = false;
        }
    }

    IEnumerator FlashSprite()
    {
        for (int i = 0; i < 3; i++)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(0.5f);
            GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
