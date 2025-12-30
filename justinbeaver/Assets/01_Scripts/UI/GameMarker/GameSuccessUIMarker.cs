using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameSuccessUIMarker : MonoBehaviour
{
    public string[] itemIds;
    public TextMeshProUGUI[] amountTexts;

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        var data = RunLootTracker.Instance?.GetResult();
        if (data == null) return;

        int n = Mathf.Min(itemIds.Length, amountTexts.Length);

        for (int i = 0; i < n; i++)
        {
            if (amountTexts[i] == null) continue;

            data.TryGetValue(itemIds[i], out int amount);
            amountTexts[i].text = amount.ToString();
        }
    }
}
