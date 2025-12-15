using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    public GameObject root;

    public void Toggle()
    {
        root.SetActive(!root.activeSelf);
    }

    public void UpdateWeight(float current, float max)
    {
        // TMP 텍스트 갱신
    }
}