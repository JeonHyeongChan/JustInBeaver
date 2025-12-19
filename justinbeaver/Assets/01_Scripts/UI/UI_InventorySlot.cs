using UnityEngine;
using UnityEngine.UI;

public class UI_InventorySlot : MonoBehaviour
{
    [Header("UI")]
    public Image highlight; // 선택 테두리
    public Image icon;      // 아이템 이미지

    [Header("Item")]
    [SerializeField] string itemName;

    public void SetSelected(bool selected)
    {
        if (highlight != null)
            highlight.enabled = selected;
    }

    public bool HasItem()
    {
        return !string.IsNullOrEmpty(itemName);
    }

    public string GetItemName()
    {
        return itemName;
    }

    public void Clear()
    {
        itemName = null;

        if (icon != null)
            icon.enabled = false;
    }
}