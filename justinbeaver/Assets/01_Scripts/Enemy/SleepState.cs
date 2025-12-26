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
        agent.enabled = false;
        enemy.LightViewOff();
        enemy.StartCoroutine(enemy.SleepWait());
        enemy.flashLight.SetActive(false);
    }

    public void Exit()
    {
        animator.SetBool("isSleeping", false);
        enemy.flashLight.SetActive(true);
    }

    public void Update()
    {
       
    }
}
