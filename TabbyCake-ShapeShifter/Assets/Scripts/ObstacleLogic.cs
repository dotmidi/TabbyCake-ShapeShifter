using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleLogic : MonoBehaviour
{
    private const float BoundaryX = -30f;
    private PlayerScript playerScript;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerScript = player.GetComponent<PlayerScript>();
        }
        if (playerScript.isGlitchPowerupActive)
        {
            StartCoroutine(ToggleSpriteRenderer());
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
        Transform childTransform = transform.Find("circle");
        if (childTransform != null)
        {
            SpriteRenderer spriteRenderer = childTransform.GetComponent<SpriteRenderer>();

            // Keep running while GlitchPowerup is active
            while (playerScript.isGlitchPowerupActive)
            {
                if (spriteRenderer != null)
                {
                    spriteRenderer.enabled = !spriteRenderer.enabled; // Toggle the visibility
                }
                yield return new WaitForSeconds(0.5f); // Wait for 0.5 seconds before toggling again
            }

            // Ensure sprite is visible when GlitchPowerup ends
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = true;
            }
        }
    }

    // void GlitchPowerupHandler()
    // {
    //     switch (tag)
    //     {
    //         case "RedObstacle":
    //             if (playerScript.isStarPowerupActive)
    //             {
    //                 Destroy(gameObject);
    //             }
    //             else
    //             {
    //                 playerScript.BlockHit();
    //             }
    //             break;
    //         case "DiamondObstacle":
    //             if (playerScript.isStarPowerupActive)
    //             {
    //                 Destroy(gameObject);
    //             }
    //             else
    //             {
    //                 playerScript.BlockHit();
    //             }
    //             break;
    //     }
    // }

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
            case "ChangeToSquare":
                playerScript.ChangeShape("Square");
                Destroy(gameObject);
                break;
            case "ChangeToTriangle":
                playerScript.ChangeShape("Triangle");
                Destroy(gameObject);
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
