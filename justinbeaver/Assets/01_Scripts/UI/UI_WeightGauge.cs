using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_WeightGauge : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image fillImage;
    [SerializeField] private TMP_Text weightText;

    [Header("Format")]
    [SerializeField] private bool showOneDecimal = true;

    public void Refresh(float currentWeight, float maxWeight)
    {
        float max = Mathf.Max(1f, maxWeight);
        float ratio = Mathf.Clamp01(currentWeight / max);

        if (fillImage != null)
        {
            fillImage.fillAmount = ratio;
        }

        if (weightText != null)
        {
            string curStr = showOneDecimal ? currentWeight.ToString("0.0") : Mathf.RoundToInt(currentWeight).ToString();
            string maxStr = showOneDecimal ? max.ToString("0.0") : Mathf.RoundToInt(max).ToString();
            weightText.text = $"{curStr} / {maxStr}";
        }
    }
}
