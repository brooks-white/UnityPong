using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour, ICollidable
{
    [SerializeField] private float speed = 5f;
    private Vector2 direction = new Vector2(1f, 1f).normalized;
    private Rigidbody2D rb;

    public float Speed
    {
        get => speed;
        set => speed = Mathf.Max(0f, value);
    }

    public Vector2 Direction
    {
        get => direction;
        set => direction = value.normalized;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.velocity = direction * speed;
    }

    private void FixedUpdate()
    {
        rb.velocity = direction * speed;
    }

    // ✅ Ball tells the OTHER object "you got hit"
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ICollidable other = collision.gameObject.GetComponent<ICollidable>();
        if (other != null)
        {
            other.OnHit(collision);
        }

        // ✅ Ball also responds to the hit (direction reversal)
        OnHit(collision);
    }

    // ✅ Ball's collision response
    
    public void OnHit(Collision2D collision)
{
    // Use the contact normal to know which side we hit
    Vector2 n = collision.contacts[0].normal;

    // Hit a vertical surface (paddle OR left/right wall) → flip X
    if (Mathf.Abs(n.x) > 0.5f)
    {
        direction = new Vector2(-direction.x, direction.y).normalized;
    }

    // Hit a horizontal surface (top/bottom wall) → flip Y
    if (Mathf.Abs(n.y) > 0.5f)
    {
        direction = new Vector2(direction.x, -direction.y).normalized;
    }
}

    public void ResetBall()
    {
        transform.position = Vector2.zero;
        direction = new Vector2(1f, 1f).normalized;
        rb.velocity = direction * speed;
    }
}