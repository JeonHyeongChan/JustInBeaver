using TMPro;
using UnityEngine;

public class UI_StorageBar : MonoBehaviour
{
    public string[] itemIds;
    public TextMeshProUGUI[] amountTexts;
    private void OnEnable()
    {
        Refresh();
   
    }

    private void Update()
    {
        Refresh();
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
