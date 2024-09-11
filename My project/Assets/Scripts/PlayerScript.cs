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

    // Raycast settings
    [Header("Raycast Settings")]
    [SerializeField]
    private GameObject FloorAndCeiling;

    [Header("Raycast Settings")]
    [SerializeField]
    private float raycastLength = 1.0f;
    private Vector2 rayDirection = Vector2.right;
    public bool alive = true;
    public float HighScore;
    public float lineMoveSpeed = 2f;

    void Start()
    {
        Time.timeScale = 1;
        HighScore = 0;
        alive = true;
    }

    void FixedUpdate()
    {
        if (
            Physics2D.Raycast(
                transform.position,
                rayDirection,
                raycastLength,
                FloorAndCeiling.layer
            )
        )
        {
            // Debug.Log("Hit floor or ceiling");
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
        // Debug.Log(rb.gravityScale);

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                if (touch.deltaPosition.y > 0)
                {
                    GravityChange(-50);
                    StartCoroutine(ResetGravity(-0.1f));
                }
                else
                {
                    GravityChange(50);
                    StartCoroutine(ResetGravity(0.1f));
                }
            }
        }

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

    public void GravityChange(float gravity)
    {
        rb.gravityScale = gravity;
    }

    public void SlowObstacles()
    {
        // set timescale to 0.5 for 5 seconds
        Debug.Log("SlowObstacles called");
        Time.timeScale = 0.75f;
        StartCoroutine(SlowObstaclesTimer());
    }

    public void BlockHit()
    {
        // Debug.Log("Player hit by block, playerscript side");
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

    IEnumerator SlowObstaclesTimer()
    {
        yield return new WaitForSeconds(2);
        Time.timeScale = 1;
    }

    // wait 0.5 seconds before resetting the gravity
    IEnumerator ResetGravity(float gravity)
    {
        yield return new WaitForSeconds(0.5f);
        GravityChange(gravity);
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
