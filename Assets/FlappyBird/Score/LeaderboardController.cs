using System;
using UnityEngine;
using LootLocker.Requests;
using System.Collections.Generic;

public class LeaderboardController : MonoBehaviour
{
    public string leaderboardKey;

    private void Start()
    {
        StartSession(ManageGuestSessionStart);
    }
    public void StartSession(Action<LootLockerGuestSessionResponse> action)
    {
        LootLockerSDKManager.StartGuestSession(action);
    }
    private void ManageGuestSessionStart(LootLockerGuestSessionResponse response)
    {
        if (response.success) Debug.Log("succes");
        else Debug.Log("failed");
    }
    public void GetScores(int maxScoreAmount, Action<LootLockerGetScoreListResponse> action)
    {
        LootLockerSDKManager.GetScoreList(leaderboardKey, maxScoreAmount, action);
    }
    public void SubmitScore(string playerName, int playerScore)
    {
        LootLockerSDKManager.SubmitScore(playerName, playerScore, leaderboardKey, ManageSubmitScore);
    }
    private void ManageSubmitScore(LootLockerSubmitScoreResponse response)
    {
        if (response.success) Debug.Log("Succes");
        else Debug.Log("failed");
    }
}
