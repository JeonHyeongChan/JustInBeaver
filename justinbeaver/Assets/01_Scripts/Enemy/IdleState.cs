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
    public IdleState(EnemyStatePattern Enemy)
    {
        enemy = Enemy;
        agent = enemy.Agent;
        updateInterval = enemy.UpdateInterval;
        timeSinceLastUpdate = enemy.TimeSinceLastUpdate;
        //player = enemy.PlayerTrf;
        detectRange = enemy.DetectRange;
        lightView = enemy.LightView;

    }

    public void Enter()
    {
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

        //float distance = Vector3.Distance(enemy.transform.position, player.position);
        float distance = Vector3.Distance(enemy.transform.position, enemy.PlayerTrf.position);

        //if (lightView.HasTarget() || distance <= detectRange)
        //{
        //    if (lightView.HasTarget())
        //        enemy.SetAlertTargetPos(lightView.GetFirstTarget().position);
        //    
        //    enemy.SetState(new ChaseState(enemy));
        //}
        if (distance <= detectRange)
        {
            enemy.SetState(new ChaseState(enemy));
        }
        //만약 비버 갈무리 이벤트 발생시?
        //enemy.SetState(new ChaseState(enemy));
    }
}
