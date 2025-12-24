using UnityEngine;
using UnityEngine.UI;

public class Inventory_Grid : MonoBehaviour
{
    [Header("Grid")]
    public int columnCount = 3;

    [Header("Slots")]
    public UI_InventorySlot[] slots;

    [Header("Scroll")]
    public ScrollRect scrollRect;

    private RectTransform contentRT;
    private RectTransform viewportRT;

    int selectedIndex = 0;

    [Header("Options")]
    [SerializeField] private bool autoBindSlotsOnEnable = true;
    [SerializeField] private bool includeInactiveSlots = true;


    private void Awake()
    {
        BindScrollRefs();
        
        if (slots == null || slots.Length == 0)
        {
            RebindSlots();
        }
    }


    private void OnEnable()
    {
        if (autoBindSlotsOnEnable)
        {
            BindScrollRefs();
            RebindSlots();
        }

        if (slots == null || slots.Length == 0)
        {
            return;
        }

        //선택 초기화
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null)
            {
                slots[i].SetSelected(false);
            }
        }

        selectedIndex = Mathf.Clamp(selectedIndex, 0, slots.Length - 1);
        if (slots[selectedIndex] != null)
        {
            slots[selectedIndex].SetSelected(true);
        }
        ScrollToSelectedSlot();
    }


    //이동
    public void Move(int x, int y)
    {
        if (slots == null || slots.Length == 0)
        {
            return;
        }

        int row = selectedIndex / columnCount;
        int col = selectedIndex % columnCount;

        int nextRow = row + y;
        int nextCol = col + x;

        if (nextCol < 0 || nextCol >= columnCount)
        {
            return;
        }

        int nextIndex = nextRow * columnCount + nextCol;
        if (nextIndex < 0 || nextIndex >= slots.Length)
        {
            return;
        }
        Select(nextIndex);
    }


    //선택
    void Select(int index)
    {
        if (slots == null || slots.Length == 0)
        {
            return;
        }    
            
        if (index < 0 || index >= slots.Length)
        {
            return;
        }

        if (slots[selectedIndex] != null)
        {
            slots[selectedIndex].SetSelected(false);
        }

        selectedIndex = index;

        if (slots[selectedIndex] != null)
        {
            slots[selectedIndex].SetSelected(true);
        }
        ScrollToSelectedSlot();
    }


    //버릴 아이템 선택
    public void DropSelectedItem()
    {
        var slot = slots[selectedIndex];
        if (slot == null || !slot.HasItem())
        {
            Debug.Log("버릴 아이템 없음");
            return;
        }    
        string itemId = slot.GetItemId();

        // 1개만 차감
        slot.TryConsume(1);
        UpdatePlayerWeight();

        if (string.IsNullOrEmpty(itemId))
        {
            return;
        }

        //드랍 위치
        var player = FindAnyObjectByType<PlayerController>(FindObjectsInactive.Exclude);
        Vector3 dropPos = player != null ? player.transform.position : Vector3.zero;

        if (player != null)
        {
            dropPos += player.transform.forward * 1.0f + Vector3.up * 0.2f;
        }

        ItemManager.Instance.SpawnPickupByItemId(itemId, dropPos, Quaternion.identity);
        UIManager.Instance?.ShowItemTooltip(slot);
    }


    //빈 슬롯에 아이템 추가
    public bool TryAddItem(string itemId, int addCount = 1)
    {
        if (slots == null || slots.Length == 0)
        {
            return false;
        }

        if (string.IsNullOrEmpty(itemId) || addCount <= 0)
        {
            return false;
        }

        var data = ItemManager.Instance != null ? ItemManager.Instance.GetItem(itemId) : null;
        bool stackable = data != null && data.stackable;
        int maxStack = data != null ? Mathf.Max(1, data.maxStack) : 1;

        //스택
        if (stackable)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                var slot = slots[i];
                if (slot == null)
                {
                    continue;
                }    
                    
                if (slot.HasItem() && slot.GetItemId() == itemId && slot.GetCount() < maxStack)
                {
                    int before = slot.GetCount();
                    slot.TryAddToStack(itemId, addCount, maxStack);
                    
                    int used = slot.GetCount() - before;
                    addCount -= used;
                    
                    if (addCount <= 0)
                    {
                        UpdatePlayerWeight();
                        return true;
                    }
                }
            }
        }

        //남은 수량을 빈 슬롯에 분배
        for (int i = 0; i < slots.Length; i++)
        {
            var s = slots[i];
            if (s == null)
            {
                continue;
            }    
                
            if (!s.HasItem())
            {
                int put = stackable ? Mathf.Min(maxStack, addCount) : 1;
                s.SetItem(itemId, put);
                addCount -= put;
                
                if (addCount <= 0)
                {
                    return true;
                }
            }
        }

        Debug.Log("인벤토리 가득 참");
        return false;
    }


    //슬롯 선택 스크롤
    void ScrollToSelectedSlot()
    {
        if (slots == null || slots.Length == 0)
        {
            return;
        }    
            
        if (scrollRect == null || contentRT == null || viewportRT == null)
        {
            return;
        }

        //레이아웃 갱신
        Canvas.ForceUpdateCanvases();

        //슬롯 RectTransform 높이 사용
        var slotRT = slots[selectedIndex].GetComponent<RectTransform>();
        if (slotRT == null)
        {
            return;
        }

        //GridLayoutGroup y값 반영
        float cellHeight = slotRT.rect.height;
        var grid = contentRT.GetComponent<GridLayoutGroup>();
        float spacingY = grid != null ? grid.spacing.y : 0f;

        float rowHeight = cellHeight + spacingY;
        
        float contentHeight = contentRT.rect.height;
        float viewportHeight = viewportRT.rect.height;

        //스크롤 필요 없음
        if (contentHeight <= viewportHeight + 0.01f)
        {
            return;
        }

        //뷰포트 중앙 위치
        int currentRow = selectedIndex / columnCount;
        float targetCenterY = currentRow * rowHeight + rowHeight * 0.5f;
        float viewportCenterY = viewportHeight * 0.5f;

        //목표값
        float desiredContentY = targetCenterY - viewportCenterY;

        //가능한 범위로 clamp
        float maxContentY = Mathf.Max(0f, contentHeight - viewportHeight);
        desiredContentY = Mathf.Clamp(desiredContentY, 0f, maxContentY);

        //적용
        Vector2 pos = contentRT.anchoredPosition;
        pos.y = desiredContentY;
        contentRT.anchoredPosition = pos;
    }

    private void BindScrollRefs()
    {
        if (scrollRect == null)
        {
            scrollRect = GetComponentInParent<ScrollRect>(true);
        }

        if (scrollRect == null)
        {
            contentRT = null;
            viewportRT = null;
            return;
        }

        contentRT = scrollRect.content;
        viewportRT = scrollRect.viewport != null
            ? scrollRect.viewport
            : scrollRect.GetComponent<RectTransform>(); //예외 방지
    }


    public void RebindSlots()
    {
        slots = GetComponentsInChildren<UI_InventorySlot>(includeInactiveSlots);
        if ((slots == null || slots.Length == 0) && contentRT != null)
        {
            slots = contentRT.GetComponentsInChildren<UI_InventorySlot>(includeInactiveSlots);
        }
    }

    void UpdatePlayerWeight()
    {
        float totalWeight = 0f;

        foreach (var slot in slots)
        {
            if (slot == null || !slot.HasItem())
                continue;

            var itemData = ItemManager.Instance.GetItem(slot.GetItemId());
            if (itemData == null)
                continue;

            float itemWeight = WeightManager.Instance.GetItemWeight(itemData);
            totalWeight += itemWeight * slot.GetCount();
        }

        PlayerStatsManager.Instance.SetCurrentWeight(totalWeight);
    }
}