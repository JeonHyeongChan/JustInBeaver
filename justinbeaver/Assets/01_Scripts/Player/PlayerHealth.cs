using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private PlayerContext playerContext;

    [Header("체력")]
    public int maxHealth = 3;

    public int currentHealth { get; private set; }
    private int damage = 1;

    private void Awake()
    {
        currentHealth = maxHealth;
        playerContext = GetComponent<PlayerContext>();
    }


    public void TakeDamage()
    {
        currentHealth -= damage;

        //HP에 따라 상태 전환
        if (currentHealth <= 0)
        {
            playerContext.playerStateMachine.ChangeState(new PlayerDieState(playerContext));
        }
        else
        {
            playerContext.playerStateMachine.ChangeState(new PlayerHitState(playerContext));
        }

    }
}
