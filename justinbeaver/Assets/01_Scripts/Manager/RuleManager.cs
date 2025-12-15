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
    
}
