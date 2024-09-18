using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class DiamondMoveScript : MonoBehaviour
{
    public float riseHeight = 1f; //how many units it should rise
    public float riseSpeed = 1f; //the speed at which the rise happens

    public Rigidbody2D rb; //rigidbody reference

    public enum State
    {
        UP,
        DOWN
    } //enumerator to describe current state of the diamond

    State state; //make variable of type State

    float startPosY;
    float endPosY;

    // Start is called before the first frame update
    void Start()
    {
        state = State.UP;
        startPosY = transform.position.y;
        endPosY = transform.position.y + riseHeight;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.UP)
        {
            //rb.velocity += riseSpeed * Time.deltaTime * Vector2.up; //smoothing version
            rb.velocity = new Vector2(rb.velocity.x, riseSpeed);
            if (transform.position.y >= endPosY)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0); //reset Y velocity
                state = State.DOWN;
            }
        }

        if (state == State.DOWN)
        {
            //rb.velocity += riseSpeed * Time.deltaTime * Vector2.down; //smoothing version
            rb.velocity = new Vector2(rb.velocity.x, -riseSpeed);
            if (transform.position.y <= startPosY)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0); //reset Y velocity
                state = State.UP;
            }
        }
    }
}
