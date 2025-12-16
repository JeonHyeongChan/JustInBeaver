using UnityEngine;

public class PlayerGatherState : IPlayerState
{
    private readonly PlayerContext playerContext;
    private readonly CaptureObject target;

    private float elapsed;   //누적 진행 시간
    private float duration;  //대상별 총 시간

    public PlayerGatherState(PlayerContext context, CaptureObject gatherTarget, float startProgress01)
    {
        playerContext = context;
        target = gatherTarget;
        elapsed = Mathf.Clamp01(startProgress01);
    }

    public void Enter()
    {
        if (target == null)
        {
            playerContext.playerStateMachine.ChangeState(new PlayerNormalState(playerContext));
            return;
        }

        if (playerContext.gatherGauge == null)
        {
            Debug.LogError("[PlayerContext] gatherGauge가 연결되지 않았습니다");
            playerContext.playerStateMachine.ChangeState(new PlayerNormalState(playerContext));
            return;
        }

        duration = Mathf.Max(0.05f, target.gatherDuration);
        elapsed *= duration;

        playerContext.gatherGauge.Show();
        playerContext.gatherGauge.SetValue(elapsed / duration);
    }

    public void Update()
    {
        if (target == null)
        {
            CancelAndReset();
            return;
        }

        //대상과 멀어지면 즉시 취소 + 초기화
        float dist = Vector3.Distance(playerContext.transform.position, target.transform.position);
        if (dist > playerContext.gatherCancelDistance)
        {
            CancelAndReset();
            return;
        }

        //홀드가 풀렸으면 상태는 Normal로 돌아가되, 진행도는 저장(유예시간 내 재입력 시 이어하기)
        if (!playerContext.isGatherHolding)
        {
            SaveProgressForResume();
            playerContext.playerStateMachine.ChangeState(new PlayerNormalState(playerContext));
            return;
        }

        //홀드 중이면 진행
        elapsed += Time.deltaTime;

        float t = Mathf.Clamp01(elapsed / duration);
        playerContext.gatherGauge.SetValue(t);

        if (t >= 1f)
        {
            //완료 처리
            target.OnGatherComplete();

            //저장값 초기화
            playerContext.lastGatherTarget = null;
            playerContext.lastGatherProgress = 0f;
            playerContext.lastGatherCancelTime = 0f;

            playerContext.playerStateMachine.ChangeState(new PlayerNormalState(playerContext));
        }
    }

    public void FixedUpdate() { }

    public void Exit()
    {
        if (playerContext.gatherGauge != null)
            playerContext.gatherGauge.Hide();

        if (playerContext.playerController) playerContext.playerController.enabled = true;
    }

    private void SaveProgressForResume()
    {
        playerContext.lastGatherTarget = target;
        playerContext.lastGatherProgress = Mathf.Clamp01(elapsed / duration);
        playerContext.lastGatherCancelTime = Time.time;
    }

    private void CancelAndReset()
    {
        //거리 이탈 취소는 즉시 초기화
        playerContext.lastGatherTarget = null;
        playerContext.lastGatherProgress = 0f;
        playerContext.lastGatherCancelTime = 0f;

        playerContext.isGatherHolding = false;
        playerContext.playerStateMachine.ChangeState(new PlayerNormalState(playerContext));
    }
}
