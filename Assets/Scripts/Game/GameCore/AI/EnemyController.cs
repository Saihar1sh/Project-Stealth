using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform wayptParent;

    private NavMeshAgent _enemyAgent;
    private List<Transform> _waypts;

    private AISensor _enemyAISensor;

    private int _destIndex;

    private Vector3 _destination;

    private Animator _animator;

    private bool isTurning = false;

    private NavMeshAgent EnemyAgent => _enemyAgent ??= GetComponent<NavMeshAgent>();

    private AnimationManager _enemyAIAnimationManager;

    private readonly int _inputX = Animator.StringToHash("InputX");
    private readonly int _inputY = Animator.StringToHash("InputY");

    private readonly int _movementWalkForward = Animator.StringToHash("Movement_Walk_Fwd");
    private readonly int _isWalkingBool = Animator.StringToHash("isWalking");

    [SerializeField] private float anglePerSecond = 10f;

    [SerializeField] private float turnAngleThreshold = 0.1f;

    private void Awake()
    {
        _enemyAISensor = GetComponent<AISensor>();
        _animator = GetComponent<Animator>();
        _enemyAIAnimationManager = new AnimationManager(_animator);
        _waypts = wayptParent.GetComponentsInChildren<Transform>(true).ToList();
        _waypts.Remove(wayptParent);
    }

    // Start is called before the first frame update
    void Start()
    {
        _destIndex = 0;
        Vector3 destination = GetDestination();
        Vector3 direction = GetEnemyDirection(destination);
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        _enemyAgent.transform.rotation = Quaternion.RotateTowards(_enemyAgent.transform.rotation, targetRotation, anglePerSecond * Time.deltaTime);
        EnemyAgent.SetDestination(destination);
    }

    private Vector3 GetEnemyDirection(Vector3 destination)
    {
        Vector3 direction = (destination - _enemyAgent.transform.position).normalized;
        direction.y = 0;
        return direction;
    }

    // Update is called once per frame
    void Update()
    {
        Patrol();
        //UpdateAnimationParams(direction.x, direction.z);
    }

    private void Patrol()
    {
        if (_waypts == null || _waypts.Count == 0 || isTurning && !_enemyAgent.hasPath)
        {
            UpdateMovementAnimation(false);
            return;
        }

        var pos = transform.position.normalized;
        UpdateMovementAnimation(true);
        TurnTowardsDirection();
        Debug.Log("enemy ai : " + _enemyAgent.speed + " acc- " + _enemyAgent.acceleration + " Vel - " +
                  _enemyAgent.velocity + " stopped " + EnemyAgent.isStopped);
    }

    private void UpdateMovementAnimation(bool isWalking)
    {
        _enemyAIAnimationManager.SetBool(_isWalkingBool, isWalking);
    }

    private async void TurnTowardsDirection()
    {
        Vector3 destination = GetDestination();
        Vector3 direction = GetEnemyDirection(destination);
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Debug.Log("This target rot: " + targetRotation + " current rot: " + _enemyAgent.transform.rotation);
        isTurning = true;
        while (CheckIfTotallyTurned(targetRotation))
        {
            _enemyAgent.transform.rotation = Quaternion.RotateTowards(_enemyAgent.transform.rotation, targetRotation,
                anglePerSecond * Time.deltaTime);
            await Task.Yield();
        }

        isTurning = false;
        EnemyAgent.SetDestination(destination);
    }

    private bool CheckIfTotallyTurned(Quaternion targetRotation) => Quaternion.Angle(targetRotation, _enemyAgent.transform.rotation) > turnAngleThreshold;


    private void UpdateAnimationParams(float x, float y)
    {
        _enemyAIAnimationManager.SetFloat(_inputX, x);
        _enemyAIAnimationManager.SetFloat(_inputY, y);
    }

    private Vector3 GetDestination()
    {
        if (_enemyAISensor.ObjectsInFOV.Count > 0)
        {
            return _destination = _enemyAISensor.ObjectsInFOV[0].transform.position;
        }

        if (EnemyAgent.hasPath)
        {
            return _destination;
        }

        _destIndex = (++_destIndex) % _waypts.Count;
        return _destination = _waypts[_destIndex].position;
    }

    private void OnDrawGizmos()
    {
        if (EnemyAgent.hasPath)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, _enemyAgent.destination);
        }

        if (_waypts != null && _waypts.Count > 0)
            foreach (var waypt in _waypts)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(waypt.position, .25f);
            }
    }
}