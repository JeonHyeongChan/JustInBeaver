using UnityEngine;
using System.Collections.Generic;

public class WeightManager : MonoBehaviour
{
    public static WeightManager Instance;

    [Header("Penalty")]
    [SerializeField] private float overWeight = 0.05f;          //초과 무게 1당 속도 감소량
    [SerializeField] private float maxPenaltyAtFull = 3f;       //가득 찼을 때 최대 감소량(이동속도에서 빼는 값)


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    //ItemData에서 무게를 바로 읽음
    public float GetItemWeight(ItemData data)
    {
        if (data == null)
        {
            return 0f;
        }
        return Mathf.Max(0f, data.itemWeight);
    }


    //currentWeight / maxWeight 기반 패널티
    public float CalculateSpeedPenalty(float currentWeight, float maxWeight)
    {
        float max = Mathf.Max(1f, maxWeight);
        float ratio = Mathf.Clamp01(currentWeight / max);

        //0%면 0, 100%면 maxPenaltyAtFull
        float penalty = ratio * maxPenaltyAtFull;

        return Mathf.Clamp(penalty, 0f, maxPenaltyAtFull);
    }
}