using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Shapes")]
    [SerializeField]
    private Sprite[] playerShapes;

    [Header("Pickup Effects")]
    public GameObject glitchEffect,
        starEffectParent,
        starEffectSquare,
        starEffectTriangle,
        starEffectCircle,
        starEffectDiamond;

    [Header("Player Settings")]
    [SerializeField]
    private Rigidbody2D PlayerRigidBody;
    public float health = 1f,
        gravityScale;
    public LeaderboardManager leaderboardManager;

    [Header("Player Hitboxes")]
    public BoxCollider2D squareHitbox;
    public PolygonCollider2D triangleHitbox,
        diamondHitbox;
    public CircleCollider2D circleHitbox;

    [Header("Player Sprites")]
    [SerializeField]
    private SpriteRenderer SpriteRenderer;
    private Sprite currentSquareSprite,
        currentTriangleSprite,
        currentCircleSprite,
        currentDiamondSprite;
    public Sprite squareSprite0,
        squareSprite1,
        squareSprite2,
        squareSprite3,
        squareSprite4;
    public Sprite triangleSprite0,
        triangleSprite1,
        triangleSprite2,
        triangleSprite3,
        triangleSprite4;
    public Sprite circleSprite0,
        circleSprite1,
        circleSprite2,
        circleSprite3,
        circleSprite4;
    public Sprite diamondSprite0,
        diamondSprite1,
        diamondSprite2,
        diamondSprite3,
        diamondSprite4;

    [Header("UI Settings")]
    [SerializeField]
    public TMP_Text ScoreText,
        GameOverScoreText;
    public GameObject GameOverCanvas,
        FullHeart,
        FullHeart2,
        HalfHeart,
        HalfHeart2,
        PauseButton;
    public UIFunctions UIScript;

    [Header("Raycast Settings")]
    [SerializeField]
    private GameObject FloorAndCeiling;
    private const float raycastLength = 1.0f;
    private static readonly Vector2 rayDirection = Vector2.right;
    public bool alive = true,
        isStarPowerupActive = false,
        isGlitchPowerupActive = false;
    public float HighScore;

    private bool isGravityChangeAllowed = true;

    private void Start()
    {
        Time.timeScale = 1;
        HighScore = 0;
        alive = true;
        UIScript.LoadCosmeticData();
        SetPlayerSprites();
    }

    private void FixedUpdate()
    {
        if (transform.position.x < -9.5f)
            alive = false;
        if (!alive)
            GameOver();
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

    private void HandleInput()
    {
        if (Input.touchCount > 0)
            HandleTouchInput(Input.GetTouch(0));
    }

    private void HandleTouchInput(Touch touch)
    {
        int controlMode = PlayerPrefs.GetInt("Controls");

        if (controlMode == 0 && touch.phase == TouchPhase.Moved && isGravityChangeAllowed)
        {
            isGravityChangeAllowed = false;
            GravityChange(
                touch.deltaPosition.y > 0 ? -Mathf.Abs(gravityScale) : Mathf.Abs(gravityScale)
            );
        }
        else if (controlMode == 1 && touch.phase == TouchPhase.Began && isGravityChangeAllowed)
        {
            isGravityChangeAllowed = false;
            GravityChange(gravityScale > 0 ? -Mathf.Abs(gravityScale) : Mathf.Abs(gravityScale));
        }

        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            isGravityChangeAllowed = true;
        }
    }

    public void GravityChange(float gravity)
    {
        gravityScale = gravity;
        PlayerRigidBody.gravityScale = gravity;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * raycastLength);
    }

    public void SlowObstacles()
    {
        Time.timeScale = isGlitchPowerupActive ? 1.5f : 0.5f;
        StartCoroutine(SlowObstaclesTimer());
    }

    public void BlockHit()
    {
        if (isStarPowerupActive)
            return;

        health -= 0.5f;
        StartCoroutine(FlashSprite(5, 0.1f));

        if (health == 1.5f)
            FullHeart2.SetActive(false);
        else if (health == 1f)
            HalfHeart2.SetActive(false);
        else if (health == 0.5f)
            FullHeart.SetActive(false);
        else if (health == 0f)
        {
            HalfHeart.SetActive(false);
            alive = false;
        }
    }

    public void ActivateStarPowerup() => StartCoroutine(PowerupTimer("Star"));

    public void ActivateGlitchPowerup() => StartCoroutine(PowerupTimer("Glitch"));

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
        if (powerup == "Star")
            yield return HandleStarPowerup();
        else if (powerup == "Glitch")
            yield return HandleGlitchPowerup();
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
        health = Mathf.Clamp(health + 1f, 0.5f, 2f);
        FullHeart.SetActive(health >= 0.5f);
        HalfHeart.SetActive(health >= 1f);
        FullHeart2.SetActive(health >= 1.5f);
        HalfHeart2.SetActive(health == 2f);
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
        currentSquareSprite = SelectSprite(
            UIScript.cosmeticSaveData.squareSprite,
            squareSprite0,
            squareSprite1,
            squareSprite2,
            squareSprite3,
            squareSprite4
        );
        currentTriangleSprite = SelectSprite(
            UIScript.cosmeticSaveData.triangleSprite,
            triangleSprite0,
            triangleSprite1,
            triangleSprite2,
            triangleSprite3,
            triangleSprite4
        );
        currentCircleSprite = SelectSprite(
            UIScript.cosmeticSaveData.circleSprite,
            circleSprite0,
            circleSprite1,
            circleSprite2,
            circleSprite3,
            circleSprite4
        );
        currentDiamondSprite = SelectSprite(
            UIScript.cosmeticSaveData.diamondSprite,
            diamondSprite0,
            diamondSprite1,
            diamondSprite2,
            diamondSprite3,
            diamondSprite4
        );

        SpriteRenderer.sprite = currentSquareSprite;
    }

    private Sprite SelectSprite(int index, params Sprite[] sprites) =>
        sprites[Mathf.Clamp(index, 0, sprites.Length - 1)];

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "SquareChunk":
                UpdateShape(currentSquareSprite, starEffectSquare, squareHitbox);
                break;
            case "TriangleChunk":
                UpdateShape(currentTriangleSprite, starEffectTriangle, triangleHitbox);
                break;
            case "DiamondChunk":
                UpdateShape(currentDiamondSprite, starEffectDiamond, diamondHitbox);
                break;
            case "CircleObstacle":
                UpdateShape(currentCircleSprite, starEffectCircle, circleHitbox);
                break;
        }
    }

    private void UpdateShape(Sprite shapeSprite, GameObject starEffect, Collider2D hitbox)
    {
        SpriteRenderer.sprite = shapeSprite;
        starEffectSquare.SetActive(false);
        starEffectTriangle.SetActive(false);
        starEffectDiamond.SetActive(false);
        starEffectCircle.SetActive(false);
        starEffect.SetActive(true);

        squareHitbox.enabled = false;
        triangleHitbox.enabled = false;
        diamondHitbox.enabled = false;
        circleHitbox.enabled = false;
        hitbox.enabled = true;
    }
}
