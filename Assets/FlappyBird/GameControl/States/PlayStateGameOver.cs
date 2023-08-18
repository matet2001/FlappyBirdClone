using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayStateGameOver : PlayState
{
    public override void OnEnter()
    {
        
    }
    public void OnClickRestart()
    {
        if (!CanvasAnimationManager.IsFinishedAnimating(stateUI)) return;
        
        TriggerExitTransitionEvent();
    }
}