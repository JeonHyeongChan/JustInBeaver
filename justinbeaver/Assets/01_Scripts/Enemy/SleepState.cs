using UnityEngine;

public class SleepState : IEnemyState
{
    private EnemyStatePattern enemy;

    public SleepState(EnemyStatePattern Enemy)
    {
        enemy = Enemy;
    }
    public void Enter()
    {
        enemy.Sleep();
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        //if ( GameManager.Instance.isHarvest == true )
        enemy.SetState(new ChaseState(enemy));
    }
}
