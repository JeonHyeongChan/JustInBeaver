using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private PlayerContext playerContext;

    [Header("체력")]
    public int maxHealth = 3;
    public int currentHealth { get; private set; }


    [Header("피격 시 무적 판정")]
    [SerializeField] private float invincibleDuration = 3.0f;
    private float invincibleEndTime = -1f;
    public bool IsInvincible => Time.time < invincibleEndTime;


    private bool rollInvincible = false; //구르기 무적 판정용


    public event Action<int, int> OnHealthChanged;


    private void Awake()
    {
        currentHealth = maxHealth;
        playerContext = GetComponent<PlayerContext>();
    }

    public void TakeDamage()
    {
        // 무적이면 데미지 무시
        if (IsInvincible)
        {
            return;
        }


        //무적 시작
        invincibleEndTime = Time.time + invincibleDuration;

        currentHealth = Mathf.Max(0, currentHealth - 1);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        //사운드
        SoundManager.Instance?.PlaySFX(SFXType.BeaverHit);


        //피격 시 이동속도 버프 
        var buff = GetComponent<PlayerSpeedBuff>();
        buff?.ApplyHitSpeedBoot();

        Debug.Log("플레이어 이속 증가");
        Debug.Log($"플레이어 피격: 무적 {invincibleDuration}초 시작");


        if (currentHealth <= 0)
        {            
            playerContext.playerStateMachine.ChangeState(new PlayerDieState(playerContext));
        }

        else
        {
            playerContext.playerStateMachine.ChangeState(new PlayerHitState(playerContext));
        }  
    }


    public void ResetHealth()
    {
        currentHealth = maxHealth;
        invincibleEndTime = -1f;
        rollInvincible = false;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }


    //구르기 무적 on/off
    public void SetRollInvincible(bool on)
    {
        rollInvincible = on;
    }
}
