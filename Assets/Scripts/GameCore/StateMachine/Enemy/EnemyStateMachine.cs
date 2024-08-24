using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachineManager<EnemyStateMachine.EnemyStates>
{
    public enum EnemyStates
    {
        Patrol,
        Alert,
        Attack,
        Death
    }

    [field: SerializeField] public Transform wayptParent { get; private set; }
    public PatrolState PatrolState;
    private NavMeshAgent _enemyAgent;

    private AISensor _enemyAISensor;
    private Animator _animator;
    

    public AnimationManager EnemyAIAnimationManager { get; private set; }

    public NavMeshAgent EnemyAgent => _enemyAgent ??= GetComponent<NavMeshAgent>();
    public AISensor EnemyAISensor => _enemyAISensor ??= GetComponent<AISensor>();
    public Animator Animator => _animator ??= GetComponent<Animator>();

    private void Awake()
    {
        EnemyAIAnimationManager = new AnimationManager(Animator);
        
        States.Add(EnemyStates.Patrol, new PatrolState(this));
        States.Add(EnemyStates.Alert, new AlertState(this));
        States.Add(EnemyStates.Attack, new AttackState(this));
        States.Add(EnemyStates.Death, new DeathState(this));

        CurrentState = States[EnemyStates.Patrol];
        PatrolState = (PatrolState)CurrentState;
    }
}