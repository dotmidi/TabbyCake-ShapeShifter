using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Shapes")]
    [SerializeField]
    private Sprite[] playerShapes;

    [Header("Player Settings")]
    [SerializeField]
    private Rigidbody2D PlayerRigidBody;
    public float health = 1f;
    public float gravityScale;

    [Header("UI Settings")]
    [SerializeField]
    public TMP_Text ScoreText;
    public GameObject GameOverCanvas;
    public TMP_Text GameOverScoreText;

    [Header("Raycast Settings")]
    [SerializeField]
    private GameObject FloorAndCeiling;
    private const float raycastLength = 1.0f;
    private static readonly Vector2 rayDirection = Vector2.right;
    public bool alive = true;
    public float HighScore;
    public bool isStarPowerupActive = false;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        Time.timeScale = 1;
        HighScore = 0;
        alive = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (transform.position.x < -9.5f)
        {
            alive = false;
        }

        if (!alive)
        {
            GameOver();
        }
    }

    private void Update()
    {
        if (alive)
        {
            HandlePlayerSpriteRotation();
            HandleInput();
            UpdateScore();
        }
    }

    private void HandlePlayerSpriteRotation()
    {
        transform.rotation =
            transform.position.y > 0 ? Quaternion.Euler(0, 0, 180) : Quaternion.identity;
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
        }
    }

    private void HandleInput()
    {
        if (
            Input.GetMouseButtonDown(0)
            || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began
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
            GravityChange(touch.deltaPosition.y > 0 ? -gravityScale : gravityScale);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * raycastLength);
    }

    public void GravityChange(float gravity)
    {
        PlayerRigidBody.gravityScale = gravity;
    }

    public void SlowObstacles()
    {
        if (!isStarPowerupActive)
        {
            Time.timeScale = 0.5f;
            StartCoroutine(SlowObstaclesTimer());
        }
    }

    public void BlockHit()
    {
        if (!isStarPowerupActive)
        {
            health -= 0.5f;
            StartCoroutine(FlashSprite(5, 0.1f));

            if (health <= 0)
            {
                alive = false;
            }
        }
    }

    public void StarPowerup()
    {
        StartCoroutine(FlashSprite(50, 0.1f));
        StartCoroutine(StarPowerupTimer());
    }

    private void GameOver()
    {
        Time.timeScale = 0;
        GameOverCanvas.SetActive(true);
        ScoreText.gameObject.SetActive(false);
        GameOverScoreText.text = $"High Score: {HighScore:0}";
    }

    private void UpdateScore()
    {
        HighScore += Time.deltaTime * 100;
        ScoreText.text = HighScore.ToString("0");
    }

    private IEnumerator StarPowerupTimer()
    {
        isStarPowerupActive = true;
        yield return new WaitForSeconds(5);
        isStarPowerupActive = false;
    }

    private IEnumerator SlowObstaclesTimer()
    {
        yield return new WaitForSeconds(0.3f);
        Time.timeScale = 1;
    }

    private IEnumerator FlashSprite(int flashCount, float flashDuration)
    {
        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(flashDuration);
        }
        spriteRenderer.enabled = true;
    }
}
