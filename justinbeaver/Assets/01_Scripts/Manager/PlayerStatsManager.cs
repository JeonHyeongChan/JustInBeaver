using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    [Header("Base Stats")]
    public float baseSpeed = 5f;
    public float strength = 10f;

    float currentSpeed;

    public void UpdateSpeed(float totalWeight)
    {
        float penaltyRate = Mathf.Clamp01((totalWeight / strength) * 0.05f);
        currentSpeed = baseSpeed * (1f - penaltyRate);

        Debug.Log($"속도 적용: {currentSpeed}");
    }

    public float GetSpeed()
    {
        return currentSpeed;
    }
}