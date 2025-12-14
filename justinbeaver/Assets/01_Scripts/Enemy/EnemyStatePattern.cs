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
    public int attackCooldown;
    public int attackDamage;
    public bool isAttacking = false;

    public float MoveSpeed => moveSpeed;
    public float DetectRange => detectRange;
    public Transform PlayerTrf => player;
    public Animator Animator => animator;
    public float AttackRange => attackRange;
    public NavMeshAgent Agent => agent;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        //SetState(new IdleState(this));
        SetState(new ChaseState(this));
    }
    private void Update()
    {
        //if (GameManager.Instance.State != GameState.Playing) return;
        currentState.Update();
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
        animator.SetTrigger("isPunching_Left");
    }

    public void SetState(IEnemyState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

}
