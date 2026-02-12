using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PaddleController : MonoBehaviour, ICollidable
{
    [SerializeField] protected float speed = 5f;
    protected Rigidbody2D rb;
    private SpriteRenderer sr;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    protected virtual void FixedUpdate()
    {
        float input = GetMovementInput();
        rb.velocity = new Vector2(0f, input * speed);
    }

    // ✅ derived classes MUST implement
    protected abstract float GetMovementInput();

    // ✅ Paddle collision response (optional visual feedback)
    public void OnHit(Collision2D collision)
    {
        // Only react if the BALL hit us
        if (collision.gameObject.GetComponent<BallMovement>() != null)
        {
            if (sr != null)
                StartCoroutine(Flash());
        }
    }

    private IEnumerator Flash()
    {
        Color original = sr.color;
        sr.color = Color.gray;
        yield return new WaitForSeconds(0.08f);
        sr.color = original;
    }
}