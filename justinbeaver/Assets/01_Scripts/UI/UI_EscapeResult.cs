using TMPro;
using UnityEngine;

public class UI_EscapeResult : MonoBehaviour
{
    public TextMeshProUGUI rewardText;

    public void Show(int reward, bool success)
    {
        gameObject.SetActive(true);
        rewardText.text = success
            ? $"»πµÊ ¿Á»≠: {reward}"
            : "≈ª√‚ Ω«∆–";
    }
}