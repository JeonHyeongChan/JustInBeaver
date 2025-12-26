using UnityEngine;
using System;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager Instance { get; private set; }

    [Header("Stats")]
    [SerializeField] private float strength = 5f;

    [Header("Weight")]
    [SerializeField] private float baseMaxWeight = 5f;        //기본 최대무게
    [SerializeField] private float maxWeightPerStrength = 0.5f;   //Strength 1당 최대무게 증가량
    private float maxWeightBonus = 0f;                          //업그레이드/버프 보너스

    public float Strength => strength;
    public float CurrentWeight { get; private set; }

    // 무게 변경 이벤트
    public event Action<float> OnWeightChanged;
    public event Action OnStatsChanged;        //스탯 UI용

    // 현재 최대 무게(Strength + 업그레이드 반영)
    public float MaxWeight => baseMaxWeight + (strength * maxWeightPerStrength) + maxWeightBonus;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 인벤토리에서 총 무게 계산 후 호출
    /// </summary>
    public void SetCurrentWeight(float weight)
    {
        CurrentWeight = Mathf.Max(0, weight);
        Debug.Log($"[Weight] SetCurrentWeight = {CurrentWeight}");
        OnWeightChanged?.Invoke(CurrentWeight);
        OnStatsChanged?.Invoke();
    }


    public float GetSpeedPenalty()
    {
        if (WeightManager.Instance == null)
        {
            return 0f;
        }

        return WeightManager.Instance.CalculateSpeedPenalty
        (
            CurrentWeight,
            MaxWeight
        );
    }


    public void AddStrength(float amount)
    {
        if (amount <= 0f)
        {
            return;
        }
        strength += amount;
        OnStatsChanged?.Invoke();
        OnWeightChanged?.Invoke(CurrentWeight);
    }


    public void UpgradeMaxWeight(float amount)
    {
        maxWeightBonus += amount;
        OnStatsChanged?.Invoke();
        OnWeightChanged?.Invoke(CurrentWeight);
    }
}