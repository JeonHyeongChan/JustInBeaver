using UnityEngine;
using System.Collections.Generic;

public class WeightManager : MonoBehaviour
{
    public static WeightManager Instance;

    [System.Serializable]
    public class WeightEntry
    {
        public string itemId;
        public float weight;
    }

    [Header("Item Weight Table")]
    public List<WeightEntry> weightTable = new();

    Dictionary<string, float> weightDict = new();

    void Awake()
    {
        Instance = this;

        foreach (var entry in weightTable)
        {
            if (!string.IsNullOrEmpty(entry.itemId))
                weightDict[entry.itemId] = entry.weight;
        }
    }

    public float GetWeight(ItemData item)
    {
        if (item == null) return 0f;

        if (weightDict.TryGetValue(item.itemId, out float w))
            return w;

        return 0f; // 등록 안 된 아이템은 무게 0
    }
}