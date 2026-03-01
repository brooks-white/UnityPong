using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerPaddleController : NetworkBehaviour
{
    [SerializeField] private float speed = 5f;

    private Rigidbody2D rb;
    private GameManager gm;

    // Owner writes, everyone reads
    private NetworkVariable<float> syncedY =
        new NetworkVariable<float>(
            0f,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner
        );

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gm = FindObjectOfType<GameManager>();
    }

   public override void OnNetworkSpawn()
{
    if (rb == null) rb = GetComponent<Rigidbody2D>();

    // Everyone sets the X based on who owns this paddle
    float x = (OwnerClientId == 0) ? -8f : 8f;
    rb.position = new Vector2(x, rb.position.y);

    // Only owner writes the network variable
    if (IsOwner)
    {
        syncedY.Value = rb.position.y;
    }
}

    private void FixedUpdate()
    {
        if (rb == null) return;

        // ✅ stop movement if game is over
        if (gm != null && gm.gameOver.Value) 
        {
            if (IsOwner) rb.velocity = Vector2.zero;
            return;
        }

        if (IsOwner)
        {
            float input = (OwnerClientId == 0)
                ? Input.GetAxis("LeftPaddle")
                : Input.GetAxis("RightPaddle");

            Vector2 pos = rb.position;
            pos.y += input * speed * Time.fixedDeltaTime;

            rb.MovePosition(pos);

            // Owner writes
            syncedY.Value = pos.y;
        }
        else
        {
            // Non-owner reads
            Vector2 pos = rb.position;
            pos.y = syncedY.Value;

            rb.MovePosition(pos);
        }
    }
}
