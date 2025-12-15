using UnityEngine;

public class PlayerHitState : IPlayerState
{
    private PlayerContext playerContext;
    private float hitDuration = 0.4f;
    private float endTime;
    private static readonly int HashHit = Animator.StringToHash("Hit");

    public PlayerHitState(PlayerContext context)
    {
        playerContext = context;
    }

    public void Enter()
    {
        if (playerContext.playerMovement)
        {
            playerContext.playerMovement.enabled = false;
        }    
            
        if (playerContext.playerRigid)
        {
            playerContext.playerRigid.linearVelocity = Vector3.zero;
        }

        playerContext.playerAnimator?.SetTrigger(HashHit);
        endTime = Time.time + hitDuration;
    }

    public void Update()
    {
        if (Time.time < endTime)
        {
            return;
        }    
            
        //HP 확인
        if (playerContext.playerhealth != null && playerContext.playerhealth.currentHealth <= 0)
        {
            playerContext.playerStateMachine.ChangeState(new PlayerDieState(playerContext));
        }
        else
        {
            playerContext.playerStateMachine.ChangeState(new PlayerNormalState(playerContext));
        }
    }

    public void FixedUpdate() { }
    public void Exit() { }
}
