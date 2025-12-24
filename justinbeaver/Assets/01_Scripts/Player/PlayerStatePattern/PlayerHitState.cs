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
        if (playerContext.playerController)
        {
            playerContext.playerController.enabled = false;
        }

        if (playerContext.playerRigid)
        {
            //기존 속도 제거
            playerContext.playerRigid.linearVelocity = Vector3.zero;

            //위로 튀는 속도 제한
            ClampVerticalVelocity(playerContext.playerRigid, 0.5f);
        }

        var anim = playerContext.GetAnimatorSafe();
        if (anim != null)
        {
            anim.SetBool(HashHit, true);
        }

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

    public void ClampVerticalVelocity(Rigidbody rigid, float maxUpSpeed = 2.5f)
    {
        var vel = rigid.linearVelocity;
        if (vel.y > maxUpSpeed)
        {
            vel.y = maxUpSpeed;
        }
        rigid.linearVelocity = vel;
    }

    public void FixedUpdate() { }
    public void Exit() { }
}
