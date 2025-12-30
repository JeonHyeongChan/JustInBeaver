using TMPro;
using UnityEngine;

public class UI_EscapeResult : MonoBehaviour
{
    public TextMeshProUGUI rewardText;

    public void Show(int reward, bool success)
    {
        gameObject.SetActive(true);
        rewardText.text = success
            ? $"획득 재화: {reward}"
            : "탈출 실패";
    }
}