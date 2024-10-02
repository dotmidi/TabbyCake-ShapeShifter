using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleLogic : MonoBehaviour
{
    private const float BoundaryX = -30f;
    private PlayerScript playerScript;
    private SpriteRenderer circleSpriteRenderer;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerScript = player.GetComponent<PlayerScript>();
        }

        if (CompareTag("CircleObstacle") && playerScript.isGlitchPowerupActive)
        {
            Transform circleTransform = transform.Find("circle");
            if (circleTransform != null)
            {
                circleSpriteRenderer = circleTransform.GetComponent<SpriteRenderer>();
            }
            StartCoroutine(ToggleSpriteRenderer());
        }

        if (CompareTag("RedObstacle") && playerScript.isGlitchPowerupActive)
        {
            Collider2D collider = GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.isTrigger = false;
            }
        }

        if (CompareTag("DiamondObstacle") && playerScript.isGlitchPowerupActive)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 0;
            }
        }
    }

    void Update() { }

    void FixedUpdate()
    {
        if (transform.position.x < BoundaryX)
        {
            Destroy(gameObject);
            Transform parentTransform = transform.parent;
            if (parentTransform != null)
            {
                Destroy(parentTransform.gameObject);
            }
        }
    }

    private IEnumerator ToggleSpriteRenderer()
    {
        while (playerScript.isGlitchPowerupActive)
        {
            if (circleSpriteRenderer != null)
            {
                circleSpriteRenderer.enabled = !circleSpriteRenderer.enabled;
            }
            yield return new WaitForSeconds(0.5f);
        }

        if (circleSpriteRenderer != null)
        {
            circleSpriteRenderer.enabled = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && playerScript != null)
        {
            if (playerScript.isStarPowerupActive)
            {
                Destroy(gameObject);
            }
            else
            {
                HandleObstacleHit();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (
            other.gameObject.CompareTag("Player")
            && playerScript != null
            && CompareTag("YellowObstacle")
        )
        {
            playerScript.SlowObstacles();
        }
        if (
            other.gameObject.CompareTag("Player")
            && playerScript != null
            && CompareTag("CircleObstacle")
        )
        {
            Destroy(gameObject);
            playerScript.BlockHit();
        }
    }

    private void HandleObstacleHit()
    {
        switch (tag)
        {
            case "RedObstacle":
                if (playerScript.isStarPowerupActive)
                {
                    Destroy(gameObject);
                }
                else
                {
                    playerScript.BlockHit();
                }
                break;
            case "DiamondObstacle":
                if (playerScript.isStarPowerupActive)
                {
                    Destroy(gameObject);
                }
                else
                {
                    playerScript.BlockHit();
                }
                break;
            case "StarPowerup":
                playerScript.ActivateStarPowerup();
                Destroy(gameObject);
                break;
            case "GlitchPowerup":
                playerScript.ActivateGlitchPowerup();
                Destroy(gameObject);
                break;
        }
    }
}
