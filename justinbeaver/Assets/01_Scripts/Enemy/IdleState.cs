using UnityEngine;
using UnityEngine.AI;

public class IdleState : IEnemyState
{
    private EnemyStatePattern enemy;
    private NavMeshAgent agent;
    private float updateInterval;
    private float timeSinceLastUpdate;
    private Transform player;
    private float detectRange;
    private LightView lightView;
    private Animator animator;
    public IdleState(EnemyStatePattern Enemy)
    {
        enemy = Enemy;
        agent = enemy.Agent;
        updateInterval = enemy.UpdateInterval;
        timeSinceLastUpdate = enemy.TimeSinceLastUpdate;        
        detectRange = enemy.DetectRange;
        lightView = enemy.LightView;
        animator = enemy.Animator;
    }

    public void Enter()
    {
        animator.SetBool("isRunning", false);
        timeSinceLastUpdate = updateInterval;
        enemy.LightViewOff();
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        if (enemy.PlayerTrf == null)
            return;

        timeSinceLastUpdate += Time.deltaTime;
        if (timeSinceLastUpdate >= updateInterval)
        {
            Vector3 randomPosition = enemy.GetRandomPositionNavMesh();
            agent.SetDestination(randomPosition);
            timeSinceLastUpdate = 0f;
        }
        
        float distance = Vector3.Distance(enemy.transform.position, enemy.PlayerTrf.position);
        
        if (distance <= detectRange)
        {
            enemy.SetState(new ChaseState(enemy));
        }        
    }
}
