using System;
using System.Collections.Generic;
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

    private PlayerContext player;

    private GameState currentState = GameState.Playing;

    [Header("Rule Data")]
    private int escapeFailCount = 0;                // 침입 0 ~ 3
    private int escapeSuccessCount = 0;             // 성공

    private bool endingTriggered = false;           //엔딩 가드용

    //이벤트
    public event Action OnPlayerRespawnRequired;    // 비버 리스폰
    public event Action OnTotalResetRequired;       // 전체 리셋
    public event Action OnEndingCondition;          // 엔딩 조건달성
    public event Action OnEscapeSucceeded;          // 탈출 성공 시
    public event Action<int> OnEscapeFailedVisual;  // 포크레인

    public bool IsTotalReset { get; private set; }

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

    private void OnEnable()
    {
        player = FindAnyObjectByType<PlayerContext>();

        if (player != null)
        {
            player.OnPlayerDied += HandlePlayerDied;
        }
    }

    private void OnDisable()
    {
        if (player != null)
        {
            player.OnPlayerDied -= HandlePlayerDied;
        }
    }

    /// <summary>
    /// 탈출 성공 처리
    /// </summary>
    public void OnEscapeSuccess()
    {
        escapeSuccessCount++;
        OnEscapeSucceeded?.Invoke();
        UIManager.Instance?.ShowEscapeSuccessUI();

        CheckEndingCondition();
    }

    /// <summary>
    /// 탈출 실패 처리
    /// </summary>
    public void OnEscapeFailed()
    {
        escapeFailCount++;

        Debug.Log($"CurrentFaild: {escapeFailCount}");

        OnEscapeFailedVisual?.Invoke(escapeFailCount);
        GameManager.Instance?.SaveGame();

        CheckTotalReset();
    }

    private void HandlePlayerDied()
    {
        OnEscapeFailed();
        //OnPlayerRespawnRequired?.Invoke();  // 이벤트
        UIManager.Instance?.ShowGameFailUI();
    }

    //==============조건 체크==============

    /// <summary>
    /// 엔딩 조건 판단
    /// </summary>
    private void CheckEndingCondition()
    {
        if (endingTriggered)
        {
            return;
        }
            
        //집이 최고레벨인지
        if (HomeManager.Instance != null &&
            HomeManager.Instance.CurrentLevel >= HomeManager.MaxLevel)
        {
            endingTriggered = true; // 엔딩 트리거 true
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
            IsTotalReset = true;
            endingTriggered = false; // 엔딩 트리거 false;

            ForceResetSave();
            OnTotalResetRequired?.Invoke();
        }
    }

    public void ClearTotalResetPoint()
    {
        IsTotalReset = false;
    }

    private void ForceResetSave()
    {
        Debug.Log("[RuleManager] Force Reset Save");

        var data = new SaveData
        {
            houseLevel = 1,
            failCountAtcurrentLevel = 0,
            storedItems = new List<StoredItem>()
        };

        SaveManager.Save(data);
    }

    //==============일단 읽기전용==============
    public GameState GetGameState() => currentState;   
    public int GetEscapeFailCount() => escapeFailCount;
    public void ResetFailCount()
    {
        escapeFailCount = 0; // 홈매니저가 쓸 실패 카운터 리셋
    }

    public void SetFailCount(int count)
    {
        escapeFailCount = Mathf.Max(0, count);
    }
}
