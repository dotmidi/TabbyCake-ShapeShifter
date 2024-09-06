using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    
    //Gravity settings
    enum gravityState { UP, DOWN };
    gravityState state = gravityState.UP;
    [SerializeField] private float gravityMultiplier;

    //Raycast stuff
    [SerializeField] private LayerMask levelGeometryLayer;
    [SerializeField] private float raycastLength;
    public bool alive;

    // Start is called before the first frame update
    void Start()
    {
        alive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Physics2D.Raycast(transform.position, Vector2.right, raycastLength, levelGeometryLayer))
        {
            alive = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            gravityFlip();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * raycastLength);
    }
    void gravityFlip()
    {
        if (state == gravityState.UP)
        {
            rb.gravityScale = gravityMultiplier;
            state = gravityState.DOWN;
        }
        else if (state == gravityState.DOWN)
        {
            rb.gravityScale = -gravityMultiplier;
            state = gravityState.UP;
        }
    }
}
