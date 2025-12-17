using UnityEngine;
using UnityEngine.AI;

public class AlertState : IEnemyState
{
    private EnemyStatePattern enemy;
    private Transform player;
    private NavMeshAgent agent;
    private float detectRange;
    public AlertState(EnemyStatePattern Enemy)
    {
        enemy = Enemy;
        player = enemy.PlayerTrf;
        agent = enemy.Agent;
        detectRange = enemy.DetectRange;
   
    }
    public void Enter()
    {
        agent.SetDestination(enemy.AlertTargetPos);
        
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && agent.velocity.sqrMagnitude == 0)
        {
            enemy.SetState(new IdleState(enemy));
        }
        float distance = Vector3.Distance(enemy.transform.position, player.position);
        if (distance <= detectRange || enemy.LightView.HasTarget())
        {
            enemy.SetState(new ChaseState(enemy));
        }
    }
}
