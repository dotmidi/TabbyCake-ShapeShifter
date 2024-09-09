using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] public GameObject GameOverCanvas;
    public TMP_Text ScoreText;
    public TMP_Text GameOverScoreText;

    // Gravity settings
    private bool isGravityUp = true;
    [SerializeField] private float gravityMultiplier = 1.0f;

    // Raycast settings
    [SerializeField] private LayerMask levelGeometryLayer;
    [SerializeField] private float raycastLength = 1.0f;
    private Vector2 rayDirection = Vector2.right;
    public bool alive = true;
    public float HighScore;

    //stupid settings
    public float lineMoveSpeed = 2f;

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

        if (alive)
        {
            // for every frame, update the score
            HighScore = Time.frameCount * 0.1f;
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
}
