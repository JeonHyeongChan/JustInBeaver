using UnityEngine;
using System;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager Instance { get; private set; }

    [Header("Stats")]
    [SerializeField] private int strengthLevel = 1;
    public int StrengthLevel => strengthLevel;

    [SerializeField] private float strength = 5f;
    public float Strength => strength;

    [Header("Weight")]
    [SerializeField] private float baseMaxWeight = 5f;        //기본 최대무게
    [SerializeField] private float maxWeightPerStrength = 0.5f;   //Strength 1당 최대무게 증가량
    private float maxWeightBonus = 0f;                          //업그레이드/버프 보너스

   
    public float CurrentWeight { get; private set; }

    // 현재 최대 무게(Strength + 업그레이드 반영)
    public float MaxWeight => baseMaxWeight + (strength * maxWeightPerStrength) + maxWeightBonus;

    // 무게 변경 이벤트
    public event Action<float> OnWeightChanged;
    public event Action OnStatsChanged;        //스탯 UI용

    [Header("Upgrade Data")]
    [SerializeField] private StrengthUpgradeData[] upgradeData; // 레벨 순서대로(1->2,2->3...)



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


    public StrengthUpgradeData GetNextStrengthUpgradeData()
    {
        //strengthLevel 1이면 다음은 index 1 (2레벨 데이터)
        int index = strengthLevel;
        if (upgradeData == null || index < 0 || index >= upgradeData.Length)
        {
            return null;
        }
        return upgradeData[index];
    }

    public bool CanUpgradeStrength()
    {
        return GetNextStrengthUpgradeData() != null;
    }


    public bool TryUpgradeStrengthWithMaterials()
    {
        var data = GetNextStrengthUpgradeData();
        if (data == null)
        {
            return false;
        }    
            
        if (StorageManager.Instance == null)
        {
            return false;
        }    
            
        if (!StorageManager.Instance.CheckSufficientItems(data.requiredMaterials))
        {
            return false;
        }
        StorageManager.Instance.ConsumeItems(data.requiredMaterials);


        //스텟 반영
        strength += data.addStrength;
        strengthLevel = Mathf.Max(strengthLevel, data.targetLevel);


        //UI 갱신 이벤트
        OnStatsChanged?.Invoke();
        OnWeightChanged?.Invoke(CurrentWeight);
        return true;
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


  
    public void UpgradeMaxWeight(float amount)
    {
        maxWeightBonus += amount;
        OnStatsChanged?.Invoke();
        OnWeightChanged?.Invoke(CurrentWeight);
    }


    public void ResetWeightToZero()
    {
        SetCurrentWeight(0f);
    }

}