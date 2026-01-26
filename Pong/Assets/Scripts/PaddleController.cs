using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PaddleController : MonoBehaviour
{
    [SerializeField] protected float speed = 5f;
    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate()
    {
        float input = Input.GetAxis(GetInputAxis());
        rb.velocity = new Vector2(0f, input * speed);
    }

    protected abstract string GetInputAxis();
}