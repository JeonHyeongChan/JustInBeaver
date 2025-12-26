using UnityEngine;

public class PlayerGatherState : IPlayerState
{
    private readonly PlayerContext playerContext;
    private readonly CaptureObject target;

    private static readonly int HashIsGathering = Animator.StringToHash("IsGathering");

    public PlayerGatherState(PlayerContext context, CaptureObject gatherTarget, float startProgress)
    {
        playerContext = context;
        target = gatherTarget;

        //이어하기 시작값 반영 (오브젝트별 진행도)
        if (target != null)
        {
            target.SetProgress(startProgress);   //내부에서 Clamp01 하도록
        }
    }


    public void Enter()
    {

        if (!IsTargetValid())
        {
            GoNormalAndClearResumeIfInvalid();
            return;
        }

        SoundManager.Instance?.PlayLoopSFX(SFXType.GatheringLoop); // 사운드

        var anim = playerContext.GetAnimatorSafe();
        if (anim != null)
        {
            anim.SetBool(HashIsGathering, true);
        }

        if (!IsTargetValid())
        {
            GoNormalAndClearResumeIfInvalid();
            return;
        }

        //적/시스템 알림용 시그널
        var signal = playerContext.GetComponent<PlayerGatherSignal>();
        signal?.RaiseGatherStart(target.transform.position);

        //UI 표시
        UIManager.Instance?.ShowGatherGauge();
        UIManager.Instance?.GatherGauge?.SetValue(target.progress);
    }


    public void Update()
    {
        if (!IsTargetValid())
        {
            ExitToNormal();
            return;
        }

        //거리 이탈, 중단만 하고 진행도는 유지
        float dist = Vector3.Distance(playerContext.transform.position, target.transform.position);
        if (dist > playerContext.gatherCancelDistance)
        {
            PauseAndExit();
            return;
        }

        //홀드 해제, 중단만 하고 진행도는 유지
        if (!playerContext.isGatherHolding)
        {
            PauseAndExit();
            return;
        }

        //진행 누적(오브젝트가 진행도 보유)
        target.TickGather(Time.deltaTime);

        UIManager.Instance?.GatherGauge?.SetValue(target.progress);

        //완료
        if (target.progress >= 1f)
        {
            target.OnGatherComplete();   //내부에서 드랍 + 풀 반환
            ClearResumeCache();
            ExitToNormal();
        }
    }



    public void Exit()
    {
        UIManager.Instance?.HideGahterGauge();

        var anim = playerContext.GetAnimatorSafe();
        if (anim != null)
        {
            anim.SetBool(HashIsGathering, false);
        }

        var signal = playerContext.GetComponent<PlayerGatherSignal>();
        signal?.RaiseGatherEnd();

        SoundManager.Instance?.StopLoopSFX(SFXType.GatheringLoop); // 사운드
    }


    private bool IsTargetValid()
    {
        if (target == null)
        {
            return false;
        }
        if (!target.gameObject.activeInHierarchy)
        {
            return false;
        }
        if (target.isCompleted)
        {
            return false;   //완료된 대상은 다시 진행 불가
        }
        return true;
    }


    private void PauseAndExit()
    {
        //이어하기 캐시 저장
        playerContext.lastGatherTarget = target;
        playerContext.lastGatherProgress = target.progress;
        playerContext.lastGatherCancelTime = Time.time;

        ExitToNormal();
    }


    private void ExitToNormal()
    {
        playerContext.isGatherHolding = false;
        playerContext.playerAnimator.SetBool(HashIsGathering, false);
        playerContext.playerStateMachine.ChangeState(new PlayerNormalState(playerContext));
    }

    private void GoNormalAndClearResumeIfInvalid()
    {
        if (playerContext.lastGatherTarget == target)
            ClearResumeCache();

        ExitToNormal();
    }

    private void ClearResumeCache()
    {
        playerContext.lastGatherTarget = null;
        playerContext.lastGatherProgress = 0f;
        playerContext.lastGatherCancelTime = 0f;
    }

    public void FixedUpdate() { }
}
