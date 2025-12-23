using System;
using UnityEngine;

public class HomeManager : MonoBehaviour
{
    public static HomeManager Instance;

    [Header("House Level")]
    [SerializeField, Range(1, 5)]
    private int currentLevel = 1;
    public int CurrentLevel => currentLevel;
    public const int MaxLevel = 5;

    [Header("Upgrade Data")]
    [SerializeField] private HouseUpgradeData[] upgradeData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        if (RuleManager.Instance != null)
        {
            RuleManager.Instance.OnTotalResetRequired += ResetHouseLevel;
            RuleManager.Instance.OnEndingCondition += OnEndingReady;
        }
    }

    private void OnDisable()
    {
        if (RuleManager.Instance != null)
        {
            RuleManager.Instance.OnTotalResetRequired -= ResetHouseLevel;
            RuleManager.Instance.OnEndingCondition -= OnEndingReady;
        }
    }

    public bool CanUpgrade()
    {
        return currentLevel < MaxLevel;
    }

    public HouseUpgradeData GetNextUpgradeData()
    {
        if (!CanUpgrade())
            return null;

        return upgradeData[currentLevel];
    }

    public bool TryUpgradeHome()
    {
        if (!CanUpgrade())
            return false;

        HouseUpgradeData data = GetNextUpgradeData();
        
        if (data == null)
            return false;
        if (StorageManager.Instance == null)
            return false;
        if (!StorageManager.Instance.CheckSufficientItems(data.requiredMaterials))
            return false;

        StorageManager.Instance.ConsumeItems(data.requiredMaterials);

        currentLevel++;
        ApplyHouseLevel(currentLevel);

        return true;
    }

    private void ApplyHouseLevel(int level)
    {
        Debug.Log($"House Upgrade -> {level}");

        //외형 변경

        //파밍 확장
        ApplyFarmingItems(level);

        RuleManager.Instance?.ResetFailCount();

        //엔딩 NPC 활성화 체크                
    }

    private void ResetHouseLevel()
    {
        Debug.Log($"House level Reset");

        currentLevel = 1;
        ApplyHouseLevel(currentLevel);
    }
    
    private void OnEndingReady()
    {
        Debug.Log($"Ending Ready!!!");
    }

    private void ApplyFarmingItems(int level)
    {
        //ObjectManager
    }    

    private void CheckEndingNPC(int level)
    {
        //if (level >= 5)
        //    .SetActive(true);
        
    }

    //private bool HasEnoughMaterials(HouseUpgradeData data)
    //{
    //
    //}

    //private void ConsumeMaterials(HouseUpgradeData data)
    //{
    //
    //}
}
