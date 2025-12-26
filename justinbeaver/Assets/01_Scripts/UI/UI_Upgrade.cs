using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Upgrade : MonoBehaviour
{

    [Header("UI")]
    [SerializeField] private TMP_Text strengthText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text maxWeightText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private Button upgradeButton;


    private void Awake()
    {
        if(upgradeButton != null)
        {
            upgradeButton.onClick.AddListener(OnClickStatUpgradeButton);
        }
    }


    private void OnEnable()
    {
        Refresh();
    }


    public void OnClickStatUpgradeButton()
    {
        var stats = PlayerStatsManager.Instance;
        if (stats == null)
        {
            return;
        }

        bool ok = stats.TryUpgradeStrengthWithMaterials();
        if (!ok)
        {
            UIManager.Instance?.ShowInteractHint(transform, "재료가 부족합니다");
        }

        Refresh();
        UIManager.Instance?.RefreshWeightGauge(); //MaxWeight 변경 반영
    }


    public void OnClickHouseUpgradeButton()
    {
        if (HomeManager.Instance == null)
        {
            Debug.LogWarning("HomeManager not found");
            return;
        }

        if (!HomeManager.Instance.CanUpgrade())
        {
            Debug.Log("House already at MAX level");
            return;
        }

        bool success = HomeManager.Instance.TryUpgradeHome();

        if (success)
        {
            Debug.Log($"House upgrade Lv.{HomeManager.Instance.CurrentLevel}");
        }
        else
        {
            Debug.Log("House upgrade failed");
        }
    }


    public void OnClickExitButton()
    {
        UIManager.Instance.HideUpgradeUI();
    }

    public void Refresh()
    {
        var stats = PlayerStatsManager.Instance;
        if (stats == null)
        {
            return;
        }    

        if (strengthText != null)
        {
            strengthText.text = $"Strength: {stats.Strength:0.0}";
        }

        if (levelText != null)
        {
            levelText.text = $"Lv: {stats.StrengthLevel}";
        }

        if (maxWeightText != null)
        {
            maxWeightText.text = $"MaxWeight: {stats.MaxWeight:0.0}";
        }

        var next = stats.GetNextStrengthUpgradeData();
        if (next == null)
        {
            if (costText != null)
            {
                costText.text = "MAX";
            }    

            if (upgradeButton != null)
            {
                upgradeButton.interactable = false;
            }
            return;
        }

        //비용 문자열 만들기
        if (costText != null)
        {
            costText.text = BuildCostText(next.requiredMaterials);
        }

        bool canPay = (StorageManager.Instance != null) &&
                      StorageManager.Instance.CheckSufficientItems(next.requiredMaterials);

        if (upgradeButton != null)
        {
            upgradeButton.interactable = canPay;
        }
    }

    private string BuildCostText(MaterialCost[] costs)
    {
        if (costs == null || costs.Length == 0)
        {
            return "Free";
        }

        //ItemManager에서 itemId -> ItemData displayName 가져오기
        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
        stringBuilder.Append("Cost: ");
        for (int i = 0; i < costs.Length; i++)
        {
            var cost = costs[i];
            stringBuilder.Append($"{cost.item} x{cost.amount}");
            if (i < costs.Length - 1)
            {
                stringBuilder.Append(", ");
            }
        }
        return stringBuilder.ToString();
    }
}
