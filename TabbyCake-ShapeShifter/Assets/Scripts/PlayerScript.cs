using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Shapes")]
    [SerializeField]
    private Sprite[] playerShapes;

    [Header("Player Settings")]
    [SerializeField]
    private Rigidbody2D rb;
    public float health = 1f;

    [Header("Game Over Settings")]
    [SerializeField]
    private GameObject GameOverCanvas;
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

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        Time.timeScale = 1;
        HighScore = 0;
        alive = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (transform.position.x < -9.5f)
        {
            alive = false;
        }

        if (!alive)
        {
            Time.timeScale = 0;
            GameOverCanvas.SetActive(true);
            ScoreText.gameObject.SetActive(false);
            GameOverScoreText.text = "High Score: " + HighScore.ToString("0");
        }
    }

    void Update()
    {
        HandlePlayerSpriteRotation();
        HandleInput();

        if (alive)
        {
            HighScore += Time.deltaTime * 100;
            ScoreText.text = HighScore.ToString("0");
        }
    }

    private void HandlePlayerSpriteRotation()
    {
        if (transform.position.y > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void ChangeShape(string shape)
    {
        switch (shape)
        {
            case "Square":
                spriteRenderer.sprite = playerShapes[0];
                break;
            case "Triangle":
                spriteRenderer.sprite = playerShapes[1];
                break;
            default:
                break;
        }
    }

    private void HandleInput()
    {
        if (
            Input.GetMouseButtonDown(0)
            || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        )
        {
            ChangeShape("ehe");
        }

        if (Input.touchCount > 0)
        {
            HandleTouchInput(Input.GetTouch(0));
        }
    }

    private void HandleTouchInput(Touch touch)
    {
        if (touch.phase == TouchPhase.Moved)
        {
            GravityChange(touch.deltaPosition.y > 0 ? -10 : 10);
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
        Time.timeScale = 0.5f;
        StartCoroutine(SlowObstaclesTimer());
    }

    public void BlockHit()
    {
        health -= 0.5f;
        StartCoroutine(FlashSprite());

        if (health <= 0)
        {
            alive = false;
        }
    }

    IEnumerator SlowObstaclesTimer()
    {
        yield return new WaitForSeconds(0.3f);
        Time.timeScale = 1;
    }

    IEnumerator FlashSprite()
    {
        for (int i = 0; i < 5; i++)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
