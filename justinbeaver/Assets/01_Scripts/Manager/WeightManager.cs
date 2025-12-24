using UnityEngine;
using System.Collections.Generic;

public class WeightManager : MonoBehaviour
{
    public static WeightManager Instance;

    [SerializeField]
    private List<ItemWeightPair> itemWeights;

    Dictionary<string, float> weightMap;

    void Awake()
    {
        Instance = this;
        weightMap = new Dictionary<string, float>();

        foreach (var pair in itemWeights)
            weightMap[pair.itemId] = pair.weight;
    }

    public float GetItemWeight(ItemData item)
    {
        if (item == null) return 0;
        return weightMap.TryGetValue(item.itemId, out float w) ? w : 1f;
    }

    public float CalculateSpeedPenalty(float totalWeight, float strength)
    {
        float excess = Mathf.Max(0, totalWeight - strength);
        return excess * 0.05f; // 비버 이동공식
    }
}

[System.Serializable]
public class ItemWeightPair
{
    public string itemId;
    public float weight;
}