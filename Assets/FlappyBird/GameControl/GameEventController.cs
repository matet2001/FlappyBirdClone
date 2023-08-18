using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameEventController : MonoBehaviour
{
    public static event Action OnGameStart, OnGameOver, OnGameRestart;
    [SerializeField] PlayState readyToPlay, inProgress, gameOver;

    private void Awake()
    {
        readyToPlay.OnExitAction += delegate { InvokeOnEvent(OnGameStart); };
        inProgress.OnExitAction += delegate { InvokeOnEvent(OnGameOver); };
        gameOver.OnExitAction += delegate { InvokeOnEvent(OnGameRestart); };
    }
    private void InvokeOnEvent(Action action)
    {
        action.Invoke();
    }
}