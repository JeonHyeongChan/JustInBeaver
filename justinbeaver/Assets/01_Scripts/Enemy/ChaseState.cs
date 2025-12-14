using UnityEngine;
using UnityEngine.AI;

public class ChaseState : IEnemyState
{
    private EnemyStatePattern enemy;
    public NavMeshAgent agent;
    private Transform player;
    private float moveSpeed;
    private float attackRange;
    private Animator animator;
    private float detectRange;

    public ChaseState(EnemyStatePattern Enemy)
    {
        enemy = Enemy;
        player = enemy.PlayerTrf;
        moveSpeed = enemy.MoveSpeed;
        attackRange = enemy.AttackRange;
        animator = enemy.Animator;
        agent = enemy.Agent;
        detectRange = enemy.DetectRange;
    }

    public void Enter()
    {
        agent.SetDestination(player.position);
        agent.isStopped = false;
        agent.speed = moveSpeed;

        animator.SetBool("isRunning", true);
    }

    public void Exit()
    {
        animator.SetBool("isRunning", false);
        agent.isStopped = false;
    }

    public void Update()
    {
        agent.SetDestination(player.position);
        float distance = Vector3.Distance(enemy.transform.position, player.position);
        if (distance <= attackRange)
        {
            agent.isStopped = true;
           
            enemy.SetState(new AttackState(enemy));
        }
        if (distance > detectRange)
        {
            //enemy.SetState(new IdleState(enemy));
        }
    }
}
