using UnityEngine;
using UnityEngine.UI;

public class UI_InventorySlot : MonoBehaviour
{
    Image bgImage;

    private readonly Color normalColor = Color.white;
    private readonly Color selectedColor = Color.cyan;

    [Header("Item")]
    [SerializeField] private string itemId;

    public Image icon;
    private bool hideIconWhenEmpty = true;


    void Awake()
    {
        bgImage = GetComponent<Image>();

        if (icon == null)
        {
            icon = GetComponentInChildren<Image>(true);
        }
        RefreshUI();
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
        return !string.IsNullOrEmpty(itemId);
    }

    public string GetItemId()
    {
        return itemId;
    }


    //슬롯에 아이템 저장(아이템 데이터는 DB에서 가져옴)
    public void SetItemId(string newItemId)
    {
        itemId = newItemId;
        RefreshUI();
    }

    public void Clear()
    {
        itemId = null;
        RefreshUI();
    }
    


    //DB 기준으로 슬롯 아이콘/표시를 갱신
    public void RefreshUI()
    {
        if (icon == null) return;

        if (string.IsNullOrEmpty(itemId))
        {
            icon.sprite = null;
            icon.enabled = !hideIconWhenEmpty;
            return;
        }

        //ItemManager -> ItemDatabase에서 조회
        var data = ItemManager.Instance != null ? ItemManager.Instance.GetItem(itemId) : null;

        if (data == null || data.icon == null)
        {
            icon.sprite = null;
            icon.enabled = false;
            return;
        }

        icon.sprite = data.icon;
        icon.enabled = true;
        icon.color = Color.white;
    }


    //아이템 빼기
    public string PopItemId()
    {
        if (string.IsNullOrEmpty(itemId))
        {
            return null;
        }
        
        string id = itemId;
        itemId = null;
        
        RefreshUI();
        return id;
    }
}