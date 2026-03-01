using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    [SerializeField] private bool isLeftZone; // true=left zone, false=right zone
    private GameManager gm;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only the server/host should award points
        if (!NetworkManager.Singleton.IsServer) return;

        // Only score when the BALL enters
        if (other.GetComponent<BallMovement>() == null) return;

        if (gm == null) return;

        // If ball hits LEFT zone, RIGHT player scored
        if (isLeftZone)
            gm.ScoreRightPoint();
        else
            gm.ScoreLeftPoint();
    }
}
