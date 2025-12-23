using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UI_InventorySlot : MonoBehaviour
{
    Image bgImage;

    private readonly Color normalColor = Color.white;
    private readonly Color selectedColor = Color.cyan;

    [Header("Item")]
    [SerializeField] private string itemId;
    [SerializeField] private int count;

    public Image icon;

    [Header("Count UI")]
    public TMP_Text countText;
    
    private bool hideCountWhenOne = true;
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

        if (selected)
        {
            UIManager.Instance?.ShowItemTooltip(this);
        }
    }


    public bool HasItem()
    {
        return !string.IsNullOrEmpty(itemId) && count > 0;
    }


    public string GetItemId()
    {
        return itemId;
    }


    public int GetCount()
    {
        return count;
    }    
        

    //슬롯에 아이템 저장(아이템 데이터는 DB에서 가져옴)
    public void SetItem(string newItemId, int newCount)
    {
        itemId = newItemId;
        count = Mathf.Max(0, newCount);
        RefreshUI();
    }


    public void Clear()
    {
        itemId = null;
        count = 0;
        RefreshUI();
    }


    //스택 추가, 성공하면 true
    public bool TryAddToStack(string addItemId, int addCount, int maxStack)
    {
        if (string.IsNullOrEmpty(addItemId) || addCount <= 0)
        {
            return false;
        }

        if (!HasItem())
        {
            itemId = addItemId;
            count = addCount;
            RefreshUI();
            return true;
        }

        if (itemId != addItemId)
        {
            return false;
        }    
            
        int before = count;
        count = Mathf.Min(maxStack, count + addCount);
        bool changed = count != before;
        
        if (changed)
        {
            RefreshUI();
        }
        return changed;
    }


    //1개(또는 N개) 빼기
    public bool TryConsume(int consumeCount = 1)
    {
        if (!HasItem())
        {
            return false;
        }

        consumeCount = Mathf.Max(1, consumeCount);
        count -= consumeCount;
        if (count <= 0)
        {
            Clear();
            return true;
        }

        RefreshUI();
        return true;
    }


    public void RefreshUI()
    {
        if (icon == null)
        {
            return;
        }

        if (!HasItem())
        {
            icon.sprite = null;
            icon.enabled = !hideIconWhenEmpty;

            if (countText != null)
            {
                countText.gameObject.SetActive(false);
            }
            return;
        }

        //ItemManager -> ItemDatabase에서 조회
        var data = ItemManager.Instance != null ? ItemManager.Instance.GetItem(itemId) : null;
        if (data == null || data.icon == null)
        {
            icon.sprite = null;
            icon.enabled = false;

            if (countText != null)
            {
                countText.gameObject.SetActive(false);
            }
            return;
        }

        icon.sprite = data.icon;
        icon.enabled = true;
        icon.color = Color.white;

        if (countText != null)
        {
            bool show = !(hideCountWhenOne && count <= 1);
            countText.gameObject.SetActive(show);

            if (show)
            {
                countText.text = count.ToString();
            }
        }
    }
}