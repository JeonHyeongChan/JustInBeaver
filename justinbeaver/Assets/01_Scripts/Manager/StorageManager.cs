using System;
using System.Collections.Generic;
using UnityEngine;

public class StorageManager : MonoBehaviour
{
    public static StorageManager Instance;

    [SerializeField] private ItemDatabase itemDatabase; // id -> itemData

    private Dictionary<ItemData, int> storageItems = new Dictionary<ItemData, int>();
    public event Action OnStorageChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Debug.Log($"[StorageManager] Awake {GetInstanceID()}");

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
        OnStorageChanged?.Invoke();
    }

    /// <summary>
    /// string -> itemData로 변환
    /// </summary>
    /// <param name="itemId"></param>
    /// <returns></returns>
    private ItemData Resolve(string itemId)
    {
        if (string.IsNullOrEmpty(itemId))
            return null;

        //ItemManager 우선
        if (ItemManager.Instance != null)
        {
            var data = ItemManager.Instance.GetItem(itemId);
            if (data != null) return data;
        }

        //ItemDatabase
        if (itemDatabase != null)
        {
            var data = itemDatabase.Find(itemId);
            if (data != null) return data;
        }
        
        return null;
    }

    public void AddItems(ItemData item, int amount)  //아이템 추가
    {
        if (item == null || amount <= 0) return;

        if (!storageItems.ContainsKey(item))
            storageItems[item] = 0;

        storageItems[item] += amount;
        OnStorageChanged?.Invoke();

        Debug.Log($"[StorageManager] Add {item.itemId} x{amount} (총량: {storageItems[item]})");
    }

    public void AddItems(string itemId, int amount)
    {
        var item = Resolve(itemId);
        if (item == null)               
            return;

        AddItems(item, amount);
    }

    public int GetItemAmount(ItemData item)  //아이템 개수 조회
    {
        if (item == null)
            return 0;

        return storageItems.TryGetValue(item, out int amount) ? amount : 0;
    }

    public int GetItemAmount(string itemId)
    {
        var item = Resolve(itemId);
        return GetItemAmount(item);
    }

    public bool CheckSufficientItems(MaterialCost[] costs)  //업그레이드 재료 충분한지 체크
    {
        foreach (var cost in costs)
        {
            if (cost.item == null)
                return false;

            if (GetItemAmount(cost.item) < cost.amount)
                return false;
        }
        return true;
    }
    public void ConsumeItems(MaterialCost[] costs)  //업그레이드 재료 소모
    {
        foreach (var cost in costs)
        {
            if (cost.item == null)
                continue;

            if (!storageItems.ContainsKey(cost.item))
                continue;

            storageItems[cost.item] -= cost.amount;

            if (storageItems[cost.item] <= 0)
                storageItems.Remove(cost.item);
        }

        OnStorageChanged?.Invoke();
    }
}
