using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleLogic : MonoBehaviour
{
    private const float BoundaryX = -30f;
    private PlayerScript playerScript;

    void Start()
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            playerScript = player.GetComponent<PlayerScript>();
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
                playerScript.StarPowerup();
                Destroy(gameObject);
                break;
        }
    }
}
