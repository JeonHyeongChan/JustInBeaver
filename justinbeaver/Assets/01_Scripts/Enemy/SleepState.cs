using UnityEngine;
using UnityEngine.AI;

public class SleepState : IEnemyState
{
    private EnemyStatePattern enemy;
    public NavMeshAgent agent;
    private Animator animator;
    public SleepState(EnemyStatePattern Enemy)
    {
        enemy = Enemy;
        agent = enemy.Agent;
        animator = enemy.Animator;
    }
    public void Enter()
    {
        agent.isStopped = true;
        enemy.LightViewOff();
        enemy.Sleep();
    }

    public void Exit()
    {
        animator.SetBool("isSleeping", false);
    }

    public void Update()
    {
       
    }
}
