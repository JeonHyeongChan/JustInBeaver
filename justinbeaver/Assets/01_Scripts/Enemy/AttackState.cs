using UnityEngine;
using UnityEngine.AI;

public class AttackState : IEnemyState
{
    private EnemyStatePattern enemy;
    private Animator animator;
    private Transform player;
    public NavMeshAgent agent;
    private float attackRange;
    public AttackState(EnemyStatePattern Enemy)
    {
        enemy = Enemy;
        animator = enemy.Animator;
        player = enemy.PlayerTrf;
        attackRange = enemy.AttackRange;
        agent = enemy.Agent;
    }

    public void Enter()
    {
        agent.isStopped = true;
        //if(만약 플레이어가 없다면 리턴)

        //플레이어 데미지 보내기 or 처리
        enemy.StartAttackCooldown();
    }

    public void Exit()
    {
        enemy.hitBox.SetActive(false);
        enemy.StopAttackCooldown();
    }

    public void Update()
    {
        if (enemy.IsAttackAnimPlaying) return;
        float distance = Vector3.Distance(enemy.transform.position, player.position);
        if (distance > attackRange)
        {
            enemy.SetState(new ChaseState(enemy));
        }
    }
}
