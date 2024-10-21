using System;
using UnityEngine;

[System.Serializable]
public abstract class BaseState<EState> where EState : Enum
{
    protected BaseState(EState stateName)
    {
        StateName = stateName;
    }

    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void OnUpdate();

    public EState StateName { get; private set; }

    public abstract EState GetNextState();

    public abstract void OnTriggerEnter(Collider other);
    public abstract void OnTriggerStay(Collider other);
    public abstract void OnTriggerExit(Collider other);

    
    public abstract void OnDrawGizmos();
}
