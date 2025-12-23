using TMPro;
using UnityEngine;

public class UI_StorageBar : MonoBehaviour
{
    public string[] itemIds;
    public TextMeshProUGUI[] amountTexts;
    private void OnEnable()
    {
        if (StorageManager.Instance != null)
        {
            StorageManager.Instance.OnStorageChanged += Refresh;
        }
        Refresh();
    }

    private void OnDisable()
    {
        if (StorageManager.Instance != null)
        {
            StorageManager.Instance.OnStorageChanged -= Refresh;
        }
    }


    public void Refresh()
    {
        if (StorageManager.Instance == null) return;
        if (itemIds == null || amountTexts == null) return ;

        int n = Mathf.Min(itemIds.Length, amountTexts.Length);

        for (int i = 0; i < n; i++)
        {
            if (amountTexts[i] == null) continue;

            int amount = StorageManager.Instance.GetItemAmount(itemIds[i]);
            amountTexts[i].text = amount.ToString();
        }
    }
}
