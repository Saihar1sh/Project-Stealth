using UnityEngine;

public class AttackState : BaseState<EnemyStateMachine.EnemyStates>
{
    private EnemyStateMachine _enemyStateMachine;
    public AttackState(EnemyStateMachine stateMachine) : base(EnemyStateMachine.EnemyStates.Attack)
    {
        _enemyStateMachine = stateMachine;
    }

    public override void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public override void OnExit()
    {
        throw new System.NotImplementedException();
    }

    public override void OnUpdate()
    {
        throw new System.NotImplementedException();
    }

    public override EnemyStateMachine.EnemyStates GetNextState()
    {
        throw new System.NotImplementedException();
    }

    public override void OnTriggerEnter(Collider other)
    {
        throw new System.NotImplementedException();
    }

    public override void OnTriggerStay(Collider other)
    {
        throw new System.NotImplementedException();
    }

    public override void OnTriggerExit(Collider other)
    {
        throw new System.NotImplementedException();
    }

    public override void OnDrawGizmos()
    {
        
    }
}
