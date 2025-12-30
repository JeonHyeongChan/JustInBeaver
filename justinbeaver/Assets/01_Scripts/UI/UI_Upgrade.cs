using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class UI_Upgrade : MonoBehaviour
{
    [Header("Strength UI")]
    public TMP_Text gatherSpeedText;     // 갈무리 속eh 힘
    public TMP_Text strengthLevelText;   // 힘 Lv
    public TMP_Text strengthCostText;    // 힘 업그레이드 재료
    public Button strengthUpgradeButton; // 힘 강화 버튼

    [Header("House UI")]
    public TMP_Text houseLevelText;      // 집 Lv
    public TMP_Text houseCostText;       // 집 업그레이드 재료
    public Button houseUpgradeButton;    // 집 업글 버튼

    [Header("exit")]
    public Button exitButton;            // 닫기 버튼

    private void Awake()
    {
        if (strengthUpgradeButton != null)
            strengthUpgradeButton.onClick.AddListener(OnClickStrengthUpgrade);

        if (houseUpgradeButton != null)
            houseUpgradeButton.onClick.AddListener(OnClickHouseUpgrade);

        if (exitButton != null)
            exitButton.onClick.AddListener(OnClickExit);
    }

    private void OnEnable()
    {
        Refresh();
    }

    private void OnClickStrengthUpgrade()
    {
        var stats = PlayerStatsManager.Instance;
        if (stats == null) return;

        stats.TryUpgradeStrengthWithMaterials();

        Refresh();
        UIManager.Instance?.RefreshWeightGauge();
    }

    private void OnClickHouseUpgrade()
    {
        var home = HomeManager.Instance;
        if (home == null) return;

        home.TryUpgradeHome(); 
        Refresh();
    }

    private void OnClickExit()
    {
        UIManager.Instance?.HideUpgradeUI();
    }

    public void Refresh()
    {
        RefreshStrengthUI();
        RefreshHouseUI();
    }

    private void RefreshStrengthUI()
    {
        var stats = PlayerStatsManager.Instance;

        if (stats == null)
        {
            SetText(gatherSpeedText, "Gather Speed: -");
            SetText(strengthLevelText, "힘 Lv: 1");
            SetText(strengthCostText, "Wood x3");
            if (strengthUpgradeButton) strengthUpgradeButton.interactable = false;
            return;
        }

        // 현재 수치
        SetText(gatherSpeedText, $"Gather Speed: {stats.Strength:0.0}");
        SetText(strengthLevelText, $"힘 Lv: {stats.StrengthLevel}");

        // 다음 업그레이드 
        var next = stats.GetNextStrengthUpgradeData();
        if (next == null)
        {
            SetText(strengthCostText, "MAX");
            if (strengthUpgradeButton) strengthUpgradeButton.interactable = false;
            return;
        }

        // 재료 텍스트 (0개 재료 제외)
        SetText(strengthCostText, BuildCostText(next.requiredMaterials));

        bool canPay =
            StorageManager.Instance != null &&
            StorageManager.Instance.CheckSufficientItems(next.requiredMaterials);

        if (strengthUpgradeButton)
            strengthUpgradeButton.interactable = canPay;
    }


    private void RefreshHouseUI()
    {
        var home = HomeManager.Instance;

        if (home == null)
        {
            SetText(houseLevelText, "집 Lv: 0");
            SetText(houseCostText, "House Materials: 0");
            if (houseUpgradeButton) houseUpgradeButton.interactable = false;
            return;
        }

        SetText(houseLevelText, $"집 Lv: {home.CurrentLevel}");

        var next = home.GetNextUpgradeData();
        if (next == null)
        {
            SetText(houseCostText, "House Materials: MAX");
            if (houseUpgradeButton) houseUpgradeButton.interactable = false;
            return;
        }

        SetText(houseCostText, BuildCostText(next.requiredMaterials));

        bool canPay =
            StorageManager.Instance != null &&
            StorageManager.Instance.CheckSufficientItems(next.requiredMaterials);

        if (houseUpgradeButton) houseUpgradeButton.interactable = canPay;
    }

    private void SetText(TMP_Text t, string value)
    {
        if (t != null) t.text = value;
    }

    private string BuildCostText(MaterialCost[] costs)
    {
        if (costs == null || costs.Length == 0)
            return "Free";

        StringBuilder sb = new StringBuilder();
        int printed = 0;

        for (int i = 0; i < costs.Length; i++)
        {
            var c = costs[i];

            //수량 0 이하 또는 아이템 없으면 스킵
            if (c.amount <= 0 || c.item == null)
                continue;

            string name = "-";
            if (!string.IsNullOrEmpty(c.item.displayName))
                name = c.item.displayName;
            else if (!string.IsNullOrEmpty(c.item.itemName))
                name = c.item.itemName;
            else if (!string.IsNullOrEmpty(c.item.itemId))
                name = c.item.itemId;

            // 콤마 처리
            if (printed > 0)
                sb.Append(", ");

            sb.Append(name);
            sb.Append(" x");
            sb.Append(c.amount);

            printed++;
        }

        //전부 0개였던 경우
        if (printed == 0)
        {
            return "Free";
        }

        return sb.ToString();
    }

}
