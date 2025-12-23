using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager Instance;

    public PlayerStats stats;

    void Awake()
    {
        Instance = this;
    }

    public void AddWeight(float weight)
    {
        stats.CurrentWeight += weight;
        stats.CurrentWeight = Mathf.Clamp(stats.CurrentWeight, 0, stats.MaxWeight);
    }

    public void RemoveWeight(float weight)
    {
        stats.CurrentWeight -= weight;
        stats.CurrentWeight = Mathf.Max(0, stats.CurrentWeight);
    }

    public float GetSpeedPenalty()
    {
        return WeightManager.Instance.CalculateSpeedPenalty(
            stats.CurrentWeight,
            stats.Power
        );
    }
}