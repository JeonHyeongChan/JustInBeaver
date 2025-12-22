using System.Collections.Generic;
using UnityEngine;

public class StorageManager : MonoBehaviour
{
    public static StorageManager Instance;
    private Dictionary<string, int> storageItems = new Dictionary<string, int>();


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

    private void OnEnable()
    {
        if (RuleManager.Instance != null)
        {
            RuleManager.Instance.OnTotalResetRequired += OnTotalReset;
        }
    }
    private void OnDisable()
    {
        if (RuleManager.Instance != null)
        {
            RuleManager.Instance.OnTotalResetRequired -= OnTotalReset;
        }
    }
    private void OnTotalReset()
    {
        //전체 초기화: 창고에 저장된 모든 재료를 0으로 만듬
        storageItems.Clear();
        Debug.Log("[StorageManager] Total Reset => storageItems cleared");
    }

    public void AddItems(string itemid, int amount)  //아이템 추가
    {
        if (string.IsNullOrEmpty(itemid) || amount <= 0)
            return;
        if (!storageItems.ContainsKey(itemid))
        {
            storageItems[itemid] = 0;
        }
        storageItems[itemid] += amount;
        Debug.Log($"[StorageManager] Add {itemid} x{amount} (총량: {storageItems[itemid]})");
    }
    public int GetItemAmount(string itemid)  //아이템 개수 조회
    {
        if (storageItems.TryGetValue(itemid, out int amount))
        {
            return amount;
        }
        return 0;
    }

    public bool CheckSufficientItems(MaterialCost[] costs)  //업그레이드 재료 충분한지 체크
    {
        foreach (var cost in costs)
        {
            if (GetItemAmount(cost.itemId) < cost.amount)
            {
                return false;
            }
        }
        return true;
    }
    public void ConsumeItems(MaterialCost[] costs)  //업그레이드 재료 소모
    {
        foreach (var cost in costs)
        {
            if (!storageItems.ContainsKey(cost.itemId))
                continue;

            storageItems[cost.itemId] -= cost.amount;

            if (storageItems[cost.itemId] <= 0)
            {
                storageItems.Remove(cost.itemId);
            }
            Debug.Log($"[StorageManager] Consume {cost.itemId} x{cost.amount}");
        }
    }


}
