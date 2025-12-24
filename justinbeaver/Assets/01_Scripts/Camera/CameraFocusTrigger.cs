using UnityEngine;
using Unity.Cinemachine; // [중요] 3.x 버전 네임스페이스

public class CameraFocusTrigger : MonoBehaviour
{
    [Header("Cinemachine 3.x Components")]
    public CinemachineCamera focusCam;       // 변경됨: VirtualCamera -> CinemachineCamera
    public CinemachineTargetGroup targetGroup;

    [Header("Settings")]
    public Transform interactionTarget;      // NPC 또는 상호작용 오브젝트
    public int highPriority = 20;            // 활성화 시 우선순위
    public int lowPriority = 0;              // 비활성화 시 우선순위

    private Transform playerTransform;

    private void Awake()
    {
        // focusCam 자동 연결
        if (focusCam == null)
        {
            focusCam = FindAnyObjectByType<CinemachineCamera>();
            if (focusCam == null)
                Debug.LogError("[CameraFocusTrigger] CinemachineCamera not found");
        }

        // targetGroup 자동 연결
        if (targetGroup == null)
        {
            targetGroup = FindAnyObjectByType<CinemachineTargetGroup>();
            if (targetGroup == null)
                Debug.LogError("[CameraFocusTrigger] CinemachineTargetGroup not found");
        }

        // interactionTarget 기본값
        if (interactionTarget == null)
            interactionTarget = transform;
    }

    private void Start()
    {
        // 1. "Player" 태그를 가진 오브젝트 찾기
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            playerTransform = playerObj.transform;

            // 2. 타겟 그룹의 0번 인덱스에 플레이어 할당
            // 3.x API에서는 Targets 리스트를 직접 수정
            var targets = targetGroup.Targets;

            if (targets != null && targets.Count > 0)
            {
                targets[0].Object = playerTransform; // 0번 자리에 플레이어 꽂기
                targets[0].Weight = 1f;
                targets[0].Radius = 2f;
            }
            else
            {
                // 만약 리스트가 비어있다면 새로 추가 (AddMember 사용)
                targetGroup.AddMember(playerTransform, 1f, 2f);
            }

            // 변경된 리스트를 다시 적용 (중요)
            targetGroup.Targets = targets;
        }
        else
        {
            Debug.LogWarning("Player 태그를 가진 오브젝트를 찾을 수 없습니다.");
        }

        // 상호작용 대상이 비어있으면 이 스크립트가 달린 오브젝트로 설정
        if (interactionTarget == null)
            interactionTarget = transform;

        // 시작 시 카메라 우선순위 초기화
        if (focusCam != null) focusCam.Priority = lowPriority;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 상호작용 대상(NPC)을 타겟 그룹에 추가
            AddTargetToGroup(interactionTarget, 1f, 2f);

            // 카메라 우선순위 높여서 전환
            focusCam.Priority = highPriority;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 카메라 우선순위 원상복구
            focusCam.Priority = lowPriority;

            // 상호작용 대상 제거
            RemoveTargetFromGroup(interactionTarget);
        }
    }

    // 헬퍼 함수: 타겟 그룹 추가
    void AddTargetToGroup(Transform t, float weight, float radius)
    {
        // 이미 그룹에 있는지 확인
        foreach (var item in targetGroup.Targets)
        {
            if (item.Object == t) return;
        }
        targetGroup.AddMember(t, weight, radius);
    }

    // 헬퍼 함수: 타겟 그룹 제거
    void RemoveTargetFromGroup(Transform t)
    {
        targetGroup.RemoveMember(t);
    }
}