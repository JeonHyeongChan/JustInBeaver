using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_HUD : MonoBehaviour
{
    public Slider hpBar;
    public TextMeshProUGUI powerText;
    public TextMeshProUGUI weightText;

    public void UpdateHUD(PlayerStats stats)
    {
        hpBar.value = stats.CurrentHP / stats.MaxHP;
        powerText.text = stats.Power.ToString();
        weightText.text = $"{stats.CurrentWeight}/{stats.MaxWeight}";
    }
}
