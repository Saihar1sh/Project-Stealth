using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform wayptParent;

    private NavMeshAgent enemyAgent;
    private List<Transform> waypts;

    private AISensor enemyAISensor;

    private int destIndex;

    private Vector3 destination;
    
    private Animator _animator;
    
    private NavMeshAgent EnemyAgent => enemyAgent ??= GetComponent<NavMeshAgent>();
    
    private AnimationController enemyAIAnimationController;

    private readonly int _inputX = Animator.StringToHash("InputX");
    private readonly int _inputY = Animator.StringToHash("InputY");
    
    private readonly int _Movement_Walk_Forward = Animator.StringToHash("Movement_Walk_Fwd");

    private void Awake()
    {
        enemyAISensor = GetComponent<AISensor>();
        _animator = GetComponent<Animator>();
        enemyAIAnimationController = new AnimationController(_animator);
        waypts = wayptParent.GetComponentsInChildren<Transform>(true).ToList();
        waypts.Remove(wayptParent);
    }

    // Start is called before the first frame update
    void Start()
    {
        destIndex = 0;
        enemyAgent.SetDestination(GetDestination());
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 destination = GetDestination();
        Vector3 direction = (destination - transform.position).normalized;
        enemyAgent.SetDestination(destination);
        //UpdateAnimationParams(direction.x, direction.z);

    }
    /*private void UpdateAnimationParams(float x, float y)
    {
        enemyAIAnimationController.SetFloat(_inputX, x);
        enemyAIAnimationController.SetFloat(_inputY, y);
    }*/
    private Vector3 GetDestination()
    {
        if(enemyAISensor.ObjectsInFOV.Count > 0)
        {
            return destination = enemyAISensor.ObjectsInFOV[0].transform.position;
        }
        
        if (enemyAgent.hasPath)
        {
            return destination;
        }

        destIndex = (++destIndex) % waypts.Count;
        return destination = waypts[destIndex].position;
    }

    private void OnDrawGizmos()
    {
        if (EnemyAgent.hasPath)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, enemyAgent.destination);
        }
if(waypts != null && waypts.Count>0)
        foreach (var waypt in waypts)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(waypt.position, .25f);
        }
        
    }
}