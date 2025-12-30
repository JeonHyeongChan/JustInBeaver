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

    //이벤트
    public event Action<int> OnHouseLevelChanged;

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
        int nextLevel = currentLevel + 1;

        foreach (var data in upgradeData)
        {
            if (data != null && data.targetLevel == nextLevel)
                return data;
        }

        return null;
    }

    public bool TryUpgradeHome()
    {
        if (!CanUpgrade())
            return false;
        
        HouseUpgradeData data = GetNextUpgradeData();
        
        if (data == null)
            return false;
        if (StorageManager.Instance == null)  //창고 매니저 연동구간 
            return false;
        if (!StorageManager.Instance.CheckSufficientItems(data.requiredMaterials))
            return false;
        
        StorageManager.Instance.ConsumeItems(data.requiredMaterials);  //여기까지 
        
        currentLevel = data.targetLevel;

        SoundManager.Instance?.PlaySFX(SFXType.HouseUpgrade);

        ApplyHouseLevel(currentLevel);

        UIManager.Instance.HideUpgradeUI();

        return true;
    }

    private void ApplyHouseLevel(int level)
    {
        Debug.Log($"House Upgrade -> {level}");        
        
        ApplyFarmingItems(level);

        RuleManager.Instance?.ResetFailCount();

        OnHouseLevelChanged?.Invoke(level); // 이벤트

        GameManager.Instance?.SaveGame();

        //엔딩 NPC 활성화 체크
        if (level >= MaxLevel)
        {
            RuleManager.Instance?.ForceEndingReady();
        }
    }

    private void ResetHouseLevel()
    {
        Debug.Log($"House level Reset");

        currentLevel = 1;

        SoundManager.Instance?.PlaySFX(SFXType.Reset);

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

    public void SetLevel(int level)
    {
        currentLevel = Mathf.Clamp(level, 1, MaxLevel);
        ApplyHouseLevel(currentLevel);
    }

    private void CheckEndingNPC(int level)
    {
        //if (level >= 5)
        //    .SetActive(true);
        
    }  
}
