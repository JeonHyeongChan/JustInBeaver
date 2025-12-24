using UnityEngine;
using System.Collections.Generic;

public class WeightManager : MonoBehaviour
{
    public static WeightManager Instance;

    [Header("Penalty")]
    [SerializeField] private float overWeight = 0.05f;          // 초과 무게 1당 속도 감소량
    [SerializeField] private float maxPenalty = 4f;             // 최대 감소량


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
        return Mathf.Max(0f, data.weight);
    }


    //CurrentWeight / Strength 기반 패널티
    public float CalculateSpeedPenalty(float currentWeight, float strength)
    {
        float limit = Mathf.Max(0f, strength);
        float over = Mathf.Max(0f, currentWeight - limit);

        float penalty = over * overWeight;
        return Mathf.Clamp(penalty, 0f, maxPenalty);
    }
}