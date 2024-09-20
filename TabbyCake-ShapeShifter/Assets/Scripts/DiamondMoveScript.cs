using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondMoveScript : MonoBehaviour
{
    // rigidbody of the diamond
    public Rigidbody2D diamondRigidbody;
    public bool moveDown;
    // Start is called before the first frame update
    void Start()
    {
        // get own rigidbody
        diamondRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // if moveDown, the diamond should move down then up over the course of 1second, then repeat indefinitely
        if (moveDown)
        {
            StartCoroutine(MoveDown());
            moveDown = false;
        }
        else
        {
            StartCoroutine(MoveUp());
            moveDown = true;
        }
    }

    // move up after 1 second
    IEnumerator MoveUp()
    {
        // wait for 1 second
        yield return new WaitForSeconds(1);
        // move up
        Debug.Log("Moving up");
        diamondRigidbody.velocity = new Vector2(0, 1);
    }

    IEnumerator MoveDown()
    {
        // wait for 1 second
        yield return new WaitForSeconds(1);
        // move down
        Debug.Log("Moving down");
        diamondRigidbody.velocity = new Vector2(0, -1);
    }
}
