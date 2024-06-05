using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform wayptParent;

    private NavMeshAgent _enemyAgent;
    private List<Transform> _waypts;

    private AISensor _enemyAISensor;

    private int _destIndex;

    private Vector3 _destination;
    
    private Animator _animator;
    
    private NavMeshAgent EnemyAgent => _enemyAgent ??= GetComponent<NavMeshAgent>();
    
    private AnimationManager _enemyAIAnimationManager;

    private readonly int _inputX = Animator.StringToHash("InputX");
    private readonly int _inputY = Animator.StringToHash("InputY");
    
    private readonly int _movementWalkForward = Animator.StringToHash("Movement_Walk_Fwd");
    private readonly int _isWalkingBool = Animator.StringToHash("isWalking");

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
        EnemyAgent.SetDestination(GetDestination());
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 destination = GetDestination();
        Vector3 direction = (destination - transform.position).normalized;
        EnemyAgent.SetDestination(destination);
        Debug.Log("enemy ai : "+_enemyAgent.speed+" acc- "+_enemyAgent.acceleration+" Vel - "+_enemyAgent.velocity+" stopped "+EnemyAgent.isStopped+ " direction - "+direction);
        //UpdateAnimationParams(direction.x, direction.z);

    }
    /*private void UpdateAnimationParams(float x, float y)
    {
        enemyAIAnimationController.SetFloat(_inputX, x);
        enemyAIAnimationController.SetFloat(_inputY, y);
    }*/
    private Vector3 GetDestination()
    {
        if(_enemyAISensor.ObjectsInFOV.Count > 0)
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
if(_waypts != null && _waypts.Count>0)
        foreach (var waypt in _waypts)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(waypt.position, .25f);
        }
        
    }
}