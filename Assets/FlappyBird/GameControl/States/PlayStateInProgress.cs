using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayStateInProgress : PlayState
{
    private void Awake()
    {
        PlayerController.OnPlayerDie += TriggerExitTransitionEvent;
    }
}
