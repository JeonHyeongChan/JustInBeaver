using UnityEngine;
using UnityEngine.UI;

public class UI_InventorySlot : MonoBehaviour
{
    Image bgImage;

    Color normalColor = Color.white;
    Color selectedColor = Color.cyan;

    [Header("Item")]
    [SerializeField] string itemName;

    [Header("UI")]
    public Image icon;



    void Awake()
    {
        bgImage = GetComponent<Image>();
    
        //if (bgImage == null)
        //{
        //    Debug.LogError($"{name} : Image 컴포넌트가 없음!");
        //}
    }


    public void SetSelected(bool selected)
    {
        if (bgImage == null)
        {
            return;
        }
        bgImage.color = selected ? selectedColor : normalColor;
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
        {
            icon.enabled = false;
        }
    }
}