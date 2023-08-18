using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float animationSpeed = 0.7f;

    private void Start()
    {
        GameEventController.OnGameRestart += OnGameRestart;
        GameEventController.OnGameOver += OnGameOver;
    }
    private void OnGameRestart()
    {
        animator.speed = animationSpeed;
    }
    private void OnGameOver()
    {
        animator.speed = 0f;
    }
}
