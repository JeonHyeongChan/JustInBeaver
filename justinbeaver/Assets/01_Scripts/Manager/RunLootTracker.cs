using System.Collections.Generic;
using UnityEngine;

public class RunLootTracker : MonoBehaviour
{
    public static RunLootTracker Instance;

    private Dictionary<string, int> runLoot = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Clear()
    {
        runLoot.Clear();
    }

    public void AddItem(string itemId, int amount = 1)
    {
        if (string.IsNullOrEmpty(itemId) || amount <= 0)
            return;

        if (!runLoot.ContainsKey(itemId))
            runLoot[itemId] = 0;

        runLoot[itemId] += amount;
    }

    public Dictionary<string, int> GetResult()
    {
        return runLoot;
    }
}
