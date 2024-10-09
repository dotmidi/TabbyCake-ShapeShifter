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

    [Header("Pickup Effects")]
    public GameObject glitchEffect;
    public GameObject starEffectParent;
    public GameObject starEffectSquare;
    public GameObject starEffectTriangle;
    public GameObject starEffectCircle;
    public GameObject starEffectDiamond;

    [Header("Player Settings")]
    [SerializeField]
    private Rigidbody2D PlayerRigidBody;
    public float health = 1f;
    public float gravityScale;
    public LeaderboardManager leaderboardManager;

    [Header("Player Hitboxes")]
    public BoxCollider2D squareHitbox;
    public PolygonCollider2D triangleHitbox;
    public PolygonCollider2D diamondHitbox;
    public CircleCollider2D circleHitbox;

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
    public GameObject FullHeart2;
    public GameObject HalfHeart;
    public GameObject HalfHeart2;
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
        // PlayerPrefs.SetInt("Controls", 0);
        // Debug.Log("Control setup:" + PlayerPrefs.GetInt("Controls"));
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
        float rotationSpeed = 20f;

        Quaternion targetRotation =
            transform.position.y > 0 ? Quaternion.Euler(0, 0, 180) : Quaternion.identity;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );
    }

    private bool isGravityChangeAllowed = true; // Flag to track if gravity change is allowed

    private void HandleInput()
    {
        if (Input.touchCount > 0)
        {
            HandleTouchInput(Input.GetTouch(0));
        }
    }

    private void HandleTouchInput(Touch touch)
    {
        if (PlayerPrefs.GetInt("Controls") == 0)
        {
            if (touch.phase == TouchPhase.Moved && isGravityChangeAllowed)
            {
                isGravityChangeAllowed = false; // Prevent further changes until touch ends
                // Change gravity based on touch drag direction
                if (touch.deltaPosition.y > 0) // Swipe up
                {
                    GravityChange(-Mathf.Abs(gravityScale)); // Make gravity negative
                }
                else // Swipe down
                {
                    GravityChange(Mathf.Abs(gravityScale)); // Make gravity positive
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isGravityChangeAllowed = true; // Allow gravity change again when touch ends or is canceled
            }
        }
        else if (PlayerPrefs.GetInt("Controls") == 1)
        {
            if (touch.phase == TouchPhase.Began && isGravityChangeAllowed)
            {
                isGravityChangeAllowed = false; // Prevent further changes until touch ends
                // Invert gravity scale on screen tap
                GravityChange(
                    gravityScale > 0 ? -Mathf.Abs(gravityScale) : Mathf.Abs(gravityScale)
                );
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isGravityChangeAllowed = true; // Allow gravity change again when touch ends or is canceled
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

            if (health == 1.5f)
            {
                FullHeart2.SetActive(false);
            }
            else if (health == 1f)
            {
                HalfHeart2.SetActive(false);
            }
            else if (health == 0.5f)
            {
                FullHeart.SetActive(false);
            }
            else if (health == 0f)
            {
                HalfHeart.SetActive(false);
                alive = false;
            }
        }
    }

    public void ActivateStarPowerup()
    {
        StartCoroutine(PowerupTimer("Star"));
    }

    public void ActivateGlitchPowerup()
    {
        StartCoroutine(PowerupTimer("Glitch"));
    }

    private void GameOver()
    {
        UIScript.StopMusicAferDeath();
        leaderboardManager.AddScore((int)HighScore);
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

    private IEnumerator PowerupTimer(string powerup)
    {
        switch (powerup)
        {
            case "Star":
                yield return StartCoroutine(HandleStarPowerup());
                break;
            case "Glitch":
                yield return StartCoroutine(HandleGlitchPowerup());
                break;
        }
    }

    private IEnumerator HandleGlitchPowerup()
    {
        isGlitchPowerupActive = true;
        glitchEffect.SetActive(true);
        DoubleHP();
        yield return new WaitForSeconds(18);
        StartCoroutine(FlashSprite(20, 0.1f));
        yield return new WaitForSeconds(2);
        glitchEffect.SetActive(false);
        isGlitchPowerupActive = false;
    }

    private IEnumerator HandleStarPowerup()
    {
        isStarPowerupActive = true;
        starEffectParent.SetActive(true);
        yield return new WaitForSeconds(8);
        StartCoroutine(FlashSprite(20, 0.1f));
        yield return new WaitForSeconds(2);
        starEffectParent.SetActive(false);
        isStarPowerupActive = false;
    }

    private void DoubleHP()
    {
        if (health == 0.5f)
        {
            health = 1f;
            FullHeart.SetActive(true);
            HalfHeart.SetActive(true);
        }
        else if (health == 1f)
        {
            health = 2f;
            FullHeart2.SetActive(true);
            HalfHeart2.SetActive(true);
        }
        else if (health == 1.5f)
        {
            health = 2f;
            FullHeart2.SetActive(true);
            HalfHeart2.SetActive(true);
        }
        else if (health == 2f)
        {
            FullHeart.SetActive(true);
            HalfHeart.SetActive(true);
            FullHeart2.SetActive(true);
            HalfHeart2.SetActive(true);
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
            SpriteRenderer.enabled = !SpriteRenderer.enabled;
            yield return new WaitForSeconds(flashDuration);
        }
        SpriteRenderer.enabled = true;
    }

    private void SetPlayerSprites()
    {
        //Square
        if (UIScript.cosmeticSaveData.squareSprite == 0)
        {
            currentSquareSprite = squareSprite0;
        }
        else if (UIScript.cosmeticSaveData.squareSprite == 1)
        {
            currentSquareSprite = squareSprite1;
        }
        else if (UIScript.cosmeticSaveData.squareSprite == 2)
        {
            currentSquareSprite = squareSprite2;
        }
        else if (UIScript.cosmeticSaveData.squareSprite == 3)
        {
            currentSquareSprite = squareSprite3;
        }
        else if (UIScript.cosmeticSaveData.squareSprite == 4)
        {
            currentSquareSprite = squareSprite4;
        }
        SpriteRenderer.sprite = currentSquareSprite; //The game starts with square, this makes sure its the right square cosmetic

        //Triangle
        if (UIScript.cosmeticSaveData.triangleSprite == 0)
        {
            currentTriangleSprite = triangleSprite0;
        }
        else if (UIScript.cosmeticSaveData.triangleSprite == 1)
        {
            currentTriangleSprite = triangleSprite1;
        }
        else if (UIScript.cosmeticSaveData.triangleSprite == 2)
        {
            currentTriangleSprite = triangleSprite2;
        }
        else if (UIScript.cosmeticSaveData.triangleSprite == 3)
        {
            currentTriangleSprite = triangleSprite3;
        }
        else if (UIScript.cosmeticSaveData.triangleSprite == 4)
        {
            currentTriangleSprite = triangleSprite4;
        }

        //Circle
        if (UIScript.cosmeticSaveData.circleSprite == 0)
        {
            currentCircleSprite = circleSprite0;
        }
        else if (UIScript.cosmeticSaveData.circleSprite == 1)
        {
            currentCircleSprite = circleSprite1;
        }
        else if (UIScript.cosmeticSaveData.circleSprite == 2)
        {
            currentCircleSprite = circleSprite2;
        }
        else if (UIScript.cosmeticSaveData.circleSprite == 3)
        {
            currentCircleSprite = circleSprite3;
        }
        else if (UIScript.cosmeticSaveData.circleSprite == 4)
        {
            currentCircleSprite = circleSprite4;
        }

        //Diamond
        if (UIScript.cosmeticSaveData.diamondSprite == 0)
        {
            currentDiamondSprite = diamondSprite0;
        }
        else if (UIScript.cosmeticSaveData.diamondSprite == 1)
        {
            currentDiamondSprite = diamondSprite1;
        }
        else if (UIScript.cosmeticSaveData.diamondSprite == 2)
        {
            currentDiamondSprite = diamondSprite2;
        }
        else if (UIScript.cosmeticSaveData.diamondSprite == 3)
        {
            currentDiamondSprite = diamondSprite3;
        }
        else if (UIScript.cosmeticSaveData.diamondSprite == 4)
        {
            currentDiamondSprite = diamondSprite4;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) //this makes sure the sprite gets changed to the right shape
    {
        switch (other.gameObject.tag)
        {
            case "SquareChunk":
                SpriteRenderer.sprite = currentSquareSprite;
                starEffectCircle.SetActive(false);
                starEffectDiamond.SetActive(false);
                starEffectTriangle.SetActive(false);
                starEffectSquare.SetActive(true);
                squareHitbox.enabled = true;
                triangleHitbox.enabled = false;
                diamondHitbox.enabled = false;
                circleHitbox.enabled = false;
                break;
            case "TriangleChunk":
                SpriteRenderer.sprite = currentTriangleSprite;
                starEffectCircle.SetActive(false);
                starEffectDiamond.SetActive(false);
                starEffectTriangle.SetActive(true);
                starEffectSquare.SetActive(false);
                squareHitbox.enabled = false;
                triangleHitbox.enabled = true;
                diamondHitbox.enabled = false;
                circleHitbox.enabled = false;
                break;
            case "DiamondChunk":
                SpriteRenderer.sprite = currentDiamondSprite;
                starEffectCircle.SetActive(false);
                starEffectDiamond.SetActive(true);
                starEffectTriangle.SetActive(false);
                starEffectSquare.SetActive(false);
                squareHitbox.enabled = false;
                triangleHitbox.enabled = false;
                diamondHitbox.enabled = true;
                circleHitbox.enabled = false;
                break;
            case "CircleObstacle":
                SpriteRenderer.sprite = currentCircleSprite;
                starEffectCircle.SetActive(true);
                starEffectDiamond.SetActive(false);
                starEffectTriangle.SetActive(false);
                starEffectSquare.SetActive(false);
                squareHitbox.enabled = false;
                triangleHitbox.enabled = false;
                diamondHitbox.enabled = false;
                circleHitbox.enabled = true;
                break;
        }
    }
}
