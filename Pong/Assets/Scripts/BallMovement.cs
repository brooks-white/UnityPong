using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
   public float speed = 3f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(speed, speed);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 velocity = rb.velocity;

        // Reverse Y direction when hitting top or bottom walls
        velocity.y = -velocity.y;

        rb.velocity = velocity;
    }
}
