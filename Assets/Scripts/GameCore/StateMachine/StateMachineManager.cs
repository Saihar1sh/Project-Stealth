using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachineManager<EState> : MonoBehaviour where EState : System.Enum
{
    protected SerializableDictionary<EState, BaseState<EState>> States = new SerializableDictionary<EState, BaseState<EState>>();

    protected BaseState<EState> CurrentState;

    [field: SerializeField] public EState StateName => CurrentState.StateName;
    
    private bool isTransitioningState = false;

    private void Start()
    {
        CurrentState.OnEnter();
    }

    private void Update()
    {
        if (isTransitioningState)
            return;
        EState nextStateName = CurrentState.GetNextState();
        if (!nextStateName.Equals(CurrentState.StateName))
        {
            CurrentState.OnUpdate();
        }
        else
        {
            TransitionToState(nextStateName);
        }
    }

    private void TransitionToState(EState nextStateName)
    {
        isTransitioningState = true;
        CurrentState.OnExit();
        CurrentState = States[nextStateName];
        CurrentState.OnEnter();
        isTransitioningState = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        CurrentState.OnTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        CurrentState.OnTriggerExit(other);
    }

    private void OnTriggerStay(Collider other)
    {
        CurrentState.OnTriggerStay(other);
    }

    private void OnDrawGizmos()
    {
        CurrentState.OnDrawGizmos();
    }
}