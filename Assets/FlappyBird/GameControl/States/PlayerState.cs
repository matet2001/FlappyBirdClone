using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Code by Mate
public abstract class PlayState : StateBase
{
    [SerializeField] protected GameObject stateUI;

    public override void OnEnter()
    {
        base.OnEnter();
        OpenStateUI();
    }
    public override void OnExit()
    {
        base.OnEnter();
        CloseStateUI();
    }
    public void OpenStateUI()
    {
        //Debug.Log("Try to open " + stateUI.name);
        CanvasAnimationManager.Open(stateUI);
    }
    public void CloseStateUI()
    {
        //Debug.Log("Try to close " + stateUI.name);
        CanvasAnimationManager.Close(stateUI);
    }
    public bool IsStateUIFinishedAnimating() => CanvasAnimationManager.IsFinishedAnimating(stateUI);
}