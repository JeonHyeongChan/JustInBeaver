using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerHearts : MonoBehaviour
{
    [Header("플레이어 하트")]
    [SerializeField] private Image[] hearts;

    [Header("하트 색상")]
    [SerializeField] private Color fullColor = Color.red;       //기본 빨간색
    [SerializeField] private Color emptyColor = Color.black;    //피격 시 검정색


    //CurrentHealth를 기준으로 하트 색상 갱신
    public void SetHeart(int curent, int max)
    {
        if (hearts == null || hearts.Length == 0)
        {
            return;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] == null)
            {
                continue;
            }
            hearts[i].color = (i < curent) ? fullColor : emptyColor;
        }
    }
}
