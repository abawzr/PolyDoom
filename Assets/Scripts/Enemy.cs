using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private List<Transform> patrolPoints;
    [SerializeField] private SwordHitbox swordHitbox;
    [SerializeField] private float playerDetectRange;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackCooldown;


    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private int _currentPatrolPoint = 0;
    private bool _isChasing;
    private float _currentAttackCooldown;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        _currentAttackCooldown = attackCooldown;
    }

    private void Start()
    {
        _navMeshAgent.SetDestination(patrolPoints[0].position);
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        _currentAttackCooldown -= Time.deltaTime;

        if (distance <= playerDetectRange)
        {
            _isChasing = true;
            _animator.SetBool("Detect", true);
        }
        else if (distance > playerDetectRange)
        {
            _isChasing = false;
            _animator.SetBool("Detect", false);
        }

        if (_isChasing)
        {
            _navMeshAgent.SetDestination(playerTransform.position);
        }

        if (distance <= attackRange && _currentAttackCooldown <= 0f)
        {
            _animator.SetTrigger("Attack");
            _currentAttackCooldown = attackCooldown;
        }

        else
        {
            if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance < 0.25f)
            {
                NextPoint();
            }
        }
    }

    private void NextPoint()
    {
        if (patrolPoints.Count == 0) return;

        while (true)
        {
            int tempIndex = Random.Range(0, patrolPoints.Count);

            if (tempIndex != _currentPatrolPoint)
            {
                _currentPatrolPoint = tempIndex;
                break;
            }
        }

        _navMeshAgent.SetDestination(patrolPoints[_currentPatrolPoint].position);
    }

    public void EnableSwordHitbox()
    {
        swordHitbox.CanDamage = true;
    }

    public void DisableSwordHitbox()
    {
        swordHitbox.CanDamage = false;
    }
}
