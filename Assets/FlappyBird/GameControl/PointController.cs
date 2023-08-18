using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Code by Mate
public class PointController : MonoBehaviour
{
    public static int points;
    public static event Action<int> OnSetPoint;

    private void Awake()
    {
        GameEventController.OnGameRestart += GameEventController_OnGameRestart;
        PlayerController.OnPlayerCollectPoint += AddPoint;      
    }
    private void GameEventController_OnGameRestart()
    {
        points = 0;
        OnSetPoint?.Invoke(points);
    }
    public void AddPoint()
    {
        points++;
        OnSetPoint?.Invoke(points);
    }
}
