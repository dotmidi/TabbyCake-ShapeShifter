using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondMoveScript : MonoBehaviour
{
    public Rigidbody2D diamondRigidbody;
    private PlayerScript playerScript;
    public bool moveDown = true;
    private float timer = 0f;
    private float moveInterval;
    public float lowerLimit = -4f;
    public float upperLimit = 4f;

    private Vector2 moveUpVector = new Vector2(0, 1);
    private Vector2 moveDownVector = new Vector2(0, -1);

    // Start is called before the first frame update
    void Start()
    {
        diamondRigidbody = GetComponent<Rigidbody2D>();
        SetRandomInterval(); // Initialize the move interval
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerScript = player.GetComponent<PlayerScript>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer < moveInterval)
            return;

        timer -= moveInterval;

        float currentY = diamondRigidbody.position.y;

        if (moveDown)
        {
            if (currentY > lowerLimit)
            {
                diamondRigidbody.MovePosition(diamondRigidbody.position + moveDownVector);
            }
            else
            {
                moveDown = false;
            }
        }
        else
        {
            if (currentY < upperLimit)
            {
                diamondRigidbody.MovePosition(diamondRigidbody.position + moveUpVector);
            }
            else
            {
                moveDown = true;
            }
        }

        SetRandomInterval();

        if (playerScript != null && playerScript.isGlitchPowerupActive)
        {
            moveInterval /= 3f;
        }
    }

    private void SetRandomInterval()
    {
        moveInterval = Random.Range(0.1f, 1f); // Random time between 0.1 and 1 seconds
    }
}
