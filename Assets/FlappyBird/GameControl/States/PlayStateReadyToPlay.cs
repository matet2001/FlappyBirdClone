using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayStateReadyToPlay : PlayState
{        
    public override void OnUpdate()
    {
        if (!Input.GetMouseButtonDown(0) || !CanvasAnimationManager.IsFinishedAnimating(stateUI)) return;

        TriggerExitTransitionEvent();
    }
}