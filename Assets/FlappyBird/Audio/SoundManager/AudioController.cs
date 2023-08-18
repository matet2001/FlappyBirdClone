using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] SoundManager soundManager;
    
    private void Awake()
    {
        PlayerController.OnPlayerDie += OnPlayerDie;   
        PlayerController.OnJump += PlayerController_OnJump;
        PointController.OnSetPoint += OnSetPoint;
    }
    private void OnPlayerDie()
    {
        soundManager.PlaySound("Hit");
        soundManager.PlaySound("Fall");
    }
    private void PlayerController_OnJump()
    {
        soundManager.PlaySound("Jump");
    }
    private void OnSetPoint(int point)
    {
        soundManager.PlaySound("GetPoint");
    }
}
