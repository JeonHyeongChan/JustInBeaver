using UnityEngine;
using System;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager Instance { get; private set; }

    [Header("Stats")]
    [SerializeField] private float strength = 50f;
    [SerializeField] private float maxWeight = 100f;

    public float Strength => strength;
    public float MaxWeight => maxWeight;
    public float CurrentWeight { get; private set; }

    // 무게 변경 이벤트
    public event Action<float> OnWeightChanged;

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
        OnWeightChanged?.Invoke(CurrentWeight);
    }

    public float GetSpeedPenalty()
    {
        if (WeightManager.Instance == null)
            return 0f;

        return WeightManager.Instance.CalculateSpeedPenalty(
            CurrentWeight,
            Strength
        );
    }
}