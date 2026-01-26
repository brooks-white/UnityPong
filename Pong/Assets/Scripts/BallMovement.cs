using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    // 🔒 Private fields (encapsulation)
    private float speed = 5f;
    private Vector2 direction = new Vector2(1f, 1f).normalized;
    private Rigidbody2D rb;

    // ✅ Public properties (getters/setters)
    public float Speed
    {
        get { return speed; }
        set { speed = Mathf.Max(0, value); }
    }

    public Vector2 Direction
    {
        get { return direction; }
        set { direction = value.normalized; }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed;
    }

    // ✅ Physics movement
    void FixedUpdate()
    {
        rb.velocity = direction * speed;
    }

    // ✅ Bounce off paddles and walls
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            direction = new Vector2(-direction.x, direction.y).normalized;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            direction = new Vector2(direction.x, -direction.y).normalized;
        }
    }

    // ✅ Reset helper (used later for scoring)
    public void ResetBall()
    {
        transform.position = Vector2.zero;
        direction = new Vector2(1f, 1f).normalized;
        rb.velocity = direction * speed;
    }
}