using UnityEngine;


public class PlayerHitState : MonoBehaviour, IPlayerState
{
    private PlayerContext playerContext;
    private float hitDuration = 0.4f;
    private float endTime;
    private const float MaxUpSpeedDuringHit = 0f;

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

        //if (playerContext.playerRigid)
        //{
        //    //기존 속도 제거
        //    playerContext.playerRigid.linearVelocity = Vector3.zero;
        //    playerContext.playerRigid.angularVelocity = Vector3.zero;
        //}

        //피격 점멸 시작
        var blinker = playerContext.GetComponent<HitBlinker>();
        if (blinker != null)
        {
            blinker.StartBlink(hitDuration);
        }

        // 피격 시 3초 이동속도 버프
        var buff = playerContext.GetComponent<PlayerSpeedBuff>();
        if (buff != null)
        {
            buff.ApplyHitSpeedBoot();
        }

        var anim = playerContext.GetAnimatorSafe();
        if (anim != null)
        {
            anim.SetBool(HashHit, true);
        }

        endTime = Time.time + hitDuration;
    }

    public void FixedUpdate()
    {
        var rigid = playerContext.playerRigid;

        if (rigid == null)
        {
            return;
        }

        var vel = rigid.linearVelocity;
        if (vel.y > MaxUpSpeedDuringHit)
        {
            vel.y = MaxUpSpeedDuringHit;
            rigid.linearVelocity = vel;
        }
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

    
    public void Exit()
    {
        //피격 점멸 종료   
        var blinker = playerContext.GetComponent<HitBlinker>();
        if (blinker != null)
        {
            blinker.StopBlink();
        }
    }
}
