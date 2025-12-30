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
        var pc = playerContext != null ? playerContext.playerController : null;
        bool isEscapingHold = (pc != null) && pc.IsHoldingEscapeHold();

        if (!isEscapingHold && pc != null)
        {
            pc.enabled = false;
        }

        //탈출 홀드가 아닐 때만 멈춤 처리
        if (!isEscapingHold)
        {
            var rb = playerContext != null ? playerContext.playerRigid : null;
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        //피격 점멸 시작
        var blinker = playerContext != null ? playerContext.GetComponent<HitBlinker>() : null;
        if (blinker != null)
        {
            blinker.StartBlink(hitDuration);
        }

        // 피격 시 3초 이동속도 버프
        var buff = playerContext != null ? playerContext.GetComponent<PlayerSpeedBuff>() : null;
        if (buff != null)
        {
            buff.ApplyHitSpeedBoot();
        }

        var anim = playerContext != null ? playerContext.GetAnimatorSafe() : null;
        if (anim != null)
        {
            anim.SetBool(HashHit, true);
        }

        endTime = Time.time + hitDuration;
    }

    public void FixedUpdate()
    {
        var rb = playerContext != null ? playerContext.playerRigid : null;
        if (rb == null) return;

        var pc = playerContext != null ? playerContext.playerController : null;
        bool isEscapingHold = (pc != null) && pc.IsHoldingEscapeHold();
        if (isEscapingHold) return; // 탈출 홀드 중에는 y속도 제한도 하지 않음

        var vel = rb.linearVelocity;
        if (vel.y > MaxUpSpeedDuringHit)
        {
            vel.y = MaxUpSpeedDuringHit;
            rb.linearVelocity = vel;
        }
    }

    public void Update()
    {
        if (Time.time < endTime) return;

        //HP 확인
        if (playerContext != null && playerContext.playerhealth != null &&
            playerContext.playerhealth.currentHealth <= 0)
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
        var blinker = playerContext != null ? playerContext.GetComponent<HitBlinker>() : null;
        if (blinker != null)
        {
            blinker.StopBlink();
        }

        var anim = playerContext != null ? playerContext.GetAnimatorSafe() : null;
        if (anim != null)
        {
            anim.SetBool(HashHit, false);
        }

        if (playerContext != null && playerContext.playerController != null)
        {
            playerContext.playerController.enabled = true;
        }
    }
}
