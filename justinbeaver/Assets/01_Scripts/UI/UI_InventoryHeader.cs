using TMPro;
using UnityEngine;

public class UI_InventoryHeader : MonoBehaviour
{
    public TextMeshProUGUI weightText;

    public void UpdateWeight(float cur, float max)
    {
        weightText.text = $"{cur} / {max}";
    }
}
