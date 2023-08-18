using LootLocker.Requests;
using System;
using System.Collections.Generic;
using UnityEngine;

//Code by Mate
public class HighScoreController : MonoBehaviour
{
    [SerializeField] HighScoreUI highScoreUI;
    [SerializeField] LeaderboardController leaderboardController;
    [SerializeField] PlayStateGameOver gameOverState;

    private List<RecordStruct> highScoresList;
    private int placing = -1;
    private string playerName;
    private bool isNameSetted;
    private bool isRecordAlreadyAdded;

    private bool isLeaderboardConnected;

    private void Awake()
    {
        GameEventController.OnGameOver += OnGameOver;
    }
    //Execute when entering game over state
    private void OnGameOver()
    {
        //Is there connection?/Get score list
        GetScores();
    }
    private void GetScores()
    {
        int scoreLength = highScoreUI.GetHighScoreElementListLength();
        leaderboardController.GetScores(scoreLength, HandleGetScores);
    }
    private void HandleGetScores(LootLockerGetScoreListResponse response)
    {
        isLeaderboardConnected = response.success;
        isRecordAlreadyAdded = false;

        //Is there internet?
        //If no, open state ui
        if (!response.success)
        {
            Debug.Log("failed");
            gameOverState.OpenStateUI();
            return;
        }

        //Is made to highscores
        GetRecords(out highScoresList, response);
        IsMadeToHighScore(out placing, PointController.points, highScoresList);

        //if no, open state ui
        if (placing == -1)
        {
            gameOverState.OpenStateUI();
            return;
        }

        //Is name setted?
        //if no, open name set ui
        if (!isNameSetted)
        {
            highScoreUI.OpenSetNamePanel();
            return;
        }

        //if yes, update, and open highscores
        SaveRecordAndOpenHighScores();
    }
    private static void GetRecords(out List<RecordStruct> highScoreList, LootLockerGetScoreListResponse response)
    {
        highScoreList = new List<RecordStruct>();
        LootLockerLeaderboardMember[] scores = response.items;

        foreach (LootLockerLeaderboardMember score in scores)
        {
            int placing = Array.IndexOf(scores, score);
            RecordStruct recordStruct = new RecordStruct(score.member_id, score.score, placing);

            highScoreList.Add(recordStruct);
        }
    }
    private void IsMadeToHighScore(out int placing, int points, List<RecordStruct> highScoreList)
    {
        placing = 0;
        
        //Point is 0, can't make to highscores
        if (points == 0)
        {
            placing = -1;
            return;
        }

        foreach (RecordStruct record in highScoreList)
        {
            if (points > record.playerPoint)
            {
                return;
            }
            placing++;
        }
        
        //Didn't made to highscores
        placing = -1;
        return;
    }
    private void SaveNewRecord(out List<RecordStruct> highScoreList, List<RecordStruct> recordStructs, string name, int points, int placing)
    {
        RecordStruct recordStruct = new RecordStruct(name, points, placing);
        leaderboardController.SubmitScore(name, points);
        recordStructs.Insert(placing, recordStruct);
        highScoreList = recordStructs;
        isRecordAlreadyAdded = true;
    }
    private void SaveRecordAndOpenHighScores()
    {
        SaveNewRecord(out List<RecordStruct> highScoreList, highScoresList, playerName, PointController.points, placing);
        highScoreUI.UpdateHighScoreUI(highScoreList, playerName);
        highScoreUI.OpenHighScorePanel();
    }
    
    public void TryToCloseSetNamePanel()
    {
        if (!highScoreUI.TryToCloseSetNamePanel()) return;
        
        playerName = highScoreUI.GetNameInputFieldText() + "$" + SystemInfo.deviceUniqueIdentifier;
        isNameSetted = true;
        SaveRecordAndOpenHighScores();
    }
    public void TryToOpenHighScores()
    {
        if (!gameOverState.IsStateUIFinishedAnimating()) return;

        gameOverState.CloseStateUI();

        if (!isLeaderboardConnected)
        {
            highScoreUI.OpenNoInternetPanel();         
            return;
        }
                
        highScoreUI.UpdateHighScoreUI(highScoresList, playerName);
        highScoreUI.OpenHighScorePanel();
    }
    public void TryToCloseHighScores()
    {
        if (!highScoreUI.TryToCloseHighScorePanel()) return;
        gameOverState.OpenStateUI();
    }
    
    public void TryToCloseNoInternetPanel()
    {
        if(!highScoreUI.TryToCloseNoInternetPanel()) return;
        TryToRestoreConnection();
    }
    private void TryToRestoreConnection()
    {
        //Try to start new session
        leaderboardController.StartSession(HandleSessionRestore);
    }
    private void HandleSessionRestore(LootLockerGuestSessionResponse sessionResponse)
    {
        if (!sessionResponse.success)
        {
            gameOverState.OpenStateUI();
            return;
        }

        int scoreLength = highScoreUI.GetHighScoreElementListLength();
        leaderboardController.GetScores(scoreLength, HandleScoreGetRestore);
    }
    private void HandleScoreGetRestore(LootLockerGetScoreListResponse scoreResponse)
    {
        if (!scoreResponse.success)
        {
            gameOverState.OpenStateUI();
            return;
        }

        isLeaderboardConnected = scoreResponse.success;

        GetRecords(out highScoresList, scoreResponse);
        IsMadeToHighScore(out placing, PointController.points, highScoresList);

        if (placing == -1 || isRecordAlreadyAdded)
        {
            gameOverState.OpenStateUI();
            return;
        }

        //Is name setted?
        //if no, open name set ui
        if (!isNameSetted)
        {
            highScoreUI.OpenSetNamePanel();
            return;
        }

        SaveRecordAndOpenHighScores();
    } 
}
[Serializable]
public struct RecordStruct
{
    public string playerName;
    public int playerPoint;
    public int playerPlacing;

    public RecordStruct(string name, int point, int placing)
    {
        playerName = name;
        playerPoint = point;
        playerPlacing = placing;
    }
}
