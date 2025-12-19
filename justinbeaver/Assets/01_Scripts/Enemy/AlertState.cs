using UnityEngine;
using UnityEngine.AI;

public class AlertState : IEnemyState
{
    private EnemyStatePattern enemy;
    private Transform player;
    private NavMeshAgent agent;
    private float detectRange;
    private const string SleepStateName = "Sleeping";
    private Animator animator;
    public AlertState(EnemyStatePattern Enemy)
    {
        enemy = Enemy;
        player = enemy.PlayerTrf;
        agent = enemy.Agent;
        detectRange = enemy.DetectRange;
        animator = enemy.Animator;
   
    }
    public void Enter()
    {
        agent.isStopped = true;
        agent.ResetPath();
        animator.SetBool("isSleeping", false);
        animator.Update(0f);
        agent.SetDestination(enemy.AlertTargetPos);
        enemy.LightViewOff();
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName(SleepStateName))
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            return;
        }
        agent.isStopped = false;

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
