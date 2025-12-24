using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private PlayerContext playerContext;

    [Header("체력")]
    public int maxHealth = 3;

    public int currentHealth { get; private set; }

    public event Action<int, int> OnHealthChanged;

    private void Awake()
    {
        currentHealth = maxHealth;
        playerContext = GetComponent<PlayerContext>();
    }

    public void TakeDamage()
    {
        currentHealth = Mathf.Max(0, currentHealth - 1);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        
        if (currentHealth <= 0)
        {            
            playerContext.playerStateMachine.ChangeState(new PlayerDieState(playerContext));
        }   
        else
        {
            playerContext.playerStateMachine.ChangeState(new PlayerHitState(playerContext));
        }

        var buff = GetComponent<PlayerSpeedBuff>();

        if (buff != null)
        {
            buff.ApplyHitSpeedBoot();
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

}
