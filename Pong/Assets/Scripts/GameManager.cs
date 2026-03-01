using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using TMPro;

public class GameManager : NetworkBehaviour
{
    public NetworkVariable<int> leftScore = new NetworkVariable<int>(
        0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public NetworkVariable<int> rightScore = new NetworkVariable<int>(
        0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public NetworkVariable<bool> gameOver = new NetworkVariable<bool>(
        false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public NetworkVariable<bool> gameStarted = new NetworkVariable<bool>(
        false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [Header("Win Condition")]
    [SerializeField] private int winScore = 5;

    [Header("References")]
    [SerializeField] private BallMovement ball;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI messageText;

    public override void OnNetworkSpawn()
    {
        leftScore.OnValueChanged += (_, __) => UpdateUI();
        rightScore.OnValueChanged += (_, __) => UpdateUI();
        gameOver.OnValueChanged += (_, __) => UpdateUI();
        gameStarted.OnValueChanged += (_, __) => UpdateUI();

        if (IsServer)
        {
            leftScore.Value = 0;
            rightScore.Value = 0;
            gameOver.Value = false;
            gameStarted.Value = false;
        }

        UpdateUI();
    }

    public void ScoreLeftPoint()
    {
        if (!IsServer) return;
        if (gameOver.Value) return;
        if (!gameStarted.Value) return;

        leftScore.Value++;

        if (ball != null) ball.ResetAndLaunchServerRpc(Vector2.right);

        CheckWin();
        UpdateUI();
    }

    public void ScoreRightPoint()
    {
        if (!IsServer) return;
        if (gameOver.Value) return;
        if (!gameStarted.Value) return;   // ✅ you were missing this line

        rightScore.Value++;

        if (ball != null) ball.ResetAndLaunchServerRpc(Vector2.left);

        CheckWin();
        UpdateUI();
    }

    private void CheckWin()
    {
        if (leftScore.Value >= winScore)
        {
            gameOver.Value = true;
            if (ball != null) ball.StopBallServerRpc();
        }
        else if (rightScore.Value >= winScore)
        {
            gameOver.Value = true;
            if (ball != null) ball.StopBallServerRpc();
        }
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = $"{leftScore.Value} - {rightScore.Value}";

        if (messageText != null)
        {
            if (!gameOver.Value)
            {
                messageText.text = "";
            }
            else
            {
                if (leftScore.Value >= winScore) messageText.text = "LEFT PLAYER WINS!";
                else if (rightScore.Value >= winScore) messageText.text = "RIGHT PLAYER WINS!";
                else messageText.text = "GAME OVER";
            }
        }
    }

    // ✅ This is what the UI button calls (NOT an RPC)
    public void StartGameButton()
    {
        Debug.Log("StartGameButton clicked");
        StartGameServerRpc(); // call the RPC (host will run it as server)
    }

    // ✅ This is the actual RPC (name ends with ServerRpc)
    [ServerRpc(RequireOwnership = false)]
    public void StartGameServerRpc()
    {
        if (!IsServer) return;
        if (gameOver.Value) return;

        leftScore.Value = 0;
        rightScore.Value = 0;
        gameOver.Value = false;
        gameStarted.Value = true;

        if (ball != null)
            ball.ResetAndLaunchServerRpc(Vector2.right);

        UpdateUI();
    }

    [ServerRpc(RequireOwnership = false)]
    public void RestartGameServerRpc()
    {
        if (!IsServer) return;

        leftScore.Value = 0;
        rightScore.Value = 0;
        gameOver.Value = false;
        gameStarted.Value = false; // optional: makes it require Start again

        if (ball != null)
            ball.ResetAndLaunchServerRpc(Vector2.right);

        UpdateUI();
    }
}
