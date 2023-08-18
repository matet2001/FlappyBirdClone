using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBasedStateMachine : MonoBehaviour
{
    public StateBase currentState;

    public void Start()
    {
        currentState.OnEnter();
        SetUpEventTransitions();
    }
    private void SetUpEventTransitions()
    {
        foreach (StateBase state in GetComponentsInChildren<StateBase>())
            state.OnExitToState += TransitionEventToExitState;       
    }
    public void Update()
    {
        currentState.OnUpdate();
    }
    private void TransitionEventToExitState(StateBase triggeredState)
    {
        TryChangeState(triggeredState);
    }
    public void TryChangeState(StateBase state)
    {
        Debug.Log("Try to change state to " + state.name + " from " + currentState.name);
        if (state == currentState) return;

        ChangeState(state);
    }
    public void ChangeState(StateBase state)
    {
        currentState.OnExit();
        currentState = state;
        currentState.OnEnter();
    }
}

public abstract class StateBase : MonoBehaviour
{
    public StateBase exitState;
    public event Action<StateBase> OnExitToState;

    public event Action OnExitAction;

    public virtual void OnEnter() { }
    public virtual void OnUpdate() { }
    public virtual void OnExit() { }

    public void TriggerExitTransitionEvent()
    {
        if (exitState == null) return;

        OnExitToState?.Invoke(exitState);
        OnExitAction?.Invoke();
    }
}

