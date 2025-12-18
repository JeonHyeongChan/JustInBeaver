using System;
using UnityEngine;

/// <summary>
/// 게임 규칙 및 진행 상태를 판단하는 매니저
/// 씬로드 / UI / 오브젝트등 생성 없어야함 XXX
/// </summary>

public enum GameState
{
    Playing,    // 정상 플레이
    Reseting,   // 실패 3회시 -> 전체 리셋 필요
    EndingReady // 엔딩 조건 충족시
}

public class RuleManager : MonoBehaviour
{
    public static RuleManager Instance;

    private GameState currentState = GameState.Playing;

    [Header("Rule Data")]
    private int escapeFailCount = 0;        // 침입 0 ~ 3
    private int escapeSuccessCount = 0;     // 성공

    //이벤트
    public event Action OnTotalResetRequired;
    public event Action OnEndingCondition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 탈출 성공 처리
    /// </summary>
    public void OnEscapeSuccess()
    {
        escapeSuccessCount++;        

        CheckEndingCondition();
    }

    /// <summary>
    /// 탈출 실패 처리
    /// </summary>
    public void OnEscapeFailed()
    {
        escapeFailCount++;        

        CheckTotalReset();
    }

    //==============조건 체크==============

    /// <summary>
    /// 엔딩 조건 판단
    /// </summary>
    private void CheckEndingCondition()
    {
        //집이 최고레벨인지
        if (HomeManager.Instance != null &&
            HomeManager.Instance.CurrentLevel >= HomeManager.MaxLevel)
        {
            currentState = GameState.EndingReady;
            OnEndingCondition?.Invoke();
        }
    }

    /// <summary>
    /// 실패 3회 누적 시 전체 리셋 판단
    /// </summary>
    private void CheckTotalReset()
    {
        if (escapeFailCount >= 3)
        {
            currentState = GameState.Reseting;
            escapeFailCount = 0; // 실패 누적 초기화

            OnTotalResetRequired?.Invoke();
        }
    }

    //==============일단 읽기전용==============
    public GameState GetGameState() => currentState;   
    public int GetEscapeFailCount() => escapeFailCount;
    public void ResetFailCount()
    {
        escapeFailCount = 0; // 홈매니저가 쓸 실패 카운터 리셋
    }
}
