using System.Collections;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System;

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

    [Header("Player Sprites")]
    [SerializeField]
    private SpriteRenderer SpriteRenderer;
    private Sprite currentSquareSprite;
    public Sprite squareSprite0;
    public Sprite squareSprite1;
    public Sprite squareSprite2;
    public Sprite squareSprite3;
    public Sprite squareSprite4;
    private Sprite currentTriangleSprite;
    public Sprite triangleSprite0;
    public Sprite triangleSprite1;
    public Sprite triangleSprite2;
    public Sprite triangleSprite3;
    public Sprite triangleSprite4;
    private Sprite currentCircleSprite;
    public Sprite circleSprite0;
    public Sprite circleSprite1;
    public Sprite circleSprite2;
    public Sprite circleSprite3;
    public Sprite circleSprite4;
    private Sprite currentDiamondSprite;
    public Sprite diamondSprite0;
    public Sprite diamondSprite1;
    public Sprite diamondSprite2;
    public Sprite diamondSprite3;
    public Sprite diamondSprite4;

    [Header("UI Settings")]
    [SerializeField]
    public TMP_Text ScoreText;
    public GameObject GameOverCanvas;
    public TMP_Text GameOverScoreText;
    public GameObject FullHeart;
    public GameObject HalfHeart;
    public GameObject PauseButton;
    public UIFunctions UIScript;

    [Header("Raycast Settings")]
    [SerializeField]
    private GameObject FloorAndCeiling;
    private const float raycastLength = 1.0f;
    private static readonly Vector2 rayDirection = Vector2.right;
    public bool alive = true;
    public float HighScore;
    public bool isStarPowerupActive = false;
    public bool isGlitchPowerupActive = false;

    private void Start()
    {
        Time.timeScale = 1;
        HighScore = 0;
        alive = true;
        PlayerPrefs.SetInt("Control", 1);
        Debug.Log("Control setup:" + PlayerPrefs.GetInt("Control"));
        UIScript.LoadCosmeticData();
        SetPlayerSprites();
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
        // Define a fixed rotation speed
        float rotationSpeed = 20f; // Adjust this value as needed

        // Determine the target rotation based on the player's position
        Quaternion targetRotation =
            transform.position.y > 0 ? Quaternion.Euler(0, 0, 180) : Quaternion.identity;

        // Smoothly interpolate to the target rotation
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );
    }

    private void HandleInput()
    {
        if (Input.touchCount > 0)
        {
            HandleTouchInput(Input.GetTouch(0));
        }
    }

    private void HandleTouchInput(Touch touch)
    {
        if (PlayerPrefs.GetInt("Control") == 0)
        {
            if (touch.phase == TouchPhase.Moved)
            {
                // Move based on touch drag (vertical direction)
                GravityChange(touch.deltaPosition.y > 0 ? -gravityScale : gravityScale);
            }
        }
        else if (PlayerPrefs.GetInt("Control") == 1)
        {
            if (touch.phase == TouchPhase.Began)
            {
                // Invert gravity scale on screen tap
                if (gravityScale > 0)
                {
                    GravityChange(-gravityScale);
                }
                else
                {
                    GravityChange(Mathf.Abs(gravityScale)); // Make it positive if it's negative
                }
            }
        }
    }

    public void GravityChange(float gravity)
    {
        gravityScale = gravity; // Update gravityScale itself
        PlayerRigidBody.gravityScale = gravity;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * raycastLength);
    }

    public void SlowObstacles()
    {
        if (isGlitchPowerupActive)
        {
            Time.timeScale = 1.5f;
            StartCoroutine(SlowObstaclesTimer());
        }
        else if (!isStarPowerupActive)
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
            FullHeart.SetActive(false);

            if (health <= 0)
            {
                HalfHeart.SetActive(false);
                alive = false;
            }
        }
    }

    public void ActivateStarPowerup()
    {
        StartCoroutine(FlashSprite(50, 0.1f));
        StartCoroutine(StarPowerupTimer("Star"));
    }

    public void ActivateGlitchPowerup()
    {
        StartCoroutine(FlashSprite(100, 0.1f));
        StartCoroutine(StarPowerupTimer("Glitch"));
    }

    private void GameOver()
    {
        PauseButton.SetActive(false);
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

    private IEnumerator StarPowerupTimer(string powerup)
    {
        switch (powerup)
        {
            case "Star":
                isStarPowerupActive = true;
                yield return new WaitForSeconds(5);
                isStarPowerupActive = false;
                break;
            case "Glitch":
                isGlitchPowerupActive = true;
                yield return new WaitForSeconds(10);
                isGlitchPowerupActive = false;
                break;
        }
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
            SpriteRenderer.enabled = !SpriteRenderer.enabled; // Use SpriteRenderer here
            yield return new WaitForSeconds(flashDuration);
        }
        SpriteRenderer.enabled = true;
    }
    private void SetPlayerSprites()
    {
        //Square
        if (UIScript.cosmeticSaveData.squareSprite == 0) { currentSquareSprite = squareSprite0; }
        else if (UIScript.cosmeticSaveData.squareSprite == 1) { currentSquareSprite = squareSprite1; }
        else if (UIScript.cosmeticSaveData.squareSprite == 2) { currentSquareSprite = squareSprite2; }
        else if (UIScript.cosmeticSaveData.squareSprite == 3) { currentSquareSprite = squareSprite3; }
        else if (UIScript.cosmeticSaveData.squareSprite == 4) { currentSquareSprite = squareSprite4; }
        SpriteRenderer.sprite = currentSquareSprite; //The game starts with square, this makes sure its the right square cosmetic

        //Triangle
        if (UIScript.cosmeticSaveData.triangleSprite == 0) { currentTriangleSprite = triangleSprite0; }
        else if (UIScript.cosmeticSaveData.triangleSprite == 1) { currentTriangleSprite = triangleSprite1; }
        else if (UIScript.cosmeticSaveData.triangleSprite == 2) { currentTriangleSprite = triangleSprite2; }
        else if (UIScript.cosmeticSaveData.triangleSprite == 3) { currentTriangleSprite = triangleSprite3; }
        else if (UIScript.cosmeticSaveData.triangleSprite == 4) { currentTriangleSprite = triangleSprite4; }

        //Circle
        if (UIScript.cosmeticSaveData.circleSprite == 0) { currentCircleSprite = circleSprite0; }

        //Diamond
        if (UIScript.cosmeticSaveData.diamondSprite == 0) { currentDiamondSprite = diamondSprite0; }
    }

    private void OnTriggerEnter2D(Collider2D other) //this makes sure the sprite gets changed to the right shape
    {
        switch (other.gameObject.tag)
        {
            case "SquareChunk":
                SpriteRenderer.sprite = currentSquareSprite;
                break;
            case "TriangleChunk":
                SpriteRenderer.sprite = currentTriangleSprite;
                break;
            case "DiamondChunk":
                SpriteRenderer.sprite = currentDiamondSprite;
                break;
            case "CircleObstacle":
                SpriteRenderer.sprite = currentCircleSprite;
                break;
        }
    }
}