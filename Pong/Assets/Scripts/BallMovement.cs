using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BallMovement : NetworkBehaviour
{
    [SerializeField] private float speed = 5f;

    private Vector2 direction = Vector2.zero;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void OnNetworkSpawn()
{
    if (IsServer)
    {
        rb.velocity = Vector2.zero;
        direction = Vector2.zero;
    }
}

    private void FixedUpdate()
{
    if (!IsServer || rb == null) return;
    if (direction == Vector2.zero) return;  // ✅ don't move until launched

    rb.velocity = direction.normalized * speed;
}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsServer) return;

        Vector2 n = collision.contacts[0].normal;

        // flip X if hit vertical surface
        if (Mathf.Abs(n.x) > 0.5f)
            direction = new Vector2(-direction.x, direction.y);

        // flip Y if hit horizontal surface
        if (Mathf.Abs(n.y) > 0.5f)
            direction = new Vector2(direction.x, -direction.y);
    }

    [ServerRpc(RequireOwnership = false)]
    public void LaunchBallServerRpc(Vector2 startDir)
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        transform.position = Vector2.zero;
        direction = startDir.normalized;

        // add a little Y so it doesn't go perfectly flat every time
        if (Mathf.Abs(direction.y) < 0.1f)
            direction = new Vector2(direction.x, 0.35f).normalized;

        rb.velocity = direction * speed;
    }

    [ServerRpc(RequireOwnership = false)]
    public void ResetAndLaunchServerRpc(Vector2 dirToward)
    {
        LaunchBallServerRpc(dirToward);
    }
    [ServerRpc(RequireOwnership = false)]
public void StopBallServerRpc()
{
    if (rb == null) rb = GetComponent<Rigidbody2D>();
    rb.velocity = Vector2.zero;
    direction = Vector2.zero;
}
}