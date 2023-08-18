using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Code by Mate
public class ScoreController : MonoBehaviour
{
    public static event Action<int, int, int> OnSetScore;

    [SerializeField] int[] scorePerMedals;

    private void Awake()
    {
        GameEventController.OnGameOver += OnGameOver;
    }
    private void OnGameOver()
    {
        UpdateScorePanel(PointController.points);
    }
    public void UpdateScorePanel(int points)
    {
        int currentBestScore = PlayerPrefs.GetInt("bestScore");
        bool isMadeToBestScore = IsMadeToBestScore(points, currentBestScore);

        int newBestScore = (isMadeToBestScore) ? points : currentBestScore;
        SaveBestScore(newBestScore);

        int medalNumb = ChooseMedalImage(points);
        OnSetScore?.Invoke(points, medalNumb, newBestScore);
    }
    private bool IsMadeToBestScore(int points, int currentBestScore)
    {
        //New best score
        if (points > currentBestScore)
            return true;

        return false;
    }
    private static void SaveBestScore(int points)
    {
        PlayerPrefs.SetInt("bestScore", points);
        PlayerPrefs.Save();
    }
    private int ChooseMedalImage(int points)
    {
        if (points < scorePerMedals[0])
            return -1;

        for (int i = 0; i < scorePerMedals.Length; i++)
        {
            if (i == scorePerMedals.Length - 1) return i;

            if (points >= scorePerMedals[i] && points < scorePerMedals[i + 1])
                return i;
        }
        return -1;
    }
}
