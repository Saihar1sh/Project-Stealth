using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class PatrolState : BaseState<EnemyStateMachine.EnemyStates>
{
    private EnemyStateMachine _enemyStateMachine;

    private NavMeshAgent _enemyAgent;
    private AnimationManager _enemyAnimationManager;
    private AISensor _enemyAISensor;

    private List<Transform> _waypts;

    private int _destIndex;

    private Vector3 _destination;

    private bool isTurning = false;


    private readonly int _inputX = Animator.StringToHash("InputX");
    private readonly int _inputY = Animator.StringToHash("InputY");

    private readonly int _movementWalkForward = Animator.StringToHash("Movement_Walk_Fwd");
    private readonly int _isWalkingBool = Animator.StringToHash("isWalking");

    private const float anglePerSecond = 60f;

    private const float turnAngleThreshold = 0.1f;

    #region State abstract functions

    public PatrolState(EnemyStateMachine stateMachine) : base(EnemyStateMachine.EnemyStates.Patrol)
    {
        StateSetup(stateMachine);
    }

    public override void OnEnter()
    {
        _destIndex = 0;
        Vector3 destination = GetDestination();
        Vector3 direction = GetEnemyDirection(destination);
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        _enemyAgent.transform.rotation = Quaternion.RotateTowards(_enemyAgent.transform.rotation, targetRotation,
            anglePerSecond * Time.deltaTime);
        _enemyAgent.SetDestination(destination);
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        Patrol();
    }

    public override EnemyStateMachine.EnemyStates GetNextState()
    {
        return EnemyStateMachine.EnemyStates.Alert;
    }

    public override void OnTriggerEnter(Collider other)
    {
    }

    public override void OnTriggerStay(Collider other)
    {
    }

    public override void OnTriggerExit(Collider other)
    {
    }

    #endregion

    private void StateSetup(EnemyStateMachine stateMachine)
    {
        _enemyStateMachine = stateMachine;
        var wayptParent = stateMachine.wayptParent;
        _enemyAgent = _enemyStateMachine.EnemyAgent;
        _enemyAISensor = _enemyStateMachine.EnemyAISensor;
        _enemyAnimationManager = _enemyStateMachine.EnemyAIAnimationManager;
        _waypts = wayptParent.GetComponentsInChildren<Transform>(true).ToList();
        _waypts.Remove(wayptParent);
    }

    private void Patrol()
    {
        if (_waypts == null || _waypts.Count == 0 || isTurning && !_enemyAgent.hasPath)
        {
            UpdateMovementAnimation(false);
            return;
        }

        UpdateMovementAnimation(true);
        TurnTowardsDirection();
        Logger.Debug("enemy ai : " + _enemyAgent.speed + " acc- " + _enemyAgent.acceleration + " Vel - " +
                  _enemyAgent.velocity + " stopped " + _enemyAgent.isStopped);
    }

    private async void TurnTowardsDirection()
    {
        Vector3 destination = GetDestination();
        Vector3 direction = GetEnemyDirection(destination);
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Logger.Debug("This target rot: " + targetRotation + " current rot: " + _enemyAgent.transform.rotation);
        isTurning = true;
        while (CheckIfTotallyTurned(targetRotation))
        {
            _enemyAgent.transform.rotation = Quaternion.RotateTowards(_enemyAgent.transform.rotation, targetRotation,
                anglePerSecond * Time.deltaTime);
            await Task.Yield();
        }

        isTurning = false;
        _enemyAgent.SetDestination(destination);
    }

    #region Utils

    private void UpdateMovementAnimation(bool isWalking)
    {
        _enemyAnimationManager.SetBool(_isWalkingBool, isWalking);
    }

    private bool CheckIfTotallyTurned(Quaternion targetRotation) =>
        Quaternion.Angle(targetRotation, _enemyAgent.transform.rotation) > turnAngleThreshold;

    private Vector3 GetEnemyDirection(Vector3 destination)
    {
        Vector3 direction = (destination - _enemyAgent.transform.position).normalized;
        direction.y = 0;
        return direction;
    }

    private void UpdateAnimationParams(float x, float y)
    {
        _enemyAnimationManager.SetFloat(_inputX, x);
        _enemyAnimationManager.SetFloat(_inputY, y);
    }

    private Vector3 GetDestination()
    {
        if (_enemyAISensor.ObjectsInFOV.Count > 0)
        {
            return _destination = _enemyAISensor.ObjectsInFOV[0].transform.position;
        }

        if (_enemyAgent.hasPath)
        {
            return _destination;
        }

        _destIndex = (++_destIndex) % _waypts.Count;
        return _destination = _waypts[_destIndex].position;
    }

//For Unity Editor
    public override void OnDrawGizmos()
    {
        if (_enemyAgent.hasPath)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(_enemyAgent.transform.position, _enemyAgent.destination);
        }

        if (_waypts != null && _waypts.Count > 0)
            foreach (var waypt in _waypts)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(waypt.position, .25f);
            }
    }

    #endregion
}