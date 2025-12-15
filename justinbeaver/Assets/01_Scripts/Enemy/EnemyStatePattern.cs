using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStatePattern : MonoBehaviour
{
    private IEnemyState currentState;

    public Transform player;
    public float detectRange;
    public float attackRange;
    public float moveSpeed;
    private Animator animator;
    public NavMeshAgent agent;
    public float attackCooldown;
    public int attackDamage;
    bool isAttacking = false;
    public float updateInterval;
    private float timeSinceLastUpdate;

    public float beaverSpottedTime;
    private float beaverSpottedLastUpdate;

    Vector3 alertTargetPos;

    public float MoveSpeed => moveSpeed;
    public float DetectRange => detectRange;
    public Transform PlayerTrf => player;
    public Animator Animator => animator;
    public float AttackRange => attackRange;
    public NavMeshAgent Agent => agent;
    public float UpdateInterval => updateInterval;
    public float TimeSinceLastUpdate => timeSinceLastUpdate;
    public float BeaverSpottedTime => beaverSpottedTime;
    public float BeaverSpottedLastUpdate => beaverSpottedLastUpdate;
    public Vector3 AlertTargetPos => alertTargetPos;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        SetState(new IdleState(this));
       
    }
    private void Update()
    {
        //if (GameManager.Instance.State != GameState.Playing) return;
        if (currentState == null) return;
        currentState.Update();


    }

    public void SetAlertTargetPos(Vector3 pos)
    {
        alertTargetPos = pos;
    }
    IEnumerator AttackDelay()
    {
        while (isAttacking)
        {
            Attack(attackDamage);
            yield return new WaitForSeconds(attackCooldown);
        }
    }

    public void StartAttackCooldown()
    {
        if (isAttacking) { return; }
        isAttacking = true;
        StartCoroutine("AttackDelay");
    }
    public void StopAttackCooldown()
    {
        if (!isAttacking) { return; }
        isAttacking = false;
        StopCoroutine("AttackDelay");
    }

    void Attack(int Damage)
    {
   
        //플레이어 데미지 처리
        animator.SetTrigger("isKicking");
    }

    public void Sleep()
    {
        animator.SetTrigger("isSleeping");
    }
 
    public Vector3 GetRandomPositionNavMesh()
    {
        Vector3 randomDirection = Random.insideUnitSphere * detectRange;
        randomDirection += transform.position;

        NavMeshHit navHit;
        if (NavMesh.SamplePosition(randomDirection, out navHit, detectRange, NavMesh.AllAreas))
        {
            return navHit.position;
        }
        else
        {
            return transform.position;
        }
    }

    public void SetState(IEnemyState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

}
