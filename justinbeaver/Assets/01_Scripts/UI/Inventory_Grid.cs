using UnityEngine;
using UnityEngine.UI;

public class Inventory_Grid : MonoBehaviour
{
    [Header("Grid")]
    public int columnCount = 3;
    public UI_InventorySlot[] slots;

    [Header("Scroll")]
    public ScrollRect scrollRect;

    private RectTransform contentRT;
    private RectTransform viewportRT;

    int selectedIndex = 0;


    void Awake()
    {
        if (scrollRect != null)
        {
            if (scrollRect == null)
            {
                scrollRect = GetComponentInParent<ScrollRect>(true);
            }
            contentRT = scrollRect.content;
            viewportRT = scrollRect.viewport;

            
        }
    }


    void OnEnable()
    {
        if (slots == null || slots.Length == 0)
        {
            return;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetSelected(false);
        }

        Select(0);
    }



    //이동
    public void Move(int x, int y)
    {
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
        if (index < 0 || index >= slots.Length)
        {
            return;
        }
            
        slots[selectedIndex].SetSelected(false);
        selectedIndex = index;
        slots[selectedIndex].SetSelected(true);

        ScrollToSelectedSlot();
    }




    //버릴 아이템 선택
    public void DropSelectedItem()
    {
        UI_InventorySlot slot = slots[selectedIndex];

        if (!slot.HasItem())
        {
            Debug.Log(" 버릴 아이템 없음");
            return;
        }
        Debug.Log($" 아이템 버림 : {slot.GetItemName()}");
        slot.Clear();
    }




    //슬롯 선택 스크롤
    void ScrollToSelectedSlot()
    {
        if (scrollRect == null || contentRT == null || viewportRT == null)
        {
            return;
        }

        //int visibleRowCount = 3; // 화면에 보이는 행 (3x3)
        int totalRowCount = Mathf.CeilToInt((float)slots.Length / columnCount);
        int currentRow = selectedIndex / columnCount;


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
        float targetCenterY = currentRow * rowHeight + rowHeight * 0.5f;


        //뷰포트 중앙 위치
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
}