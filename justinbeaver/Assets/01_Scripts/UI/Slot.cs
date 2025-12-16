using UnityEngine;
using UnityEngine.UI;

public class UI_InventorySlot : MonoBehaviour
{
    [SerializeField] private Image icon;

    public void Set(InventoryItem item)
    {
        icon.enabled = true;
        icon.sprite = GetSprite(item.type);
    }

    public void Clear()
    {
        icon.enabled = false;
    }

    Sprite GetSprite(MaterialType type)
    {
        // ScriptableObject or Dictionary ±«¿Â
        return InventoryIconTable.Get(type);
    }
}