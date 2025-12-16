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
    private float lostTimer;            // 못 본 시간
    private const float LostLimit = 3f; // 3초 지나면 포기
    private LightView lightView;
    public ChaseState(EnemyStatePattern Enemy)
    {
        enemy = Enemy;
        player = enemy.PlayerTrf;
        moveSpeed = enemy.MoveSpeed;
        attackRange = enemy.AttackRange;
        animator = enemy.Animator;
        agent = enemy.Agent;
        detectRange = enemy.DetectRange;
        lightView = enemy.LightView;
    }

    public void Enter()
    {
        lostTimer = 0f;

        agent.isStopped = false;
        agent.speed = moveSpeed;

        animator.SetBool("isRunning", true);

        if (player != null)
            agent.SetDestination(player.position);
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
        agent.isStopped = false;
        agent.SetDestination(player.position);

        if (lightView != null && lightView.HasTarget())
        {
            lostTimer = 0f;
        }
        else
        {
            lostTimer += Time.deltaTime;
            if (lostTimer >= LostLimit)
            {
                enemy.SetState(new IdleState(enemy));
                return;
            }
        }
    }
}
