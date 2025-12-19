using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Inventory_Grid : MonoBehaviour
{
    [Header("Grid")]
    public int columnCount = 3;
    public UI_InventorySlot[] slots;

    [Header("Scroll")]
    public ScrollRect scrollRect;

    int selectedIndex = 0;

    void OnEnable()
    {
        if (slots == null || slots.Length == 0) return;

        for (int i = 0; i < slots.Length; i++)
            slots[i].SetSelected(false);

        Select(0);
    }

    void Update()
    {
        if (Keyboard.current == null) return;
    
      // 이동
      if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
          Move(-1, 0);
      if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
          Move(1, 0);
      if (Keyboard.current.upArrowKey.wasPressedThisFrame)
          Move(0, -1);
      if (Keyboard.current.downArrowKey.wasPressedThisFrame)
          Move(0, 1);
    
      // C : 아이템 버리기
      if (Keyboard.current.cKey.wasPressedThisFrame)
      {
          DropSelectedItem();
      }
    
    }
    void Move(int x, int y)
    {
        int row = selectedIndex / columnCount;
        int col = selectedIndex % columnCount;

        int nextRow = row + y;
        int nextCol = col + x;

        if (nextCol < 0 || nextCol >= columnCount)
            return;

        int nextIndex = nextRow * columnCount + nextCol;

        if (nextIndex < 0 || nextIndex >= slots.Length)
            return;

        Select(nextIndex);
    }

    void Select(int index)
    {
        if (index < 0 || index >= slots.Length)
            return;

        slots[selectedIndex].SetSelected(false);
        selectedIndex = index;
        slots[selectedIndex].SetSelected(true);

        ScrollToSelectedSlot();

        Debug.Log($" 선택 슬롯: {selectedIndex}");
    }

    void DropSelectedItem()
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

    void ScrollToSelectedSlot()
    {
        if (scrollRect == null) return;

        int visibleRowCount = 3; // 화면에 보이는 행 (3x3)
        int totalRowCount = Mathf.CeilToInt((float)slots.Length / columnCount);

        if (totalRowCount <= visibleRowCount)
            return; // 스크롤 필요 없음

        int currentRow = selectedIndex / columnCount;

        // 화면 상단 기준 시작 row 계산
        int maxStartRow = totalRowCount - visibleRowCount;
        int startRow = Mathf.Clamp(currentRow, 0, maxStartRow);

        float normalizedY =
            1f - (float)startRow / maxStartRow;

        scrollRect.verticalNormalizedPosition = normalizedY;
    }
}