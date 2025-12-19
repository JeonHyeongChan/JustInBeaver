using System;
using UnityEngine;

public class PlayerContext : MonoBehaviour
{
    [Header("참조")]
    public PlayerController playerController;
    public Animator playerAnimator;
    public Rigidbody playerRigid;
    public PlayerHealth playerhealth;
    public PlayerStateMachine playerStateMachine;
    public PlayerGatherDetector playerGatherDetector;
    //public GatherGauge gatherGauge;
    

    [Header("갈무리 설정")]
    [HideInInspector] public bool isGatherHolding;
    public float gatherCancelDistance = 2.5f;   //대상과 이 거리보다 멀어지면 취소
    public float gatherResumeWindow = 1.0f;     //이 시간 안에 재입력하면 이어서 진행


   
    //진행 저장(재입력 이어서 하기용)
    [HideInInspector] public CaptureObject lastGatherTarget;
    [HideInInspector] public float lastGatherProgress;
    [HideInInspector] public float lastGatherCancelTime;   //취소된 시각

    //이벤트
    public event Action OnPlayerDied;
    public event Action<int> OnHealthChange;

    private void Awake()
    {
        if (!playerRigid)
        {
            playerRigid = GetComponent<Rigidbody>();
        }    
            
        if (!playerController)
        {
            playerController = GetComponent<PlayerController>();
        }

        if (!playerAnimator)
        {
            playerAnimator = GetComponentInChildren<Animator>();
        }

        if (!playerStateMachine)
        {
            playerStateMachine = GetComponent<PlayerStateMachine>();
        }    
            
        if (!playerhealth)
        {
            playerhealth = GetComponent<PlayerHealth>();
        }

        if (!playerGatherDetector)
        {
            playerGatherDetector = GetComponent<PlayerGatherDetector>();
        }

        //if (!gatherGauge)
        //{
        //    gatherGauge = GetComponent<GatherGauge>();
        //}
    }

    public void NotifyDamaged(int currentHp)
    {
        OnHealthChange?.Invoke(currentHp);
    }
    
    public void NotifyDied()
    {
        OnPlayerDied?.Invoke();
    }

    public void OnDieAnimationFinished()
    {
        NotifyDied();
    }
}
